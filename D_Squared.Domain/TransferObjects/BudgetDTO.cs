using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class BudgetDTO
    {
        public Budget Budget { get; set; }

        public List<DateTime> BudgetDateRange { get; set; }

        public int NumberOfWeeks { get; set; }
    }
}