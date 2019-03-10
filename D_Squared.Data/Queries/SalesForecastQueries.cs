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

        public readonly ForecastDataQueries fdq;

        public SalesForecastQueries(D_SquaredDbContext db, ForecastDataDbContext f_db)
        {
            this.db = db;
            this.f_db = f_db;

            fdq = new ForecastDataQueries(f_db);
        }

        public bool CheckById(int id)
        {
            return db.SalesForecasts.Any(sf => sf.Id == id);
        }

        public SalesForecast FindById(int id)
        {
            return db.SalesForecasts.Where(sf => sf.Id == id).FirstOrDefault();
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
                dto = new SalesForecastDTO(date, fdq.GetSalesPriorYear(storeNumber, date), fdq.GetSalesPriorTwoYears(storeNumber, date), fdq.GetSalesPriorThreeYears(storeNumber, date), fdq.GetAverageSalesPerMonth(storeNumber, date), fdq.GetLaborForecast(storeNumber, date));

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

        public void UpdateSalesForecast(SalesForecast forecast, string username)
        {
            SalesForecast entry = FindById(forecast.Id);

            entry.ForecastAM = forecast.ForecastAM;
            entry.ForecastPM = forecast.ForecastPM;
            entry.ForecastAmount = forecast.ForecastAM + forecast.ForecastPM;
            entry.UpdatedBy = username;
            entry.UpdatedDate = DateTime.Now;

            db.SaveChanges();
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

        public List<SalesForecast> GetSalesForecastEntries(SalesForecastSearchDTO searchDTO, List<string> accessibleLocations, DateTime fiscStart, DateTime fiscEnd)
        {
            decimal zero = new decimal(0);
            string cleanLocation = searchDTO.LocationId == "Any" ? "Any" : searchDTO.LocationId.Substring(0, 3);

            DateTime realFiscEnd = fiscEnd.AddDays(1);

            return db.SalesForecasts.Where(sf => (cleanLocation == "Any" ? accessibleLocations.Any(al => al == sf.StoreNumber): sf.StoreNumber == cleanLocation)
                                                    //&& ((searchDTO.ForecastAmountMin == zero && searchDTO.ForecastAmountMax == zero) || (sf.ForecastAmount >= searchDTO.ForecastAmountMin && sf.ForecastAmount <= searchDTO.ForecastAmountMax))
                                                    //&& ((searchDTO.ActualPriorYearMin == zero && searchDTO.ActualPriorYearMax == zero) || (sf.ActualPriorYear >= searchDTO.ActualPriorYearMin && sf.ActualPriorYear <= searchDTO.ActualPriorYearMax))
                                                    //&& ((searchDTO.ActualPrior2YearsMin == zero && searchDTO.ActualPrior2YearsMax == zero) || (sf.ActualPrior2Years >= searchDTO.ActualPrior2YearsMin && sf.ActualPrior2Years <= searchDTO.ActualPrior2YearsMax))
                                                    //&& ((searchDTO.AvgPrior4WeeksMin == zero && searchDTO.AvgPrior4WeeksMax == zero) || (sf.AvgPrior4Weeks >= searchDTO.AvgPrior4WeeksMin && sf.AvgPrior4Weeks <= searchDTO.AvgPrior4WeeksMax))
                                                    //&& ((searchDTO.LaborForecastMin == zero && searchDTO.LaborForecastMax == zero) || (sf.LaborForecast >= searchDTO.LaborForecastMin && sf.LaborForecast <= searchDTO.LaborForecastMax))
                                                    && (sf.BusinessDate >= fiscStart && sf.BusinessDate < realFiscEnd))
                                    .OrderBy(sf => sf.StoreNumber)
                                    .ThenBy(sf => sf.BusinessDate)
                                    .ToList();
        }

        public List<SalesForecast> GetSalesForecastByDates(List<DateTime> dates)
        {
            return db.SalesForecasts.Where(sf => dates.Contains(sf.BusinessDate)).ToList();
        }
    }
}
