using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class PCIComplianceDTO
    {
        public string Question { get; set; }
        public bool Response { get; set; }
        public string MODUserName { get; set; }
    }

    public class PCIComplianceSearchDTO
    {
        [Display(Name = "Business Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }

        public string SelectedLocation { get; set; }
        public PCIComplianceSearchDTO()
        {
            SelectedDate = DateTime.Today;
            SelectedLocation = string.Empty;
        }

    }
       

}
