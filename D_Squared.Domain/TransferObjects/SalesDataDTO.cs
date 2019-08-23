using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class SalesDataDTO
    {
        public SalesDataDTO()
        {

        }

        public SalesDataDTO(LSSales salesData)
        {
            if (salesData != null)
            {
                DayOfWeek = salesData.BusinessDate.DayOfWeek.ToString();
                DateOfEntry = salesData.BusinessDate;
                CloseTime = salesData.CloseTime;
                Sales = salesData.Sales;
                Discounts = salesData.Discounts;
                Checks = salesData.CheckID != long.MinValue ? salesData.CheckID.ToString() : "0";
                FoodSales = salesData.FoodSales;
                LiquorSales = salesData.LiquorSales;
                BeerDraftSales = salesData.BeerDraftSales;
                BeerBottleSales = salesData.BeerBottleSales;
                NonAlcBevSales = salesData.NonAlcBevSales;
                WineSales = salesData.WineSales;
                RetailBeerSales = salesData.RetailBeerSales;
                RetailSales = salesData.RetailSales;
                TaxAmount = salesData.TaxAmount;
                PaymentAmount = salesData.PaymentAmount;
                CheckNumber = salesData.CheckNumber;
            }
            else
            {
                DateOfEntry = DateTime.Now;
            }
        }

        public SalesDataDTO(RedbookSalesData rbSalesData)
        {
            if (rbSalesData != null)
            {
                DateOfEntry = rbSalesData.CreatedDate;
                Sales = rbSalesData.Sales.HasValue ? rbSalesData.Sales.Value : 0;
                Discounts = rbSalesData.Discounts.HasValue ? rbSalesData.Discounts.Value : 0;
                Checks = rbSalesData.Checks;
                Manager = rbSalesData.CreatedBy;
            }
        }

        [Display(Name = "Day of Week")]
        public string DayOfWeek { get; set; }

        [Display(Name = "Date")]
        [Exportable("Business Date", DataFormatType.Date, false, DisplayFor.Condition_2)]
        public DateTime DateOfEntry { get; set; }

        [Display(Name = "Time")]
        [Exportable("Time", DataFormatType.Time, false, DisplayFor.Condition_1)]
        public DateTime CloseTime { get; set; }

        [Display(Name = "Food Sales")]
        [Exportable("Food Sales", DataFormatType.Currency, true)]
        public decimal FoodSales { get; set; }

        [Display(Name = "Non Alc Bev Sales")]
        [Exportable("Non Alc Bev Sales", DataFormatType.Currency, true)]
        public decimal NonAlcBevSales { get; set; }
        
        [Display(Name = "Beer Bottle Sales")]
        [Exportable("Beer Bottle Sales", DataFormatType.Currency, true)]
        public decimal BeerBottleSales { get; set; }
        
        [Display(Name = "Beer Draft Sales")]
        [Exportable("Beer Draft Sales", DataFormatType.Currency, true)]
        public decimal BeerDraftSales { get; set; }

        [Display(Name = "Liquor Sales")]
        [Exportable("Liquor Sales", DataFormatType.Currency, true)]
        public decimal LiquorSales { get; set; }

        [Display(Name = "Retail Sales")]
        [Exportable("Retail Sales", DataFormatType.Currency, true)]
        public decimal RetailSales { get; set; }

        [Display(Name = "Wine Sales")]
        [Exportable("Wine Sales", DataFormatType.Currency, true)]
        public decimal WineSales { get; set; }

        [Display(Name = "Sales")]
        [Exportable("Total Sales", DataFormatType.Currency, true)]
        public decimal Sales { get; set; }

        [Display(Name = "Discounts")]
        [Exportable("Discount/Comp Amount", DataFormatType.Currency, true)]
        public decimal Discounts { get; set; }

        [Display(Name = "Adjustment Sales")]
        [Exportable("Adjustment Sales", DataFormatType.Currency, true)]
        public decimal AdjustmentSales { get; set; }

        [Display(Name = "Tax Amount")]
        [Exportable("Tax Amount", DataFormatType.Currency, true)]
        public decimal TaxAmount { get; set; }
        
        [Display(Name = "Check Number")]
        [Exportable("Check Number", DataFormatType.String, false, DisplayFor.Condition_1)]
        public string CheckNumber { get; set; }

        [Display(Name = "Retail Beer Sales")]
        public decimal RetailBeerSales { get; set; }
        
        [Display(Name = "Payment Amount")]
        public decimal PaymentAmount { get; set; }
        
        [Display(Name = "Checks")]
        public string Checks { get; set; }

        public string Manager { get; set; }
    }

    public class SalesDataSearchDTO
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

        public SalesDataSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedDateFilter = ReportByDay;
            SelectedDateRangeBegin = DateTime.MinValue;
            SelectedDateRangeEnd = DateTime.MinValue;
        }
    }
}
