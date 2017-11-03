using D_Squared.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Queries
{
    public class CodeQueries
    {
        private readonly D_SquaredDbContext db;

        public CodeQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public List<string> GetDistinctListOfCodeCategories()
        {
            return db.Codes.OrderBy(c => c.CodeCategory).Select(c => c.CodeCategory).Distinct().ToList();
        }

        public List<string> GetDistrinctListByCodeCategory(string category)
        {
            return db.Codes.Where(c => c.CodeCategory == category).OrderBy(c => c.CodeValue).Select(c => c.CodeValue).Distinct().ToList();
        }
    }
}
