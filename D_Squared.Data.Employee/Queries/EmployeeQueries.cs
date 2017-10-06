using D_Squared.Data.Employees.Context;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System.Linq;

namespace D_Squared.Data.Employees.Queries
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
    }
}
