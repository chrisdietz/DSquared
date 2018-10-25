using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Models
{
    public class SalesForecastViewModel
    {
        public SalesForecastViewModel()
        {
            Weekdays = new List<SalesForecastDTO>();
        }

        public DateTime EndingPeriod { get; set; }

        public DateTime AccessTime { get; set; }

        public List<SalesForecastDTO> Weekdays { get; set; }

        public SalesForecastCalculationDTO Calculations { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        public string TicketURL { get; set; }

        [Display(Name = "Date Search:")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public string SelectedDateString { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class SalesForecastDetailPartialViewModel
    {
        public SalesForecastDTO SalesForecastDTO { get; set; }

        public SalesForecastCalculationDTO Calculations { get; set; }

        public List<SalesForecastDTO> Weekdays { get; set; }
    }

    public class SalesForecastCreateEditPartialViewModel
    {
        public SalesForecastCalculationDTO Calculations { get; set; }

        public SalesForecast SalesForecast { get; set; }
    }

    public class SalesForecastSearchViewModel
    {
        public SalesForecastSearchViewModel()
        {
            SearchViewModel = new SalesForecastSearchPartialViewModel();
            SearchResults = new List<SalesForecast>();
        }

        public SalesForecastSearchViewModel(SalesForecast salesForecast)
        {
            SearchViewModel = new SalesForecastSearchPartialViewModel(salesForecast);
            SearchResults = new List<SalesForecast>();
        }

        public List<SalesForecast> SearchResults { get; set; }

        public SalesForecastSearchPartialViewModel SearchViewModel { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }
    }

    public class SalesForecastSearchPartialViewModel
    {
        public SalesForecastSearchPartialViewModel()
        {
            SearchDTO = new SalesForecastSearchDTO();
        }

        public SalesForecastSearchPartialViewModel(SalesForecast salesForecast)
        {
            SearchDTO = new SalesForecastSearchDTO(salesForecast);
        }

        [Display(Name = "Location")]
        public List<SelectListItem> LocationSelectList { get; set; }

        [Display(Name = "Day of Week")]
        public List<SelectListItem> WeekdaySelectList { get; set; }

        public SalesForecastSearchDTO SearchDTO { get; set; }
    }
}