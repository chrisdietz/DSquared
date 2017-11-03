using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Models
{
    public class HelpDocumentViewModel
    {

    }

    public class HelpDocumentCreateViewModel
    {
        public SelectList ControllerList { get; set; }

        public SelectList ActionList { get; set; }

        [Display(Name = "Controller")]
        public string SelectedController { get; set; }

        [Display(Name = "Action")]
        public string SelectedAction { get; set; }

        [AllowHtml]
        public string HelpHtml { get; set; }

        public HelpDocument HelpDocument { get; set; }
    }
}