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
    public class SpreadHourQueries
    {
        private readonly D_SquaredDbContext db;

        public SpreadHourQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public bool CheckById(int id)
        {
            return db.SpreadHours.Any(sf => sf.Id == id);
        }

        public SpreadHour FindById(int id)
        {
            return db.SpreadHours.Where(sf => sf.Id == id).FirstOrDefault();
        }

        public List<SpreadHour> GetSpreadHoursByWeek(string storeLocation, DateTime startDate, DateTime endDate)
        {
            storeLocation = storeLocation.Substring(0, 3);
            DateTime realEndDate = endDate.AddDays(1);

            return db.SpreadHours.Where(sh => sh.StoreNumber == storeLocation
                                              && (sh.BusinessDate >= startDate && sh.BusinessDate < realEndDate))
                                 .OrderBy(sh => sh.BusinessDate)
                                 .ThenBy(sh => sh.EmployeeName)
                                 .ToList();
        }

        public List<SpreadHour> GetSpreadHours(SpreadHourSearchDTO searchDTO, List<string> accessibleLocations, DateTime fiscStart, DateTime fiscEnd)
        {
            string storeLocation = searchDTO.SelectedLocation.Substring(0, 3);
            DateTime realFiscEnd = fiscEnd.AddDays(1);

            return db.SpreadHours.Where(sh => (storeLocation == "Any" ? accessibleLocations.Any(al => al == sh.StoreNumber) : sh.StoreNumber == storeLocation)
                                                && (sh.BusinessDate >= fiscStart && sh.BusinessDate < realFiscEnd))
                                 .OrderBy(sh => sh.StoreNumber)
                                 .ThenBy(sh => sh.BusinessDate)
                                 .ThenBy(sh => sh.EmployeeName)
                                 .ToList();
        }

        public List<MinimumWage> GetMinimumWages()
        {
            return db.MimumumWages.ToList();
        }
    }
}
