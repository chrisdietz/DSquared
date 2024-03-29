﻿using System;
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
            SalesDataDTO = new SalesDataDTO();            
        }

        public RedbookEntryBaseViewModel(RedbookEntry redbookEntry, SalesForecastDTO salesForecastDTO, SalesDataDTO salesDataDTO)
        {
            RedbookEntry = redbookEntry;
            SalesForecastDTO = salesForecastDTO;
            SalesDataDTO = salesDataDTO;
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

        public SalesDataDTO SalesDataDTO { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        public List<EventDTO> EventDTOs { get; set; }

        public List<SalesDataDTO> RedbookSalesDataDTOs { get; set; }

        public CompetitiveEventListViewModel CompetitiveEventListViewModel { get; set; }

        public CompetitiveEventCreateEditViewModel CompetitiveEventCreateEditViewModel { get; set; }

        public List<QuestionBank> Questions { get; set; }

        public List<PCICompliance> PCIComplianceResponses { get; set; }

        public string EndingPeriod { get; set; }

        public string TicketURL { get; set; }
    }

    public class RedbookEntryDetailPartialViewModel
    {
        public RedbookEntry RedbookEntry { get; set; }

        public SalesForecastDTO SalesForecastDTO { get; set; }

        public SalesDataDTO SalesDataDTO { get; set; }

        public List<EventDTO> EventDTOs { get; set; }

        public CompetitiveEventListViewModel CompetitiveEventListViewModel { get; set; }
    }

    public class RedbookEntrySearchViewModel
    {
        public RedbookEntrySearchViewModel()
        {
            SearchViewModel = new RedbookEntrySearchPartialViewModel();
            SearchResults = new List<RedbookEntry>();
        }

        public List<RedbookEntry> SearchResults { get; set; }

        public RedbookEntrySearchPartialViewModel SearchViewModel { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }
    }

    public class RedbookEntrySearchPartialViewModel
    {
        public RedbookEntrySearchPartialViewModel()
        {
            SearchDTO = new RedbookSearchDTO();
        }

        [Display(Name = "Location")]
        public List<SelectListItem> LocationSelectList { get; set; }

        [Display(Name = "AM Weather")]
        public SelectList WeatherSelectListAM { get; set; }

        [Display(Name = "PM Weather")]
        public SelectList WeatherSelectListPM { get; set; }

        [Display(Name = "AM Manager")]
        public List<SelectListItem> ManagerSelectListAM { get; set; }

        [Display(Name = "PM Manager")]
        public List<SelectListItem> ManagerSelectListPM { get; set; }

        public RedbookSearchDTO SearchDTO { get; set; }
    }
}