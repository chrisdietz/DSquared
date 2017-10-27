using D_Squared.Data.Millers.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Millers.Queries
{
    public class ForecastDataQueries
    {
        private readonly ForecastDataDbContext db;

        public ForecastDataQueries(ForecastDataDbContext db)
        {
            this.db = db;
        }

        //dummy queries
        public decimal GetSalesPriorYear(string storeNumber, DateTime day)
        {
            return db.ForecastData.Where(fd => fd.StoreNumber == storeNumber && fd.BusinessDate == day).FirstOrDefault().ActualPriorYear;
        }

        public decimal GetAverageSalesPerMonth(string storeNumber, DateTime day)
        {
            return db.ForecastData.Where(fd => fd.StoreNumber == storeNumber && fd.BusinessDate == day).FirstOrDefault().AvgPrior4Weeks;
        }

        public decimal GetLaborForecast(string storeNumber, DateTime day)
        {
            return db.ForecastData.Where(fd => fd.StoreNumber == storeNumber && fd.BusinessDate == day).FirstOrDefault().LaborForecast;
        }
    }
}
