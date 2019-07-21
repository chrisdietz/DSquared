using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class Labor8020DTO
    {
        public long ID { get; set; }
        public string ClockID { get; set; }
        [Display(Name = "Location Name")]
        public string Store { get; set; }
        [Display(Name = "Business Date")]
        public DateTime BusinessDate { get; set; }
        [Display(Name = "Personnel Number")]
        public int PersonnelNumber { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Job")]
        public string JobName { get; set; }
        public DateTime Time { get; set; }
        [Display(Name = "80/20")]
        public string P_8020 { get; set; }
        
        public int P_8020ManagerID { get; set; }
        [Display(Name = "Manager")]
        public string P_8020Manager { get; set; }
    }

    public class Labor8020SearchDTO
    {
        public const string ReportByDay = "ByDay";
        public const string ReportByWeek = "ByWeek";
        public const string ReportByDisagree = "Disagree";
        public const string ReportByOverride = "Manager Override";

        [Display(Name = "Business Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }
        public string SelectedDayOrWeekFilter { get; set; }
        public string Selected8020Filter { get; set; }

        public string SelectedLocation { get; set; }

        public Labor8020SearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedDayOrWeekFilter = ReportByDay;
            Selected8020Filter = ReportByOverride;
        }
    }
}
