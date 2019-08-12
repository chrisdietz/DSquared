using D_Squared.Domain.TransferObjects.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class TimeClockDetailDTO
    {
        public long ID { get; set; }
        public string Store { get; set; }
        public DateTime BusinessDate { get; set; }
        public int PersonnelNUmber { get; set; }
        [Display(Name = "STAFF - SSN")]
        [Exportable("STAFF - SSN", DataFormatType.String, false)]
        public string EmployeeName { get; set; }
        [Display(Name = "Job Name")]
        [Exportable("Job Name", DataFormatType.String, false)]
        public string JobID { get; set; }
        [Display(Name = "Clock In")]
        [Exportable("Clock In", DataFormatType.TimeStamp, false)]
        public DateTime Intime { get; set; }
        [Display(Name = "Clock Out")]
        [Exportable("Clock Out", DataFormatType.TimeStamp, false)]
        public DateTime Outtime { get; set; }
        [Display(Name = "Hours")]
        [Exportable("Hours", DataFormatType.Decimal, false)]
        public double TotalDuration { get; set; }
        [Display(Name = "Pay Rate")]
        [Exportable("Pay Rate", DataFormatType.Currency, false)]
        public decimal Rate { get; set; }
        [Display(Name = "Wage")]
        [Exportable("Wage", DataFormatType.Currency, false)]
        public double TotalAmount { get; set; }
        [Display(Name = "Tips")]
        [Exportable("Tips", DataFormatType.Currency, false)]
        public decimal TotalTips { get; set; }
    }

    public class TimeClockDetailSearchDTO
    {
        public const string ReportByDay = "ByDay";
        public const string ReportByWeek = "ByWeek";
        public const string ReportByDateRange = "ByDateRange";

        [Display(Name = "Business Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }

        public string SelectedReportType { get; set; }

        public string SelectedLocation { get; set; }

        [Display(Name = "Business Date Range")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDateRangeBegin { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDateRangeEnd { get; set; }

        public TimeClockDetailSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedReportType = ReportByDay;
            SelectedDateRangeBegin = DateTime.Today;
            SelectedDateRangeEnd = DateTime.Today;
        }

        public TimeClockDetailSearchDTO(DateTime selectedDate)
        {
            SelectedDate = selectedDate;
        }
    }
}
