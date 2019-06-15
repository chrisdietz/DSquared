using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.Entities
{
    public class EmployeeJob
    {
        [Key]
        public int Id { get; set; }

        public string StoreNumber { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string Job { get; set; }
    }
}
