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

        [Display(Name = "Bartenders & Servers")]
        public List<SelectListItem> EmployeeSelectList { get; set; }
    }

    public class TipPercentageSearchViewModel
    {
        public TipPercentageSearchDTO SearchDTO { get; set; }

        public List<TipPercentageDTO> SearchResults { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        [Display(Name = "Bartenders & Servers")]
        public List<SelectListItem> EmployeeSelectList { get; set; }
    }
}