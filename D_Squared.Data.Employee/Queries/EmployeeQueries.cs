using D_Squared.Data.Millers.Context;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System.Collections.Generic;
using System.Linq;

namespace D_Squared.Data.Millers.Queries
{
    public class EmployeeQueries
    {
        private readonly EmployeeDbContext db;

        public EmployeeQueries(EmployeeDbContext db)
        {
            this.db = db;
        }

        public string GetStoreNumber(string windowsUsername)
        {
            return db.Employees.Where(e => e.sAMAccountName == windowsUsername).FirstOrDefault().Location;
        }

        public bool EmployeeExists(string windowsUsername)
        {
            return db.Employees.Any(e => e.sAMAccountName == windowsUsername);
        }

        public EmployeeDTO GetEmployeeInfo(string windowsUsername)
        {
            Employee employeeRecord = db.Employees.Where(e => e.sAMAccountName == windowsUsername).FirstOrDefault();

            EmployeeDTO employeeInfo = new EmployeeDTO
            {
                FirstName = employeeRecord.First,
                LastName = employeeRecord.Last,
                Username = employeeRecord.sAMAccountName,
                StoreNumber = employeeRecord.Location
            };

            return employeeInfo;
        }

        public List<string> GetLocationList()
        {
            return db.Employees.Select(e => e.Location).Distinct().ToList();
        }

        public List<Employee> GetManagersForLocation(string storeNumber)
        {
            storeNumber = storeNumber.Substring(0, 3);

            return db.Employees.Where(e => e.Location == storeNumber && e.EmployeeId != "9999").ToList();
        }

        public List<Employee> GetManagersForLocation(List<string> storeNumbers)
        {
            for (int i = 0; i < storeNumbers.Count(); i++)
                storeNumbers[i] = storeNumbers[i].Substring(0, 3);

            return db.Employees.Where(e => storeNumbers.Contains(e.Location) && e.EmployeeId != "9999").ToList();
        }

        public List<Employee> GetAllManagers()
        {
            return db.Employees.Where(e => e.EmployeeId != "9999").ToList();
        }

        public List<string> GetAllValidStoreLocations()
        {
            return db.StoreLocations.Select(sl => sl.LocationName.Substring(0, 3)).ToList();
        }

        public List<string> GetStoreLocationListForAdmin(bool includeClosed)
        {
            if (includeClosed)
            {
                return db.StoreLocations.Select(sl => sl.LocationName).ToList();
            }
            else
            {
                return db.StoreLocations.Where(sl => sl.isClose == 0).Select(sl => sl.LocationName).ToList();
            }
        }

        public List<string> GetStoreLocationListByRegion(EmployeeDTO employee, bool includeClosed)
        {
            if (includeClosed)
            {
                return db.StoreLocations.Where(sl => sl.Region == employee.LastName).Select(sl => sl.LocationName).ToList();
            }
            else
            {
                return db.StoreLocations.Where(sl => sl.Region == employee.LastName && sl.isClose == 0).Select(sl => sl.LocationName).ToList();
            }
        }

        public List<string> GetStoreLocationListByDivision(EmployeeDTO employee, bool includeClosed)
        {
            if (includeClosed)
            {
                return db.StoreLocations.Where(sl => sl.Division == employee.LastName).Select(sl => sl.LocationName).ToList();
            }
            else
            {
                return db.StoreLocations.Where(sl => sl.Division == employee.LastName && sl.isClose == 0).Select(sl => sl.LocationName).ToList();
            }
        }

    }
}
