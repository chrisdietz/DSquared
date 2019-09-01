using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Models
{
    public class ForcedOutEmployeesDetailViewModel
    {
            
        public EmployeeDTO EmployeeInfo { get; set; }

        public List<ForcedOutEmployeeDTO> ForcedOutEmployeeDTOList { get; set; }

        public DateTime BusinessWeekStartDate { get; set; }

        public DateTime BusinessWeekEndDate { get; set; }

        public bool CurrentWeekFlag { get; set; }
    }

    public class ForcedOutEmployeesDetailSearchViewModel
    {
        public ForcedOutEmployeeSearchDTO SearchDTO { get; set; }

        public List<ForcedOutEmployeeDTO> SearchResults { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        [Display(Name = "Location")]
        public List<SelectListItem> LocationSelectList { get; set; }

        public DateTime BusinessWeekStartDate { get; set; }

        public DateTime BusinessWeekEndDate { get; set; }
    }
}