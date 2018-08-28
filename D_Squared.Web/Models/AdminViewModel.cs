using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D_Squared.Web.Models
{
    public class AdminViewModel
    {
        public AdminViewModel(EmployeeDTO employee)
        {
            EmployeeInfo = employee;
            AccessTime = DateTime.Now.ToLocalTime();
        }

        public EmployeeDTO EmployeeInfo { get; set; }

        public DateTime AccessTime { get; set; }
    }
}