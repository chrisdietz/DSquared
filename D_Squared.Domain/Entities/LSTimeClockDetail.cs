using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class LSTimeClockDetail
    {
        [Key]
        public long ID { get; set; }
        public string Store { get; set; }
        public DateTime BusinessDate { get; set; }
        public int PersonnelNUmber { get; set; }
        public string EmployeeName { get; set; }
        public string JobID { get; set; }
        public decimal Rate { get; set; }
        public DateTime Intime { get; set; }
        public DateTime Outtime { get; set; }
        public double TotalDuration { get; set; }
        public double TotalAmount { get; set; }
        public decimal TotalTips { get; set; }

    }
}
