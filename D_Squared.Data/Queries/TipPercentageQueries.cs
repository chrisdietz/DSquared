using D_Squared.Data.Context;
using D_Squared.Data.Millers.Context;
using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Queries
{
    public class TipPercentageQueries
    {
        private readonly D_SquaredDbContext db;
        private readonly ForecastDataDbContext f_db;

        public TipPercentageQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public List<TipPercentage> GetTipPercentageList(List<DateTime> dates)
        {
            return db.TipPercentage.Where(tp => dates.Contains(tp.BusinessDate)).ToList();
        }
    }
}
