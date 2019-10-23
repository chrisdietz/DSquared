using System;
using System.ComponentModel.DataAnnotations;

namespace D_Squared.Domain.TransferObjects
{
    public class MeetingNotesDTO
    {
        public long ID { get; set; }
        public string Store { get; set; }
        [Display(Name = "Meeting Notes")]
        [Required]
        public string Notes { get; set; }
        [Display(Name = "Huddle Date")]
        [Required]
        public DateTime? HuddleDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}