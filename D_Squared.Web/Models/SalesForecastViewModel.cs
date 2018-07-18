using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace D_Squared.Web.Models
{
    public class SalesForecastViewModel
    {
        public SalesForecastViewModel()
        {
            Weekdays = new List<SalesForecastDTO>();
        }

        public SalesForecastViewModel(List<SalesForecastDTO> weekdays, DateTime accessTime, EmployeeDTO employeeDTO)
        {
            Weekdays = weekdays;
            AccessTime = accessTime;
            EndingPeriod = weekdays.Last().DateOfEntry;
            EmployeeInfo = employeeDTO;
            TicketURL = ConfigurationManager.AppSettings["SalesForecastTicketURL"];
        }

        public DateTime EndingPeriod { get; set; }

        public DateTime AccessTime { get; set; }

        public List<SalesForecastDTO> Weekdays { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        public string TicketURL { get; set; }
    }
}