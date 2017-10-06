using D_Squared.Data.Employees.Context;
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

        public string GetStoreNumber(string name)
        {
            return db.Employees.Where(e => e.sAMAccountName == name).FirstOrDefault().Location;
        }
    }
}
