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
            return db.Employees.Where(e => e.Location == storeNumber).ToList();
        }
    }
}
