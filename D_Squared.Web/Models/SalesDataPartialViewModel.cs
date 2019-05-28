using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace D_Squared.Web.Models
{
    public class SalesDataPartialViewModel
    {
        public List<SalesDataDTO> DailySales { get; set; }

        [Display(Name = "Date")]
        public DateTime DateOfEntry { get; set; }

        [Display(Name = "Store Number")]
        public string StoreNumber { get; set; }
    }
}