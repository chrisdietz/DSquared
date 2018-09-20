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

        public SalesForecastDTO GetLiveSalesForecastDTO(DateTime date, string storeNumber)
        {
            SalesForecastDTO dto = new SalesForecastDTO();

            if (CheckForExistingSalesForecastByDate(date, storeNumber))
            {
                dto = LiveUpdateSalesForecastDTO(new SalesForecastDTO(GetSalesForecastRecordsByDate(date, storeNumber)), storeNumber);
            } 
            else
                dto = new SalesForecastDTO(date, fdq.GetSalesPriorYear(storeNumber, date), fdq.GetSalesPriorTwoYears(storeNumber, date), fdq.GetAverageSalesPerMonth(storeNumber, date), fdq.GetLaborForecast(storeNumber, date));

            return dto;
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
                    theList.Add(new SalesForecastDTO(day, fdq.GetSalesPriorYear(storeNumber, day), fdq.GetSalesPriorTwoYears(storeNumber, day), fdq.GetAverageSalesPerMonth(storeNumber, day), fdq.GetLaborForecast(storeNumber, day)));
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
                    entry.ForecastAM = forecast.ForecastAM;
                    entry.ForecastPM = forecast.ForecastPM;
                    entry.ForecastAmount = forecast.ForecastAM + forecast.ForecastPM;
                    entry.UpdatedBy = userName;
                    entry.UpdatedDate = DateTime.Now;
                }
                else
                {
                    SalesForecast entry = new SalesForecast()
                    {
                        BusinessDate = forecast.DateOfEntry,
                        StoreNumber = storeNumber,
                        ForecastAM = forecast.ForecastAM,
                        ForecastPM = forecast.ForecastPM,
                        ForecastAmount = forecast.ForecastAM + forecast.ForecastPM,
                        ActualPriorYear = forecast.PriorYearSales,
                        ActualPrior2Years = forecast.Prior2YearSales,
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

        public List<SalesForecastDTO> LiveUpdateSalesForecastDTOs(List<SalesForecastDTO> list, string storeNumber)
        {
            foreach (var record in list)
            {
                record.AverageSalesPerMonth = fdq.GetAverageSalesPerMonth(storeNumber, record.DateOfEntry);
                record.PriorYearSales = fdq.GetSalesPriorYear(storeNumber, record.DateOfEntry);
                record.Prior2YearSales = fdq.GetSalesPriorTwoYears(storeNumber, record.DateOfEntry);
                record.LaborForecast = fdq.GetLaborForecast(storeNumber, record.DateOfEntry);
            }

            return list;
        }

        public SalesForecastDTO LiveUpdateSalesForecastDTO(SalesForecastDTO dto, string storeNumber)
        {
            dto.AverageSalesPerMonth = fdq.GetAverageSalesPerMonth(storeNumber, dto.DateOfEntry);
            dto.PriorYearSales = fdq.GetSalesPriorYear(storeNumber, dto.DateOfEntry);
            dto.Prior2YearSales = fdq.GetSalesPriorTwoYears(storeNumber, dto.DateOfEntry);
            dto.LaborForecast = fdq.GetLaborForecast(storeNumber, dto.DateOfEntry);

            return dto;
        }

        public void RefreshSalesForecastData(List<SalesForecastDTO> forecasts, string storeNumber, string userName)
        {
            forecasts = LiveUpdateSalesForecastDTOs(forecasts, storeNumber);

            foreach (var forecast in forecasts)
            {
                if (CheckForExistingSalesForecastByDate(forecast.DateOfEntry, storeNumber))
                {
                    SalesForecast entry = GetSalesForecastRecordsByDate(forecast.DateOfEntry, storeNumber);
                    entry.ForecastAmount = forecast.ForecastAmount;
                    entry.ActualPriorYear = forecast.PriorYearSales;
                    entry.ActualPrior2Years = forecast.Prior2YearSales;
                    entry.AvgPrior4Weeks = forecast.AverageSalesPerMonth;
                    entry.LaborForecast = forecast.LaborForecast;
                    entry.UpdatedBy = userName;
                    entry.UpdatedDate = DateTime.Now;
                }
                else if (storeNumber != "OSRC")
                {
                    SalesForecast entry = new SalesForecast()
                    {
                        BusinessDate = forecast.DateOfEntry,
                        StoreNumber = storeNumber,
                        ForecastAmount = forecast.ForecastAmount,
                        ActualPriorYear = forecast.PriorYearSales,
                        ActualPrior2Years = forecast.Prior2YearSales,
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

        public List<SalesForecastSummaryDTO> GetSalesForecastSummaryList (DateTime selectedDate, List<string> locationList)
        {
            List<SalesForecastSummaryDTO> summaryList = new List<SalesForecastSummaryDTO>();

            foreach (string location in locationList)
            {
                summaryList.Add(new SalesForecastSummaryDTO(location, GetSpecificWeekAsSalesForecastDTOList(selectedDate, location)));
            }

            return summaryList;
        }

        public List<SalesForecastSummaryColumnDTO> GetWeeklyReportColumnTotals(DateTime selectedDay)
        {
            List<DateTime> dates = GetCurrentWeek(selectedDay);

            List<SalesForecast> theList = db.SalesForecasts.Where(sf => dates.Contains(sf.BusinessDate)).ToList();

            List<SalesForecastSummaryColumnDTO> columnSums = new List<SalesForecastSummaryColumnDTO>();

            foreach (var day in dates)
            {
                columnSums.Add(new SalesForecastSummaryColumnDTO(day, theList.Where(tl => tl.BusinessDate == day).ToList()));
            }

            return columnSums;
        }
    }
}
