using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class CompetitiveEvent
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("RedbookEntry")]
        public int RedbookEntryId { get; set; }

        [Required]
        public string StoreNumber { get; set; }

        [Required]
        public string EventType { get; set; }

        [Required]
        public string EventName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Distance { get; set; }

        #region AuditFields
        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Updated By")]
        [StringLength(50)]
        public string UpdatedBy { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Created By")]
        [StringLength(50)]
        public string CreatedBy { get; set; }
        #endregion
    }
}
