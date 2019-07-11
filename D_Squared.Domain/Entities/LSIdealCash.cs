using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class LSIdealCash
    {
        [Key]
        public long ID { get; set; }
        public DateTime BusinessDate { get; set; }
        public string Store { get; set; }
        public double Cash { get; set; }
        public double CCTips { get; set; }
        public double PaidIn { get; set; }
        public double PaidOut { get; set; }
        public double IdealCash { get; set; }
    }
}
