using D_Squared.Domain.Entities;
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
        public DateTime DateOfEntry { get; set; }

        [Display(Name = "Time")]
        public DateTime CloseTime { get; set; }

        [Display(Name = "Sales")]
        public decimal Sales { get; set; }

        [Display(Name = "Discounts")]
        public decimal Discounts { get; set; }

        [Display(Name = "Checks")]
        public string Checks { get; set; }

        public string Manager { get; set; }

        [Display(Name = "Food Sales")]
        public decimal FoodSales { get; set; }

        [Display(Name = "Liquor Sales")]
        public decimal LiquorSales { get; set; }

        [Display(Name = "Beer Draft Sales")]
        public decimal BeerDraftSales { get; set; }

        [Display(Name = "Beer Bottle Sales")]
        public decimal BeerBottleSales { get; set; }

        [Display(Name = "Non Alc Bev Sales")]
        public decimal NonAlcBevSales { get; set; }

        [Display(Name = "Wine Sales")]
        public decimal WineSales { get; set; }

        [Display(Name = "Retail Beer Sales")]
        public decimal RetailBeerSales { get; set; }

        [Display(Name = "Retail Sales")]
        public decimal RetailSales { get; set; }

        [Display(Name = "Tax Amount")]
        public decimal TaxAmount { get; set; }

        [Display(Name = "Payment Amount")]
        public decimal PaymentAmount { get; set; }

        [Display(Name = "Adjustment Sales")]
        public decimal AdjustmentSales { get; set; }
    }

    public class SalesDataSearchDTO
    {
        public const string ReportByDay = "ByDay";
        public const string ReportByWeek = "ByWeek";

        [Display(Name = "Business Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }
        public string SelectedReportType { get; set; }

        public string SelectedLocation { get; set; }

        public SalesDataSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
            SelectedReportType = ReportByDay;
        }

        public SalesDataSearchDTO(DateTime selectedDate)
        {
            SelectedDate = selectedDate;
        }
    }
}
