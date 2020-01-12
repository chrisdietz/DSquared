using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace D_Squared.Domain.Entities
{
    public class QuestionBank
    {
        public int Id { get; set; }

        public int QuestionCategoryId { get; set; }

        [Required]
        [StringLength(500)]
        public string Question { get; set; }

        [Display(Name = "Is Active")]
        [Required]
        public bool IsActive { get; set; }

        [NotMapped]
        [Display(Name = "Is Active")]
        public string DisplayIsActive
        {
            get { return IsActive ? "Yes" : "No"; }
        }

        [NotMapped]
        public bool IsChecked { get; set; }

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
