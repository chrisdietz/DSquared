using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class RedbookSearchDTO
    {
        public RedbookSearchDTO()
        {
            LocationId = string.Empty;
            SelectedWeatherAM = string.Empty;
            SelectedWeatherPM = string.Empty;
            ManagerOnDutyAM = string.Empty;
            ManagerOnDutyPM = string.Empty;
            EndDate = DateTime.Today.ToLocalTime();
            StartDate = DateTime.Today.ToLocalTime();
        }

        public RedbookSearchDTO(string lId, string mAM, string mPM)
        {
            LocationId = lId;
            ManagerOnDutyAM = mAM;
            ManagerOnDutyPM = mPM;
        }

        [Display(Name = "Location")]
        public string LocationId { get; set; }

        [Display(Name = "AM Weather")]
        public string SelectedWeatherAM { get; set; }

        [Display(Name = "PM Weather")]
        public string SelectedWeatherPM { get; set; }

        [Display(Name = "AM Manager")]
        public string ManagerOnDutyAM { get; set; }

        [Display(Name = "PM Manager")]
        public string ManagerOnDutyPM { get; set; }

        [Display(Name = "Business Date Range")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }
}
