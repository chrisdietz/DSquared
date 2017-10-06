using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class Employee
    {
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string EmployeeId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        [Required]
        public string First { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        [Required]
        public string Last { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar")]
        [Required]
        public string FullName { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        [Required]
        public string Location { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        [Required]
        public string sAMAccountName { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        [Required]
        public string ObjectType { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ObjectID { get; set; }
    }
}
