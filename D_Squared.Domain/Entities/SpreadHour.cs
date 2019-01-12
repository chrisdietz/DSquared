namespace D_Squared.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class SpreadHour
    {
        [Key]
        public int Id { get; set; }

        public DateTime BusinessDate { get; set; }

        public int ExternalID { get; set; }

        public string EmployeeName { get; set; }

        public string StoreNumber { get; set; }

        public string Job { get; set; }

        public int SpreadHours { get; set; }

        public decimal TotalHours { get; set; }

        public DateTime FirstClockIn { get; set; }

        public DateTime LastClockOut { get; set; }
    }
}
