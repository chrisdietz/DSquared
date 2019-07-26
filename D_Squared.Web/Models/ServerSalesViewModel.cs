using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Models
{
    public class ServerSalesViewModel
    {
        public EmployeeDTO EmployeeInfo { get; set; }
    }

    public class ServerSalesSearchViewModel
    {
        public ServerSalesSearchDTO SearchDTO { get; set; }

        public List<ServerSalesDTO> SearchResults { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        [Display(Name = "Location")]
        public List<SelectListItem> LocationSelectList { get; set; }

        [Display(Name = "Employee")]
        public List<SelectListItem> EmployeeSelectList { get; set; }

        public DateTime BusinessWeekStartDate { get; set; }

        public DateTime BusinessWeekEndDate { get; set; }
    }
}