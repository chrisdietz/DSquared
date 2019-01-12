using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class MinimumWage
    {
        public int Id { get; set; }

        public string StoreNumber { get; set; }

        public string State { get; set; }

        public decimal MinWage { get; set; }
    }
}
