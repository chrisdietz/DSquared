using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Models
{
    public class TipReportingViewModel
    {
        public List<MakeUpPay> MakeUpPayList { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        public DateTime AccessTime { get; set; }

        public DateTime EndingPeriod { get; set; }
    }

    public class TipReportingSearchViewModel
    {
        public List<MakeUpPay> SearchResults { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        [Display(Name = "Location")]
        public List<SelectListItem> LocationSelectList { get; set; }

        public string SelectedLocation { get; set; }
    }
}