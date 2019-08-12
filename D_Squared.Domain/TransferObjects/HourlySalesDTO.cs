using D_Squared.Domain.TransferObjects.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class HourlySalesDTO
    {
        public long ID { get; set; }
        [Display(Name = "Date")]
        public DateTime BusinessDate { get; set; }
        public string Store { get; set; }
        public int Hour { get; set; }
        [Display(Name = "Hour")]
        [Exportable("Hour", DataFormatType.String, false)]
        public string DisplayHour { get; set; }
        [Display(Name = "Beer Bottle")]
        [Exportable("Beer Bottle", DataFormatType.Currency, true)]
        public decimal BeerBottleSales { get; set; }
        [Display(Name = "Beer Draft")]
        [Exportable("Beer Draft", DataFormatType.Currency, true)]
        public decimal BeerDraftSales { get; set; }
        [Display(Name = "Food")]
        [Exportable("Food", DataFormatType.Currency, true)]
        public decimal FoodSales { get; set; }
        [Display(Name = "Liquor")]
        [Exportable("Liquor", DataFormatType.Currency, true)]
        public decimal LiquorSales { get; set; }
        [Display(Name = "Retail")]
        [Exportable("Retail", DataFormatType.Currency, true)]
        public decimal RetailSales { get; set; }
        [Display(Name = "Wine")]
        [Exportable("Wine", DataFormatType.Currency, true)]
        public decimal WineSales { get; set; }
        [Display(Name = "Beverages")]
        [Exportable("Beverages", DataFormatType.Currency, true)]
        public decimal NonAlcBevSales { get; set; }
        [Display(Name = "Retail Beer Sales")]
        [Exportable("Retail Beer Sales", DataFormatType.Currency, true)]
        public decimal RetailBeerSales { get; set; }
        [Display(Name = "Total")]
        [Exportable("Total", DataFormatType.Currency, true)]
        public decimal TotalSales { get; set; }
    }

    public class HourlySalesSearchDTO
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

        public HourlySalesSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedReportType = ReportByDay;
            SelectedDateRangeBegin = DateTime.Today;
            SelectedDateRangeEnd = DateTime.Today;
        }

        public HourlySalesSearchDTO(DateTime selectedDate)
        {
            SelectedDate = selectedDate;
        }
    }
}
