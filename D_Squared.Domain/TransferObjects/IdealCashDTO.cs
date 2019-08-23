using D_Squared.Domain.TransferObjects.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class IdealCashDTO
    {
        public long ID { get; set; }
        [Display(Name = "Business Date")]
        [Exportable("Business Date", DataFormatType.Date, false, DisplayFor.Condition_2)]
        public DateTime BusinessDate { get; set; }
        [Display(Name = "Total Sales")]
        [Exportable("Total Sales", DataFormatType.Currency, true)]
        public double TotalSales { get; set; }
        [Display(Name = "Location")]
        public string Store { get; set; }
        [Display(Name = "Cash")]
        [Exportable("Cash", DataFormatType.Currency, true)]
        public double Cash { get; set; }
        [Display(Name = "CC Tips")]
        [Exportable("CC Tips", DataFormatType.Currency, true)]
        public double CCTips { get; set; }
        [Display(Name = "Paid In")]
        [Exportable("Paid In", DataFormatType.Currency, true)]
        public double PaidIn { get; set; }
        [Display(Name = "Paid Out")]
        [Exportable("Paid Out", DataFormatType.Currency, true)]
        public double PaidOut { get; set; }
        [Display(Name = "Ideal Cash")]
        [Exportable("Ideal Cash", DataFormatType.Currency, true)]
        public double IdealCash { get; set; }
    }

    public class IdealCashSearchDTO
    {
        public const string ReportByDateRange = "ByDateRange";

        [Display(Name = "Business Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }
        public string SelectedLocation { get; set; }
        public string SelectedDateFilter { get; set; }

        [Display(Name = "Business Date Range")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDateRangeBegin { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDateRangeEnd { get; set; }

        public IdealCashSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedDateFilter = ReportByDateRange;
            SelectedDateRangeBegin = DateTime.MinValue;
            SelectedDateRangeEnd = DateTime.MinValue;
        }
    }
}
