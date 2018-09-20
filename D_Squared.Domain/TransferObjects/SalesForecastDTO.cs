using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class SalesForecastDTO
    {
        public SalesForecastDTO()
        {

        }

        public SalesForecastDTO(SalesForecast salesForecast)
        {
            DayOfWeek = salesForecast.BusinessDate.DayOfWeek.ToString();
            DateOfEntry = salesForecast.BusinessDate;
            ForecastAmount = salesForecast.ForecastAmount;
            ForecastAM = salesForecast.ForecastAM;
            ForecastPM = salesForecast.ForecastPM;
            PriorYearSales = salesForecast.ActualPriorYear;
            Prior2YearSales = salesForecast.ActualPrior2Years;
            AverageSalesPerMonth = salesForecast.AvgPrior4Weeks;
            LaborForecast = salesForecast.LaborForecast;
        }

        public SalesForecastDTO(DateTime newSalesForecastDate, decimal priorSales, decimal prior2Sales, decimal averageSales, decimal laborForecast)
        {
            DayOfWeek = newSalesForecastDate.DayOfWeek.ToString();
            DateOfEntry = newSalesForecastDate;
            PriorYearSales = priorSales;
            Prior2YearSales = prior2Sales;
            AverageSalesPerMonth = averageSales;
            LaborForecast = laborForecast;
        }

        public SalesForecastDTO(List<SalesForecast> preexistingForecasts)
        {
            foreach (var forecast in preexistingForecasts)
            {
                DayOfWeek = forecast.BusinessDate.DayOfWeek.ToString();
                DateOfEntry = forecast.BusinessDate;
                ForecastAmount = forecast.ForecastAmount;
                ForecastAM = forecast.ForecastAM;
                ForecastPM = forecast.ForecastPM;
                PriorYearSales = forecast.ActualPriorYear;
                Prior2YearSales = forecast.ActualPrior2Years;
                AverageSalesPerMonth = forecast.AvgPrior4Weeks;
                LaborForecast = forecast.LaborForecast;
            }
        }

        [Display(Name = "Day of Week")]
        public string DayOfWeek { get; set; }

        [Display(Name = "Date")]
        public DateTime DateOfEntry { get; set; }

        [Display(Name = "Total Sales Forecast")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal ForecastAmount { get; set; }

        [Display(Name = "AM Forecast")]
        public decimal ForecastAM { get; set; }

        [Display(Name = "PM Forecast")]
        public decimal ForecastPM { get; set; }

        [Display(Name = "FY17 Sales")]
        public decimal PriorYearSales { get; set; }

        [Display(Name = "FY16 Sales")]
        public decimal Prior2YearSales { get; set; }

        [Display(Name = "6 Week Average")]
        public decimal AverageSalesPerMonth { get; set; }

        [Display(Name = "Labor Forecast")]
        public decimal LaborForecast { get; set; }
    }
}
