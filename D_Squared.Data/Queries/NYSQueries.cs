using D_Squared.Data.Context;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace D_Squared.Data.Queries
{
    public class NYSQueries
    {
        private readonly D_SquaredDbContext db;

        public NYSQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public bool CheckById(int id)
        {
            return db.NYS.Any(ny => ny.Id == id);
        }

        public NYS FindById(int id)
        {
            return db.NYS.Where(ny => ny.Id == id).FirstOrDefault();
        }

        public List<NYS> GetNYSByWeek(string storeLocation, DateTime startDate, DateTime endDate)
        {
            storeLocation = storeLocation.Substring(0, 3);
            DateTime realEndDate = endDate.AddDays(1);

            return db.NYS.Where(ny => ny.StoreNumber == storeLocation
                                              && (ny.BusinessDate >= startDate && ny.BusinessDate < realEndDate))
                                 .OrderBy(ny => ny.BusinessDate)
                                 .ThenBy(ny => ny.EmployeeName)
                                 .ToList();
        }

        public List<NYS> GetNYS(NYSSearchDTO searchDTO, List<string> accessibleLocations, DateTime fiscStart, DateTime fiscEnd)
        {
            string storeLocation = searchDTO.SelectedLocation.Substring(0, 3);
            DateTime realFiscEnd = fiscEnd.AddDays(1);

            return db.NYS.Where(ny => (storeLocation == "Any" ? accessibleLocations.Any(al => al == ny.StoreNumber) : ny.StoreNumber == storeLocation)
                                                && (ny.BusinessDate >= fiscStart && ny.BusinessDate < realFiscEnd))
                                 .OrderBy(ny => ny.StoreNumber)
                                 .ThenBy(ny => ny.BusinessDate)
                                 .ThenBy(ny => ny.EmployeeName)
                                 .ToList();
        }

        public List<MinimumWage> GetMinimumWages()
        {
            return db.MimumumWages.ToList();
        }
    }
}
