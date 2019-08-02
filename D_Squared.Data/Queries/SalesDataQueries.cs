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

        public List<EmployeeJob> GetStoreEmployees(string storeNumber)
        {
            return db.EmployeeJobs.Where(e => e.StoreNumber == storeNumber).ToList();
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
                                                CloseTime = ls.CloseTime,
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
                                                CheckNumber = ls.CheckNumber,
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


        public List<IdealCashDTO> GetIdealCashDataByWeek(string storeNumber, DateTime startDate, DateTime endDate)
        {
            DateTime realEndDate = endDate.AddDays(1);
            List<IdealCashDTO> idealCashDTOs = db.LSIdealCashes.Where(i => i.Store == storeNumber && i.BusinessDate >= startDate && i.BusinessDate < realEndDate)
                                                    .Select(i => new IdealCashDTO
                                                    {
                                                        BusinessDate = i.BusinessDate,
                                                        Store = i.Store,
                                                        Cash = i.Cash,
                                                        CCTips = i.CCTips,
                                                        PaidIn = i.PaidIn,
                                                        PaidOut = i.PaidOut,
                                                        IdealCash = i.IdealCash
                                                    }).ToList();

            List<SalesDataDTO> salesDataDTOs = GetSalesDataByWeek(storeNumber, startDate, endDate);
            foreach (var idealCashDTO in idealCashDTOs)
            {
                SalesDataDTO sdDTO = salesDataDTOs.Find(s => s.DateOfEntry == idealCashDTO.BusinessDate);
                idealCashDTO.TotalSales = Convert.ToDouble(sdDTO.Sales);

            }

            return idealCashDTOs;
        }

        public List<PaidInOutDTO> GetPaidInOutByDayAndAccountTypeFilter(string storeNumber, DateTime businessDate, string accountTypeFilter)
        {
            var paidInOuts = (accountTypeFilter == PaidInOutSearchDTO.ReportByPaidInNOut)
                                ? db.PaidInOuts.Where(pio => pio.BusinessDate == businessDate.Date && pio.Store.Contains(storeNumber)).ToList()
                                : db.PaidInOuts.Where(pio => pio.BusinessDate == businessDate.Date && pio.Store.Contains(storeNumber)
                                                        && pio.AccountType == accountTypeFilter).ToList();

            return BuildPaidInOutDTOs(paidInOuts);
        }

        public List<PaidInOutDTO> GetPaidInOutByWeekAndAccountTypeFilter(string storeNumber, DateTime startDate, DateTime endDate, string accountTypeFilter = null)
        {
            DateTime realEndDate = endDate.AddDays(1);
            var paidInOuts = (accountTypeFilter == null || accountTypeFilter == PaidInOutSearchDTO.ReportByPaidInNOut) 
                                ? db.PaidInOuts.Where(ld => ld.BusinessDate >= startDate && ld.BusinessDate < realEndDate
                                                        && ld.Store.Contains(storeNumber)).ToList()
                                : db.PaidInOuts.Where(ld => ld.BusinessDate >= startDate && ld.BusinessDate < realEndDate
                                                        && ld.Store.Contains(storeNumber) && ld.AccountType == accountTypeFilter).ToList();

            return BuildPaidInOutDTOs(paidInOuts);
        }

        private List<PaidInOutDTO> BuildPaidInOutDTOs(List<PaidInOut> paidInOuts)
        {
            List<PaidInOutDTO> paidInOutDTOs = new List<PaidInOutDTO>();
            foreach (var paidInOut in paidInOuts)
            {
                PaidInOutDTO paidInOutDTO = new PaidInOutDTO
                {
                    Store = paidInOut.Store,
                    BusinessDate = paidInOut.BusinessDate,
                    PersonnelNumber = paidInOut.PersonnelNumber,
                    EmployeeName = paidInOut.EmployeeName,
                    Receipt = paidInOut.Receipt,
                    AccountType = paidInOut.AccountType,
                    AccountName = paidInOut.AccountName,
                    ExpenseAmount = paidInOut.ExpenseAmount
                };

                paidInOutDTOs.Add(paidInOutDTO);
            }

            return paidInOutDTOs;
        }

        public List<ServerSalesDTO> GetServerSalesDTOsByDate(string storeNumber, DateTime businessDate, int employeeID)
        {
            var lsServerSales =
                (employeeID == -1)
                        ? db.LSServerSales.Where(ss => ss.BusinessDate == businessDate.Date && ss.Store.Contains(storeNumber)).ToList()
                        : db.LSServerSales.Where(ss => ss.BusinessDate == businessDate.Date && ss.Store.Contains(storeNumber) && ss.EmployeeID == employeeID).ToList();

            return BuildServerSalesDTOs(lsServerSales);
        }

        public List<ServerSalesDTO> GetServerSalesDTOsByWeek(string storeNumber, DateTime startDate, DateTime endDate, int employeeID)
        {
            DateTime realEndDate = endDate.AddDays(1);
            var lsServerSales =
                (employeeID == -1)
                        ? db.LSServerSales.Where(ss => ss.BusinessDate >= startDate && ss.BusinessDate < realEndDate && ss.Store.Contains(storeNumber)).ToList()
                        : db.LSServerSales.Where(ss => ss.BusinessDate >= startDate && ss.BusinessDate < realEndDate && ss.Store.Contains(storeNumber) 
                            && ss.EmployeeID == employeeID).ToList();

            return BuildServerSalesDTOs(lsServerSales);
        }

        public List<ServerSalesDTO> GetServerSalesDTOsBy_BiWeekly(string storeNumber, DateTime startDate, DateTime endDate, int employeeID)
        {
            DateTime realEndDate = endDate.AddDays(1);
            var lsServerSales =
                (employeeID == -1)
                        ? db.LSServerSales.Where(ss => ss.BusinessDate >= startDate && ss.BusinessDate < realEndDate && ss.Store.Contains(storeNumber)).ToList()
                        : db.LSServerSales.Where(ss => ss.BusinessDate >= startDate && ss.BusinessDate < realEndDate && ss.Store.Contains(storeNumber)
                            && ss.EmployeeID == employeeID).ToList();

            return BuildServerSalesDTOs(lsServerSales);
        }

        private List<ServerSalesDTO> BuildServerSalesDTOs(List<LSServerSales> lSServerSales)
        {
            List<ServerSalesDTO> serverSalesDTOs = new List<ServerSalesDTO>();
            foreach (var lsSSales in lSServerSales)
            {
                ServerSalesDTO serverSalesDTO = new ServerSalesDTO
                {
                    Store = lsSSales.Store,
                    BusinessDate = lsSSales.BusinessDate,
                    TotalSales = lsSSales.TotalSales,
                    EmployeeName = lsSSales.EmployeeName,
                    FoodSales = lsSSales.FoodSales,
                    FoodSalesPercent = lsSSales.FoodSalesPercent,
                    LBWSales = lsSSales.LBWSales,
                    LBWSalesPercent = lsSSales.LBWSalesPercent,
                    NonAlcBevSales = lsSSales.NonAlcBevSales,
                    NonAlcBevSalesPercent = lsSSales.NonAlcBevSalesPercent
                };

                serverSalesDTOs.Add(serverSalesDTO);
            }

            return serverSalesDTOs;
        }
    }
}
