﻿using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace D_Squared.Web.Models
{
    public class DailyDepositViewModel
    {
        public DailyDepositViewModel()
        {
            Weekdays = new List<DepositEntryDTO>();
        }

        public DailyDepositViewModel(List<DepositEntryDTO> weekdays, DateTime accessTime, EmployeeDTO employeeDTO, bool currentWeekFlag)
        {
            Weekdays = weekdays;
            AccessTime = accessTime;
            EndingPeriod = weekdays.Last().DateOfEntry;
            EmployeeInfo = employeeDTO;
            CurrentWeekFlag = currentWeekFlag;
            TicketURL = ConfigurationManager.AppSettings["DailyDepositTicketURL"];
        }

        public DateTime EndingPeriod { get; set; }

        public DateTime AccessTime { get; set; }

        public List<DepositEntryDTO> Weekdays { get; set; }

        public EmployeeDTO EmployeeInfo { get; set; }

        public bool CurrentWeekFlag { get; set; }

        public string TicketURL { get; set; }
    }
}