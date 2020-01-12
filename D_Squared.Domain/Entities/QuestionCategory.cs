using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace D_Squared.Domain.Entities
{
    public class QuestionCategory
    {
        public int Id { get; set; }

        [StringLength(200)]
        [Required]
        public string Category { get; set; }

        [Required]
        [Display(Name = "Allow Questions Modification")]
        public bool AllowQuestionsModification { get; set; }

        [NotMapped]
        public string DisplayAllowQuestionsModification
        {
            get { return AllowQuestionsModification ? "Yes" : "No"; }
        }

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
