using D_Squared.Data.Context;
using D_Squared.Data.Millers.Context;
using D_Squared.Data.Millers.Queries;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace D_Squared.Data.Queries
{
    public class SalesForecastQueries
    {
        private readonly D_SquaredDbContext db;
        private readonly ForecastDataDbContext f_db;

        private readonly ForecastDataQueries fdq;

        public SalesForecastQueries(D_SquaredDbContext db, ForecastDataDbContext f_db)
        {
            this.db = db;
            this.f_db = f_db;

            fdq = new ForecastDataQueries(f_db);
        }

        public bool CheckForExistingSalesForecastByDate(DateTime date, string storeNumber)
        {
            return db.SalesForecasts.Any(sf => sf.BusinessDate == date && sf.StoreNumber == storeNumber);
        }

        public SalesForecast GetSalesForecastRecordsByDate(DateTime date, string storeNumber)
        {
            return db.SalesForecasts.Where(sf => sf.BusinessDate == date && sf.StoreNumber == storeNumber).FirstOrDefault();
        }

        public List<SalesForecast> GetSalesForecastsByDate(DateTime date, string storeNumber)
        {
            if (CheckForExistingSalesForecastByDate(date, storeNumber))
                return db.SalesForecasts.Where(sf => sf.BusinessDate == date && sf.StoreNumber == storeNumber).ToList();
            else
                return new List<SalesForecast>();
        }

        public List<DateTime> GetCurrentWeek(DateTime selectedDay)
        {
            int currentDayOfWeek = (int)selectedDay.DayOfWeek;
            DateTime sunday = selectedDay.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            return dates;
        }

        public List<SalesForecastDTO> GetSpecificWeekAsSalesForecastDTOList(DateTime selectedDay, string storeNumber)
        {
            List<SalesForecastDTO> theList = new List<SalesForecastDTO>();

            var dates = GetCurrentWeek(selectedDay);

            foreach(var day in dates)
            {
                if (!CheckForExistingSalesForecastByDate(day, storeNumber))
                    theList.Add(new SalesForecastDTO(day, fdq.GetSalesPriorYear(storeNumber, day), fdq.GetAverageSalesPerMonth(storeNumber, day), fdq.GetLaborForecast(storeNumber, day)));
                else
                    theList.Add(new SalesForecastDTO(GetSalesForecastsByDate(day, storeNumber)));
            }

            return theList;
        }

        public void AddOrUpdateSalesForecasts(List<SalesForecastDTO> forecasts, string storeNumber, string userName)
        {
            foreach(var forecast in forecasts)
            {
                if(CheckForExistingSalesForecastByDate(forecast.DateOfEntry, storeNumber))
                {
                    SalesForecast entry = GetSalesForecastRecordsByDate(forecast.DateOfEntry, storeNumber);
                    entry.ForecastAmount = forecast.ForecastAmount;
                    entry.UpdatedBy = userName;
                    entry.UpdatedDate = DateTime.Now;
                }
                else
                {
                    SalesForecast entry = new SalesForecast()
                    {
                        BusinessDate = forecast.DateOfEntry,
                        StoreNumber = storeNumber,
                        ForecastAmount = forecast.ForecastAmount,
                        ActualPriorYear = forecast.PriorYearSales,
                        AvgPrior4Weeks = forecast.AverageSalesPerMonth,
                        LaborForecast = forecast.LaborForecast,
                        CreatedBy = userName,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = userName,
                        UpdatedDate = DateTime.Now
                    };

                    db.SalesForecasts.Add(entry);
                }
            }

            db.SaveChanges();
        }
    }
}
