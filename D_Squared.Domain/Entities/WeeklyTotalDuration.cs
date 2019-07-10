using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class WeeklyTotalDuration
    {
        [Key]
        public long Id { get; set; }
        public string ExternalID { get; set; }
        public DateTime WeekEnding { get; set; }
        public string StoreNumber { get; set; }
        public string StaffName { get; set; }
        public double TotalDuration { get; set; }
    }
}
