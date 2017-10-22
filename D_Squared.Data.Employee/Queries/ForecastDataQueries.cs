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
        public decimal GetSalesPriorYear(string storeNumber)
        {
            return new decimal(123456);
        }

        public decimal GetAverageSalesPerMonth(string storeNumber)
        {
            return new decimal(1234);
        }

        public decimal GetLaborForecast(string storeNumber)
        {
            return new decimal(123);
        }
    }
}
