using System;
using System.ComponentModel.DataAnnotations;

namespace D_Squared.Domain.Entities
{
    public class NYS
    {
        [Key]
        public int Id { get; set; }

        public DateTime BusinessDate { get; set; }

        public int ExternalID { get; set; }

        public string EmployeeName { get; set; }

        public string StoreNumber { get; set; }

        public string Job { get; set; }

        public decimal NYSHours { get; set; }

        public decimal TotalHours { get; set; }

        public DateTime FirstClockIn { get; set; }

        public DateTime LastClockOut { get; set; }
    }
}
