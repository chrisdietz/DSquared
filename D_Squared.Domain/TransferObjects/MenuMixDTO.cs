using D_Squared.Domain.TransferObjects.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class MenuMixDTO
    {
        public long ID { get; set; }
        [Display(Name = "Business Date")]
        [Exportable("Business Date", DataFormatType.Date, false, DisplayFor.Condition_2)]
        public DateTime BusinessDate { get; set; }
        public string Store { get; set; }
        [Display(Name = "Department")]
        [Exportable("Department", DataFormatType.String, false)]
        public string Department { get; set; }
        [Display(Name = "Category")]
        [Exportable("Category", DataFormatType.String, false)]
        public string Category { get; set; }
        [Display(Name = "Reporting Category")]
        [Exportable("Reporting Category", DataFormatType.String, false)]
        public string REPORTINGCATEGORY { get; set; }
        [Display(Name = "PLU")]
        [Exportable("PLU", DataFormatType.String, false)]
        public string PLU { get; set; }
        [Display(Name = "Item Name")]
        [Exportable("Item Name", DataFormatType.String, false)]
        public string ItemName { get; set; }
        [Display(Name = "Price")]
        [Exportable("Price", DataFormatType.Currency, false)]
        public decimal Price { get; set; }
        [Display(Name = "Basic Unit")]
        [Exportable("Basic Unit", DataFormatType.String, false)]
        public string BasicUnit { get; set; }
        [Display(Name = "Basic Quantity")]
        [Exportable("Basic Quantity", DataFormatType.Decimal, false)]
        public decimal BasicQty { get; set; }
        [Display(Name = "Selling Unit")]
        [Exportable("Selling Unit", DataFormatType.String, false)]
        public string SellingUnit { get; set; }
        [Display(Name = "Selling Quantity")]
        [Exportable("Selling Quantity", DataFormatType.Decimal, false)]
        public decimal SellingQty { get; set; }
        [Display(Name = "Amount")]
        [Exportable("Amount", DataFormatType.Currency, false)]
        public double Amount { get; set; }
    }

    public class MenuMixSearchDTO
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

        public MenuMixSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedDateFilter = ReportByDay;
            SelectedDateRangeBegin = DateTime.MinValue;
            SelectedDateRangeEnd = DateTime.MinValue;
        }
    }
}
