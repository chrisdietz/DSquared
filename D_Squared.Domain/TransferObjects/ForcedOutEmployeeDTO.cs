using D_Squared.Domain.TransferObjects.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace D_Squared.Domain.TransferObjects
{
    public class ForcedOutEmployeeDTO
    {
        public long ID { get; set; }
        [Display(Name = "Business Date")]
        [Exportable("Business Date", DataFormatType.Date, false, DisplayFor.Condition_2)]
        public DateTime BusinessDate { get; set; }
        [Display(Name = "Store")]
        [Exportable("Store", DataFormatType.String, false)]
        public string Store { get; set; }
        [Display(Name = "Staff Name")]
        [Exportable("Staff Name", DataFormatType.String, false)]
        public string StaffName { get; set; }
        [Display(Name = "Job")]
        [Exportable("Job", DataFormatType.String, false)]
        public string Job { get; set; }
        [Display(Name = "Salary Rate")]
        [Exportable("Salary Rate", DataFormatType.Currency, false)]
        public double SalaryRate { get; set; }
        [Display(Name = "In Time")]
        [Exportable("In Time", DataFormatType.TimeStamp, false)]
        public DateTime InTime { get; set; }
        [Display(Name = "Out Time")]
        [Exportable("Out Time", DataFormatType.TimeStamp, false)]
        public DateTime OutTime { get; set; }
        [Display(Name = "Total Duration")]
        [Exportable("Total Duration", DataFormatType.Decimal, false)]
        public double TotalDuration { get; set; }
    }

    public class ForcedOutEmployeeSearchDTO
    {
        public const string ReportByDay = "ByDay";
        public const string ReportByDateRange = "ByDateRange";

        [Display(Name = "Business Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }
        public string SelectedDateFilter { get; set; }

        public string SelectedLocation { get; set; }

        [Display(Name = "Business Date Range")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDateRangeBegin { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDateRangeEnd { get; set; }

        public ForcedOutEmployeeSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedDateFilter = ReportByDay;
            SelectedDateRangeBegin = DateTime.MinValue;
            SelectedDateRangeEnd = DateTime.MinValue;
        }

    }
}
