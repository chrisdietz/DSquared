using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class ForcedOutEmployee
    {
        public long ID { get; set; }
        public DateTime BusinessDate { get; set; }
        public string Store { get; set; }
        public string StaffName { get; set; }
        public string Job { get; set; }
        public double SalaryRate { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public double TotalDuration { get; set; }
    }
}
