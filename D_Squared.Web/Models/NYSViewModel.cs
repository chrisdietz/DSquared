using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Models
{
    public class NYSViewModel
    {
        public EmployeeDTO EmployeeInfo { get; set; }

        public List<NYSDTO> NYSDTOs { get; set; }

        public DateTime AccessTime { get; set; }

        public DateTime EndingPeriod { get; set; }

        public bool CurrentWeekFlag { get; set; }
    }

    public class NYSSearchViewModel
    {
        public NYSSearchDTO SearchDTO { get; set; }

        public List<NYSDTO> SearchResults { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        [Display(Name = "Location")]
        public List<SelectListItem> LocationSelectList { get; set; }
    }
}