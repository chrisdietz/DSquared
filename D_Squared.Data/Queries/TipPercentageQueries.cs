using D_Squared.Data.Context;
using D_Squared.Data.Millers.Context;
using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Queries
{
    public class TipPercentageQueries
    {
        private readonly D_SquaredDbContext db;
        private readonly ForecastDataDbContext f_db;

        public TipPercentageQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public List<TipPercentage> GetTipPercentageList(List<DateTime> dates)
        {
            return db.TipPercentages.Where(tp => dates.Contains(tp.BusinessDate)).ToList();
        }

        public List<TipPercentage> GetTipPercentagesByWeek(string storeNumber, DateTime startDate, DateTime endDate)
        {
            DateTime realEndDate = endDate.AddDays(1);

            return db.TipPercentages.Where(tp => tp.StoreNumber == storeNumber
                                              && (tp.BusinessDate >= startDate && tp.BusinessDate < realEndDate))
                                 .OrderBy(tp => tp.EmployeeName)
                                 .ThenBy(tp => tp.BusinessDate)
                                 .ToList();
        }

        public List<TipPercentage> GetTipPercentagesByEmployeeByWeek(string employeeNumber, string storeNumber, DateTime startDate, DateTime endDate)
        {
            DateTime realEndDate = endDate.AddDays(1);

            return db.TipPercentages.Where(tp => tp.EmployeeNumber == employeeNumber
                                              && tp.StoreNumber == tp.StoreNumber
                                              && (tp.BusinessDate >= startDate && tp.BusinessDate < realEndDate))
                                 .OrderBy(tp => tp.EmployeeName)
                                 .ThenBy(tp => tp.BusinessDate)
                                 .ToList();
        }

        public decimal GetMAHAverageTipPercentForGivenDates(DateTime startDate, DateTime endDate)
        {
            DateTime realEndDate = endDate.AddDays(1);
            List<TipPercentage> tpList = db.TipPercentages.Where(tp => tp.BusinessDate >= startDate && tp.BusinessDate < realEndDate).ToList();
            var totalMAHSales = tpList.Sum(tp => tp.Sales.Value);
            var totalMAHTips = tpList.Sum(tp => tp.Tips.Value);

            return ((totalMAHSales != 0) ? (totalMAHTips * 100) / totalMAHSales : (totalMAHTips * 100));
        }

        public decimal GetStoreAverageTipPercentForGivenDates(DateTime startDate, DateTime endDate, string storeNumber)
        {
            DateTime realEndDate = endDate.AddDays(1);
            List<TipPercentage> tpList = db.TipPercentages.Where(tp => tp.BusinessDate >= startDate && tp.BusinessDate < realEndDate && tp.StoreNumber == storeNumber).ToList();
            var totalStoreSales = tpList.Sum(tp => tp.Sales.Value);
            var totalStoreTips = tpList.Sum(tp => tp.Tips.Value);

            return ((totalStoreSales != 0) ? (totalStoreTips * 100) / totalStoreSales : (totalStoreTips * 100));
        }

        public List<EmployeeJob> GetTippedEmployees(string storeNumber)
        {
            return db.EmployeeJobs.Where(e => e.StoreNumber == storeNumber && (e.Job == "Bartender" || e.Job == "Server"))
                                .OrderBy(e => e.EmployeeName)
                                .ToList();
        }

        public List<string> GetTippedEmployeeByEmployeeNumber(string employeeNumber)
        {
            return db.EmployeeJobs.Where(e => e.EmployeeNumber == employeeNumber).Select(e => e.Job).ToList();
        }
    }
}
