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
    public class QuestionResponseViewModel
    {
        public EmployeeDTO EmployeeInfo { get; set; }
        public PCIComplianceSearchDTO SearchDTO { get; set; }

        public List<PCIComplianceDTO> pCIComplianceResponses { get; set; }

        [Display(Name = "Location")]
        public List<SelectListItem> LocationSelectList { get; set; }

    }
}