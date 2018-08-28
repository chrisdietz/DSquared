using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class RedbookEntry
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Store Number")]
        public string LocationId { get; set; }

        [Required]
        [Display(Name = "Business Date")]
        public DateTime BusinessDate { get; set; }

        //json string of selectedEvents
        //outdated, child table 'SalesEvents' stores this data instead
        public string SelectedEvents { get; set; }

        [Display(Name = "AM Weather")]
        public string SelectedWeatherAM { get; set; }

        [Display(Name = "PM Weather")]
        public string SelectedWeatherPM { get; set; }

        [Display(Name = "Daily procedures and observations")]
        public string DailyNotes { get; set; }

        [Display(Name = "AM Manager")]
        public string ManagerOnDutyAM { get; set; }

        [Display(Name = "Review of the shift")]
        public string ManagerNoteAM { get; set; }

        [Display(Name = "PM Manager")]
        public string ManagerOnDutyPM { get; set; }

        [Display(Name = "Review of the shift")]
        public string ManagerNotePM { get; set; }

        [Display(Name = "To Do Today")]
        public string ToDoToday { get; set; }

        [Display(Name = "R&M Issues")]
        public string RMIssues { get; set; }

        [Display(Name = "Employee Notes")]
        public string EmployeeNotes { get; set; }

        [Display(Name = "Food and Beverage")]
        public string FoodAndBeverage { get; set; }

        [Display(Name = "M Power")]
        public string MPower { get; set; }

        public bool IsReadOnly { get; set; }

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

        public virtual ICollection<CompetitiveEvent> CompetitiveEvents { get; set; }

        public virtual ICollection<RedbookSalesEvent> SalesEvents { get; set; }
    }
}
