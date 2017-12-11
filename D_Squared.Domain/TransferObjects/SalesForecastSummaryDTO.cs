using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class SalesForecastSummaryDTO
    {
        public SalesForecastSummaryDTO(string location, List<SalesForecastDTO> forecastList)
        {
            LocationNumber = location;
            WeeklyForecastRecords = forecastList;
        }

        public string LocationNumber { get; set; }

        public List<SalesForecastDTO> WeeklyForecastRecords { get; set; }
    }

    public class SalesForecastSummarySearchDTO
    {
        public SalesForecastSummarySearchDTO()
        {
            DesiredDate = DateTime.MinValue;
        }

        public SalesForecastSummarySearchDTO(DateTime currentDate)
        {
            DesiredDate = currentDate;
        }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yy}", ApplyFormatInEditMode = true)]
        public DateTime DesiredDate { get; set; }
    }

    public class SalesForecastSummaryColumnDTO
    {
        public SalesForecastSummaryColumnDTO(DateTime day, List<SalesForecast> salesForecastListByDay)
        {
            DayOfWeek = day;
            TotalSalesForecast = salesForecastListByDay.AsQueryable().Sum(sf => sf.ForecastAmount);
        }

        public DateTime DayOfWeek { get; set; }

        public decimal TotalSalesForecast { get; set; }
    }


}
