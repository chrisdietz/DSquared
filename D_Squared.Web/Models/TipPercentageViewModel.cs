using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Models
{
    public class TipPercentageViewModel
    {
        public EmployeeDTO EmployeeInfo { get; set; }

        [Display(Name = "TM")]
        public List<SelectListItem> EmployeeSelectList { get; set; }

        public List<TipPercentageDTO> TipPercentageList { get; set; }
    }

    public class TipPercentageSearchViewModel
    {
        public TipPercentageSearchDTO SearchDTO { get; set; }

        public List<TipPercentageDTO> SearchResults { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        [Display(Name = "Location")]
        public List<SelectListItem> LocationSelectList { get; set; }

        [Display(Name = "TM")]
        public List<SelectListItem> EmployeeSelectList { get; set; }

        public DateTime BusinessWeekStartDate { get; set; }

        public DateTime BusinessWeekEndDate { get; set; }

        [Display(Name = "YTD MAH Avg Tip %")]
        public decimal YTDMAHAverageTipPercentage { get; set; }

        [Display(Name = "YTD Restaurant Avg Tip %")]
        public decimal YTDStoreAverageTipPercentage { get; set; }
    }

    public class TipPercentagePartialViewModel
    {
        [Display(Name = "TM")]
        public string EmployeeName { get; set; }

        [Display(Name = "Restaurant Number")]
        public string StoreNumber { get; set; }

        public DateTime BusinessWeekStartDate { get; set; }

        public DateTime BusinessWeekEndDate { get; set; }

        [Display(Name = "Job(s)")]
        public string Job { get; set; }

        [Display(Name = "YTD MAH Avg Tip %")]
        public decimal YTDMAHAverageTipPercentage { get; set; }

        [Display(Name = "YTD Restaurant Avg Tip %")]
        public decimal YTDStoreAverageTipPercentage { get; set; }

        public List<TipPercentageDTO> DetailResults { get; set; }

    }
}