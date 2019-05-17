using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class LSSales
    {
        [Key]
        public string CheckID { get; set; }

        [Display(Name = "Date")]
        public DateTime BusinessDate { get; set; }

        [StringLength(3)]
        public string Store { get; set; }

        [Display(Name = "Sales")]
        public decimal Sales { get; set; }

        [Display(Name = "Discounts")]
        public decimal Discounts { get; set; }
    }
}
