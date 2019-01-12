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
    public class SpreadHourViewModel
    {
        public EmployeeDTO EmployeeInfo { get; set; }

        public List<SpreadHourDTO> SpreadHours { get; set; }

        public DateTime AccessTime { get; set; }

        public DateTime EndingPeriod { get; set; }

        public bool CurrentWeekFlag { get; set; }
    }

    public class SpreadHourSearchViewModel
    {
        public SpreadHourSearchDTO SearchDTO { get; set; }

        public List<SpreadHourDTO> SearchResults { get; set; }
        
        public EmployeeDTO EmployeeInfo { get; set; }

        [Display(Name = "Location")]
        public List<SelectListItem> LocationSelectList { get; set; }
    }
}