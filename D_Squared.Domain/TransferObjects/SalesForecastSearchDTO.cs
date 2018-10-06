using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class SalesForecastSearchDTO
    {
        public SalesForecastSearchDTO()
        {
            LocationId = string.Empty;
            EndDate = DateTime.Today.ToLocalTime();
            StartDate = DateTime.Today.ToLocalTime();
        }

        [Display(Name = "Location")]
        public string LocationId { get; set; }

        //[Display(Name = "Day of Week")]
        //public string DayOfWeek { get; set; }

        [Display(Name = "Forecast Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ForecastAmountMin { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ForecastAmountMax { get; set; }

        [Display(Name = "FY 2017 Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ActualPriorYearMin { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ActualPriorYearMax { get; set; }

        [Display(Name = "FY 2016 Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ActualPrior2YearsMin { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ActualPrior2YearsMax { get; set; }

        [Display(Name = "6 Week Average Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal AvgPrior4WeeksMin { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal AvgPrior4WeeksMax { get; set; }

        [Display(Name = "Labor Forecast Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal LaborForecastMin { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal LaborForecastMax { get; set; }

        [Display(Name = "Business Date Range")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }
}
