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

        public SalesForecastDTO(DateTime newSalesForecastDate, decimal priorSales, decimal averageSales, decimal laborForecast)
        {
            DayOfWeek = newSalesForecastDate.DayOfWeek.ToString();
            DateOfEntry = newSalesForecastDate;
            PriorYearSales = priorSales;
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
                PriorYearSales = forecast.PriorYearSales;
                AverageSalesPerMonth = forecast.AverageSalesPerMonth;
                LaborForecast = forecast.LaborForecast;
            }
        }

        [Display(Name = "Day of Week")]
        public string DayOfWeek { get; set; }

        [Display(Name = "Date")]
        public DateTime DateOfEntry { get; set; }

        [Display(Name = "Sales Forecast")]
        public decimal ForecastAmount { get; set; }

        [Display(Name = "Last Year's Sales")]
        public decimal PriorYearSales { get; set; }

        [Display(Name = "Monthly Sales Average")]
        public decimal AverageSalesPerMonth { get; set; }

        [Display(Name = "Labor Forecast")]
        public decimal LaborForecast { get; set; }
    }
}
