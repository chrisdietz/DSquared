using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class WeeklyTotalDurationDTO
    {
        public DateTime WeekEnding { get; set; }
        [Display(Name = "Store Number")]
        public string StoreNumber { get; set; }
        [Display(Name = "Employee Name")]
        public string StaffName { get; set; }
        [Display(Name = "Hours Worked")]
        public double TotalDuration { get; set; }
        [Display(Name = "Hours Over 35")]
        public double Overtime { get { return (TotalDuration - 35); } }

        public WeeklyTotalDurationDTO()
        {

        }

        public WeeklyTotalDurationDTO(WeeklyTotalDuration weeklyTotalDuration)
        {
            if (weeklyTotalDuration != null)
            {
                WeekEnding = weeklyTotalDuration.WeekEnding;
                StoreNumber = weeklyTotalDuration.StoreNumber;
                StaffName = weeklyTotalDuration.StaffName;
                TotalDuration = weeklyTotalDuration.TotalDuration;
            }
        }

    }

    public class WeeklyTotalDurationSearchDTO
    {
        [Display(Name = "Business Week by Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }

        public string SelectedLocation { get; set; }
        public string SelectedJob { get; set; }

        public WeeklyTotalDurationSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedJob = string.Empty;
        }

        public WeeklyTotalDurationSearchDTO(DateTime selectedDate)
        {
            SelectedDate = selectedDate;
        }

    }
}
