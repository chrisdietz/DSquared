using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class LSLabor
    {
        [Key]
        public long ID { get; set; }
        public DateTime BusinessDate { get; set; }
        public string Store { get; set; }
        public long ClockID { get; set; }
        public string JobName { get; set; }
        public string Center { get; set; }
        public double RegularHours { get; set; }
        public double OTHours { get; set; }
        public double RegularPayAmount { get; set; }
        public double OTPayAmount { get; set; }
        public double TotalHours { get; set; }
        public double TotalPayAmount { get; set; }
    }
}
