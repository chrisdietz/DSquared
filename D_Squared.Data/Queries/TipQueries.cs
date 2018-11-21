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
    public class TipQueries
    {
        private readonly D_SquaredDbContext db;

        public TipQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public List<MakeUpPay> GetOutstandingMakeUps(string storeLocation, DateTime weekEnd)
        {
            storeLocation = storeLocation.Substring(0, 3);

            return db.MakeUpPay.Where(mup => mup.MakeUpPay1 > 0
                                                && mup.StoreNumber == storeLocation
                                                && mup.FiscalWeekEnding == weekEnd)
                               .ToList();
        }

        public List<MakeUpPay> GetOutstandingMakeUps(TipReportingSearchDTO searchDTO, List<string> accessibleLocations, DateTime fiscStart, DateTime fiscEnd)
        {
            string storeLocation = searchDTO.SelectedLocation.Substring(0, 3);
            DateTime realFiscEnd = fiscEnd.AddDays(1);

            if(searchDTO.MinimumMakeUpPay > 0)
            {
                var makeUps = db.MakeUpPay.Where(mup => (storeLocation == "Any" ? accessibleLocations.Any(al => al == mup.StoreNumber) : mup.StoreNumber == storeLocation)
                                                                && (mup.MakeUpPay1 > 0)
                                                                && (mup.FiscalWeekEnding >= fiscStart && mup.FiscalWeekEnding < realFiscEnd))
                                                .GroupBy(gb => gb.StoreNumber);

                List<MakeUpPay> finalResults = new List<MakeUpPay>();

                foreach(var makeUp in makeUps)
                {
                    if (makeUp.Sum(mu => mu.MakeUpPay1) > searchDTO.MinimumMakeUpPay)
                        finalResults.AddRange(makeUp);
                }

                return finalResults;
            }
            else
            {
                return db.MakeUpPay.Where(mup => (storeLocation == "Any" ? accessibleLocations.Any(al => al == mup.StoreNumber) : mup.StoreNumber == storeLocation)
                                                                && (mup.MakeUpPay1 > 0)
                                                                && (mup.FiscalWeekEnding >= fiscStart && mup.FiscalWeekEnding < realFiscEnd))
                                                .ToList();
            }
            
        }
    }
}
