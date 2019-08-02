using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class LaborDataDTO
    {
        public long ID { get; set; }
        [Display(Name = "Business Date")]
        public DateTime BusinessDate { get; set; }
        [Display(Name = "Store Number")]
        public string Store { get; set; }
        public long ClockID { get; set; }
        [Display(Name = "Job ID")]
        public string JobName { get; set; }
        [Display(Name = "Center")]
        public string Center { get; set; }
        [Display(Name = "Regular Hours")]
        public double RegularHours { get; set; }
        [Display(Name = "OT Hours")]
        public double OTHours { get; set; }
        [Display(Name = "Regular Pay Amount")]
        public double RegularPayAmount { get; set; }
        [Display(Name = "OT Pay Amount")]
        public double OTPayAmount { get; set; }
        [Display(Name = "Total Hours")]
        public double TotalHours { get; set; }
        [Display(Name = "Total Pay Amount")]
        public double TotalPayAmount { get; set; }
    }

    public class LaborDataSearchDTO
    {
        public const string ReportByDay = "ByDay";
        public const string ReportByWeek = "ByWeek";
        public const string ReportByJob = "ByJob";
        public const string ReportByCenter = "ByCenter";

        [Display(Name = "Business Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }
        public string SelectedDayOrWeekFilter { get; set; }
        public string SelectedJobOrCenterFilter { get; set; }

        public string SelectedLocation { get; set; }

        public LaborDataSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedDayOrWeekFilter = ReportByDay;
            SelectedJobOrCenterFilter = ReportByJob;
        }

    }
}
