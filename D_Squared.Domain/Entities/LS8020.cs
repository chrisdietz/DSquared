using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class LS8020
    {
        [Key]
        public long ID { get; set; }
        public string ClockID { get; set; }
        public string Store { get; set; }
        public DateTime BusinessDate { get; set; }
        public int PersonnelNumber { get; set; }
        public string EmployeeName { get; set; }
        public string JobName { get; set; }
        public DateTime Time { get; set; }
        [Column("8020")]
        public string P_8020 { get; set; }
        [Column("8020ManagerID")]
        public int P_8020ManagerID { get; set; }
        [Column("8020Manager")]
        public string P_8020Manager { get; set; }
    }
}