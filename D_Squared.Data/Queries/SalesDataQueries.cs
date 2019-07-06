using D_Squared.Data.Context;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Queries
{
    public class SalesDataQueries
    {
        private readonly D_SquaredDbContext db;
        public SalesDataQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }
        public bool CheckForExistingSalesDataByDate(DateTime date, string storeNumber)
        {
            return db.LSSales.Any(sd => sd.BusinessDate == date && sd.Store == storeNumber);
        }

        public SalesDataDTO GetCurrentDaySales(string storeNumber)
        {
            DateTime currentDate = DateTime.Now.Date;
            LSSales sData = db.LSSales.Where(sd => sd.BusinessDate == currentDate && sd.Store.StartsWith(storeNumber)).FirstOrDefault();
            SalesDataDTO sdDTO = new SalesDataDTO(sData);
            return sdDTO;
        }

        public SalesDataDTO GetSelectedBusinessDaySales(DateTime businessDate, string storeNumber)
        {
            List<LSSales> sDataList = db.LSSales.Where(sd => sd.BusinessDate == businessDate.Date && sd.Store.StartsWith(storeNumber)).ToList();
            SalesDataDTO sdDTO = new SalesDataDTO();
            sdDTO.DateOfEntry = businessDate;
            if (sDataList != null && sDataList.Count > 0)
            {
                sdDTO.Sales = sDataList.Sum(sd => sd.Sales);
                sdDTO.Discounts = sDataList.Sum(sd => sd.Discounts);
                sdDTO.Checks = sDataList.Select(sd => sd.CheckID).Distinct().ToList().Count.ToString();
            }
            return sdDTO;
        }

        public List<WeeklyTotalDurationDTO> GetWeeklyTotalDurationDTOs(string storeNumber, DateTime weekEnding, int hours)
        {
            List<WeeklyTotalDurationDTO> weeklyTotalDurationDTOs = db.WeeklyTotalDurations.Where(w => w.StoreNumber == storeNumber && w.WeekEnding == weekEnding && w.TotalDuration > hours)
                                                                    .Select(w => new WeeklyTotalDurationDTO
                                                                    {
                                                                        WeekEnding = w.WeekEnding,
                                                                        StoreNumber = w.StoreNumber,
                                                                        StaffName = w.StaffName,
                                                                        TotalDuration = w.TotalDuration,
                                                                        Overtime = w.TotalDuration - hours
                                                                    }).ToList();

            return weeklyTotalDurationDTOs;
        }

        public List<WeeklyTotalDurationDTO>  GetWeeklyTotalDurationDTOsByJob(string job, string storeNumber, DateTime weekEnding, int hours)
        {
            List<WeeklyTotalDurationDTO> weeklyTotalDurationDTOs = db.WeeklyTotalDurations.Where(w => w.StoreNumber == storeNumber && w.WeekEnding == weekEnding && w.TotalDuration > hours /*&& w.Job == job*/)
                                                                    .Select(w => new WeeklyTotalDurationDTO
                                                                    {
                                                                        WeekEnding = w.WeekEnding,
                                                                        StoreNumber = w.StoreNumber,
                                                                        StaffName = w.StaffName,
                                                                        TotalDuration = w.TotalDuration,
                                                                        Overtime = w.TotalDuration - hours
                                                                    }).ToList();

            return weeklyTotalDurationDTOs;
        }

        public List<EmployeeJobDTO> GetDistinctJobNames(string storeNumber)
        {
            List<EmployeeJobDTO> jobs = db.EmployeeJobs.Where(j => j.StoreNumber == storeNumber).Distinct()
                                        .Select(j => new EmployeeJobDTO
                                        {
                                            Job = j.Job
                                        }).ToList();
            return jobs;
        }

        public List<SalesDataDTO> GetSalesDataByDay(string storeNumber, DateTime businessDate)
        {
            List<SalesDataDTO> sDataList = db.LSSales.Where(sd => sd.BusinessDate == businessDate.Date && sd.Store.StartsWith(storeNumber))
                                            .Select(ls => new SalesDataDTO
                                            {
                                                DateOfEntry = ls.BusinessDate,
                                                Sales = ls.Sales,
                                                Discounts = ls.Discounts,
                                                FoodSales = ls.FoodSales,
                                                LiquorSales = ls.LiquorSales,
                                                BeerDraftSales = ls.BeerDraftSales,
                                                BeerBottleSales = ls.BeerBottleSales,
                                                NonAlcBevSales = ls.NonAlcBevSales,
                                                WineSales = ls.WineSales,
                                                RetailBeerSales = ls.RetailBeerSales,
                                                RetailSales = ls.RetailSales,
                                                TaxAmount = ls.TaxAmount,
                                                PaymentAmount = ls.PaymentAmount,
                                                AdjustmentSales = ls.Sales - ls.Discounts
                                            }).ToList();
            return sDataList;

        }

        public List<SalesDataDTO> GetSalesDataByWeek(string storeNumber, DateTime startDate, DateTime endDate)
        {
            DateTime realEndDate = endDate.AddDays(1);
            var lsSalesGroup = from sd in db.LSSales
                                      where sd.BusinessDate >= startDate && sd.BusinessDate < realEndDate && sd.Store == storeNumber
                                      group sd by sd.BusinessDate into sDataGroup
                                      orderby sDataGroup.Key descending
                                      select sDataGroup;
            List<SalesDataDTO> salesDataDTOs = new List<SalesDataDTO>();
            foreach (var sDataGroup in lsSalesGroup)
            {
                SalesDataDTO sdDTO = new SalesDataDTO()
                {
                    DateOfEntry = sDataGroup.Key,
                    Sales = sDataGroup.Sum(sd => sd.Sales),
                    Discounts = sDataGroup.Sum(sd => sd.Discounts),
                    FoodSales = sDataGroup.Sum(sd => sd.FoodSales),
                    LiquorSales = sDataGroup.Sum(sd => sd.LiquorSales),
                    BeerDraftSales = sDataGroup.Sum(sd => sd.BeerDraftSales),
                    BeerBottleSales = sDataGroup.Sum(sd => sd.BeerBottleSales),
                    NonAlcBevSales = sDataGroup.Sum(sd => sd.NonAlcBevSales),
                    WineSales = sDataGroup.Sum(sd => sd.WineSales),
                    RetailBeerSales = sDataGroup.Sum(sd => sd.RetailBeerSales),
                    RetailSales = sDataGroup.Sum(sd => sd.RetailSales),
                    TaxAmount = sDataGroup.Sum(sd => sd.TaxAmount),
                    PaymentAmount = sDataGroup.Sum(sd => sd.PaymentAmount),
                    AdjustmentSales = sDataGroup.Sum(sd => sd.Sales) - sDataGroup.Sum(sd => sd.Discounts)
                };
                salesDataDTOs.Add(sdDTO);
            }

            return salesDataDTOs;
        }
    }
}
