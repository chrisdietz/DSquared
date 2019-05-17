using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class TipPercentage
    {
        [Key]
        public int Id { get; set; }

        public DateTime BusinessDate { get; set; }

        public String EmployeeName { get; set; }

        public string StoreNumber { get; set; }

        public string Job { get; set; }

        public decimal? Sales { get; set; }

        public decimal? TotalTips { get; set; }
    }
}
