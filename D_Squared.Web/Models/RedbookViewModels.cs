using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using D_Squared.Domain.Entities;
using System.Web.Mvc;
using D_Squared.Web.Helpers;
using D_Squared.Domain.TransferObjects;
using System.ComponentModel.DataAnnotations;

namespace D_Squared.Web.Models
{
    public class RedbookEntryBaseViewModel
    {
        public RedbookEntryBaseViewModel()
        {
            SalesForecastDTO = new SalesForecastDTO();
            RedbookEntry = new RedbookEntry();
        }

        public RedbookEntryBaseViewModel(RedbookEntry redbookEntry, SalesForecastDTO salesForecastDTO)
        {
            RedbookEntry = redbookEntry;
            SalesForecastDTO = salesForecastDTO;
        }

        [Display(Name = "Record Date")]
        public SelectList DateSelectList { get; set; }

        [Display(Name = "Record Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public string SelectedDateString { get; set; }

        [Display(Name = "Restaurant")]
        public SelectList LocationSelectList { get; set; }

        public string SelectedLocation { get; set; }

        [Display(Name = "AM Weather")]
        public SelectList WeatherSelectListAM { get; set; }

        [Display(Name = "PM Weather")]
        public SelectList WeatherSelectListPM { get; set; }

        [Display(Name = "AM Manager")]
        public List<SelectListItem> ManagerSelectListAM { get; set; }

        [Display(Name = "PM Manager")]
        public List<SelectListItem> ManagerSelectListPM { get; set; }

        public RedbookEntry RedbookEntry { get; set; }

        public SalesForecastDTO SalesForecastDTO { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        public List<EventDTO> EventDTOs { get; set; }

        public CompetitiveEventListViewModel CompetitiveEventListViewModel { get; set; }

        public CompetitiveEventCreateEditViewModel CompetitiveEventCreateEditViewModel { get; set; }

        public string EndingPeriod { get; set; }

        public string TicketURL { get; set; }
    }

    public class RedbookEntryDetailPartialViewModel
    {
        //for previous year pop up and search page
    }

    public class RedbookEntrySearchViewModel
    {
        //for admin search function
    }
}