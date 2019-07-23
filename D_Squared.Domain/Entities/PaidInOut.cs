using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class PaidInOut
    {
        public long ID { get; set; }
        public string Store { get; set; }
        public DateTime BusinessDate { get; set; }
        public string Receipt { get; set; }
        public string CheckID { get; set; }
        public string AccountType { get; set; }
        public string AccountName { get; set; }
        public decimal ExpenseAmount { get; set; }
        public int PersonnelNumber { get; set; }
        public string EmployeeName { get; set; }
    }
}
