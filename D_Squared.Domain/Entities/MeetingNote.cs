using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace D_Squared.Domain.Entities
{
    public class MeetingNote
    {
        [Key]
        public long ID { get; set; }
        public string Store { get; set; }
        public string Notes { get; set; }
        public DateTime HuddleDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
