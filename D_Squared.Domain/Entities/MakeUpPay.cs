using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace D_Squared.Domain.Entities
{
    [Table("MakeUpPay")]
    public class MakeUpPay
    {
        [Key]
        [Column(Order = 0, TypeName = "date")]
        [Display(Name = "Fiscal Week")]
        public DateTime FiscalWeekEnding { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        [Display(Name = "Location")]
        public string StoreNumber { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StaffID { get; set; }

        public int ExternalID { get; set; }

        [Required]
        [StringLength(80)]
        [Display(Name = "Employee")]
        public string EmployeeName { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int JobID { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Position")]
        public string JobName { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal RegularPayRate { get; set; }

        [Display(Name = "Regular Hours")]
        public decimal? RegularHours { get; set; }

        [Display(Name = "Overtime Hours")]
        public decimal? OvertimeHours { get; set; }

        public decimal? RegularPayAmount { get; set; }

        public decimal? OvertimePayAmount { get; set; }

        [Display(Name = "Net Tips Reported")]
        public decimal? TipsEarned { get; set; }

        [Display(Name = "Tip Credit")]
        public decimal? StateTipCredit { get; set; }

        [Display(Name = "Total Tip Credit")]
        public decimal? TotalTipCredit { get; set; }

        [Column("MakeUpPay")]
        [Display(Name = "Make Up Pay")]
        public decimal? MakeUpPay1 { get; set; }
    }
}
