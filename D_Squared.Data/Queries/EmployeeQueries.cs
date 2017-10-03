using D_Squared.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Queries
{
    public class EmployeeQueries
    {
        private readonly D_SquaredDbContext db;

        public EmployeeQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public string GetStoreNumber(string name)
        {
            return "123";
        }
    }
}
