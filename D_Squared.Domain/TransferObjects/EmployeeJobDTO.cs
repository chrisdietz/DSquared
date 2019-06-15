using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Domain.TransferObjects
{
    public class EmployeeJobDTO
    {
        public int Id { get; set; }

        public string StoreNumber { get; set; }

        public string EmployeeNumber { get; set; }

        public string EmployeeName { get; set; }

        public string Job { get; set; }

        public EmployeeJobDTO(EmployeeJob employeeJob)
        {
            Id = employeeJob.Id;
            StoreNumber = employeeJob.StoreNumber;
            EmployeeNumber = employeeJob.EmployeeNumber;
            EmployeeName = employeeJob.EmployeeName;
            Job = employeeJob.Job;
        }
    }
}
