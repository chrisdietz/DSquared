using D_Squared.Data.Context;
using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Queries
{
    public class TipQueries
    {
        private readonly D_SquaredDbContext db;

        public TipQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public List<MakeUpPay> GetOutstandingMakeUps(string storeLocation, string username, DateTime weekEnd)
        {
            storeLocation = storeLocation.Substring(0, 3);

            return db.MakeUpPay.Where(mup => mup.MakeUpPay1 > 0 
                                                && mup.StoreNumber == storeLocation
                                                && mup.FiscalWeekEnding == weekEnd)
                                .ToList();
        }
    }
}
