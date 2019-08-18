using D_Squared.Data.Context;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Queries
{
    public class LaborDataQueries
    {
        private readonly D_SquaredDbContext db;
        public LaborDataQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public List<WeeklyTotalDurationDTO> GetWeeklyTotalDurationDTOs(string storeNumber, DateTime weekEnding, int hours)
        {
            List<WeeklyTotalDurationDTO> weeklyTotalDurationDTOs = db.WeeklyTotalDurations.Where(w => w.StoreNumber == storeNumber && w.WeekEnding == weekEnding && w.TotalDuration > hours)
                                                                    .Select(w => new WeeklyTotalDurationDTO
                                                                    {
                                                                        WeekEnding = w.WeekEnding,
                                                                        StoreNumber = w.StoreNumber,
                                                                        StaffName = w.StaffName,
                                                                        TotalDuration = w.TotalDuration,
                                                                        Overtime = w.TotalDuration - hours
                                                                    }).ToList();

            return weeklyTotalDurationDTOs;
        }

        public List<WeeklyTotalDurationDTO> GetWeeklyTotalDurationDTOsByJob(string job, string storeNumber, DateTime weekEnding, int hours)
        {
            List<WeeklyTotalDurationDTO> weeklyTotalDurationDTOs = db.WeeklyTotalDurations.Where(w => w.StoreNumber == storeNumber && w.WeekEnding == weekEnding && w.TotalDuration > hours /*&& w.Job == job*/)
                                                                    .Select(w => new WeeklyTotalDurationDTO
                                                                    {
                                                                        WeekEnding = w.WeekEnding,
                                                                        StoreNumber = w.StoreNumber,
                                                                        StaffName = w.StaffName,
                                                                        TotalDuration = w.TotalDuration,
                                                                        Overtime = w.TotalDuration - hours
                                                                    }).ToList();

            return weeklyTotalDurationDTOs;
        }

        public List<LaborDataDTO> GetLaborDataByDayAndJob(string storeNumber, DateTime businessDate)
        {
            var lsLaborGroup = db.LSLabors.Where(ld => ld.BusinessDate == businessDate.Date && ld.Store == storeNumber)
                                                .GroupBy(ld => ld.JobName);


            return BuildLaborDataDTOs(lsLaborGroup, "Job");
        }

        public List<LaborDataDTO> GetLaborDataByDayAndCenter(string storeNumber, DateTime businessDate)
        {
            var lsLaborGroup = db.LSLabors.Where(ld => ld.BusinessDate == businessDate.Date && ld.Store == storeNumber)
                                        .GroupBy(ld => ld.Center);

            return BuildLaborDataDTOs(lsLaborGroup, "Center");
        }

        public List<LaborDataDTO> GetLaborDataByDateRangeAndJob(string storeNumber, DateTime startDate, DateTime endDate)
        {
            DateTime realEndDate = endDate.AddDays(1);
            var lsLaborGroup = db.LSLabors.Where(ld => ld.BusinessDate >= startDate && ld.BusinessDate < realEndDate && ld.Store == storeNumber)
                                        .GroupBy(ld => ld.JobName);

            return BuildLaborDataDTOs(lsLaborGroup, "Job");
        }

        public List<LaborDataDTO> GetLaborDataByDateRangeAndCenter(string storeNumber, DateTime startDate, DateTime endDate)
        {
            DateTime realEndDate = endDate.AddDays(1);
            var lsLaborGroup = db.LSLabors.Where(ld => ld.BusinessDate >= startDate && ld.BusinessDate < realEndDate && ld.Store == storeNumber)
                                        .GroupBy(ld => ld.Center);

            return BuildLaborDataDTOs(lsLaborGroup, "Center");
        }

        private List<LaborDataDTO> BuildLaborDataDTOs(IQueryable<IGrouping<string, LSLabor>> lsLaborGroup, string reportType)
        {
            List<LaborDataDTO> laborDataDTOs = new List<LaborDataDTO>();
            foreach (var ldGroup in lsLaborGroup)
            {
                LaborDataDTO lDataDTO = new LaborDataDTO
                {
                    RegularHours = ldGroup.Sum(ld => ld.RegularHours),
                    OTHours = ldGroup.Sum(ld => ld.OTHours),
                    RegularPayAmount = ldGroup.Sum(ld => ld.RegularPayAmount),
                    OTPayAmount = ldGroup.Sum(ld => ld.OTPayAmount),
                    TotalHours = ldGroup.Sum(ld => ld.TotalHours),
                    TotalPayAmount = ldGroup.Sum(ld => ld.TotalPayAmount)
                };
                if (reportType == "Job")
                {
                    lDataDTO.JobName = ldGroup.Key;
                }
                else
                {
                    lDataDTO.Center = ldGroup.Key;
                }

                laborDataDTOs.Add(lDataDTO);
            }

            return laborDataDTOs;
        }

        public List<Labor8020DTO> GetLabor8020ByDayAnd8020Filter(string storeNumber, DateTime businessDate, string filter_8020)
        {
            var lsLabor8020s = db.LS8020s.Where(ld => ld.BusinessDate == businessDate.Date && ld.Store.Contains(storeNumber) && ld.P_8020 == filter_8020).ToList();

            return BuildLabor8020DTOs(lsLabor8020s);
        }

        public List<Labor8020DTO> GetLabor8020ByDateRangeAnd8020Filter(string storeNumber, DateTime startDate, DateTime endDate, string filter_8020 = null)
        {
            DateTime realEndDate = endDate.AddDays(1);
            var lsLabor8020s = (filter_8020 == null) ? db.LS8020s.Where(ld => ld.BusinessDate >= startDate && ld.BusinessDate < realEndDate
                                                        && ld.Store.Contains(storeNumber)).ToList()
                                                     : db.LS8020s.Where(ld => ld.BusinessDate >= startDate && ld.BusinessDate < realEndDate
                                                        && ld.Store.Contains(storeNumber) && ld.P_8020 == filter_8020).ToList();

            return BuildLabor8020DTOs(lsLabor8020s);
        }

        private List<Labor8020DTO> BuildLabor8020DTOs(List<LS8020> lsLabor8020s)
        {
            List<Labor8020DTO> labor8020DTOs = new List<Labor8020DTO>();
            foreach (var lS8020 in lsLabor8020s)
            {
                Labor8020DTO l8020DTO = new Labor8020DTO
                {
                    Store = lS8020.Store,
                    BusinessDate = lS8020.BusinessDate,
                    PersonnelNumber = lS8020.PersonnelNumber,
                    EmployeeName = lS8020.EmployeeName,
                    JobName = lS8020.JobName,
                    P_8020 = lS8020.P_8020,
                    P_8020Manager = lS8020.P_8020Manager
                };

                labor8020DTOs.Add(l8020DTO);
            }

            return labor8020DTOs;
        }

        public List<TimeClockDetailDTO> GetTimeClockDetailDTOsByDate(string storeNumber, DateTime businessDate)
        {
            var lsTimeClockDetails = db.LSTimeClockDetails.Where(lt => lt.BusinessDate == businessDate.Date && lt.Store.StartsWith(storeNumber)).ToList();                                        

            return BuildTimeClockDetailDTOs(lsTimeClockDetails);
        }

        public List<TimeClockDetailDTO> GetTimeClockDetailDTOsByDateRange(string storeNumber, DateTime startDate, DateTime endDate)
        {
            DateTime realEndDate = endDate.AddDays(1);
            var lsTimeClockDetails = db.LSTimeClockDetails.Where(ld => ld.BusinessDate >= startDate && ld.BusinessDate < realEndDate && ld.Store.StartsWith(storeNumber)).ToList();
            
            return BuildTimeClockDetailDTOs(lsTimeClockDetails); 
        }

        private List<TimeClockDetailDTO> BuildTimeClockDetailDTOs(List<LSTimeClockDetail> lsTimeClockDetails)
        {
            List<TimeClockDetailDTO> timeClockDetailDTOs = new List<TimeClockDetailDTO>();
            foreach (var lsTimeClockDetail in lsTimeClockDetails)
            {
                TimeClockDetailDTO timeClockDetailDTO = new TimeClockDetailDTO
                {
                    Store = lsTimeClockDetail.Store,
                    BusinessDate = lsTimeClockDetail.BusinessDate,
                    PersonnelNUmber = lsTimeClockDetail.PersonnelNUmber,
                    EmployeeName = lsTimeClockDetail.EmployeeName,
                    JobID = lsTimeClockDetail.JobID,
                    Rate = lsTimeClockDetail.Rate,
                    Intime = lsTimeClockDetail.Intime,
                    Outtime = lsTimeClockDetail.Outtime,
                    TotalDuration = lsTimeClockDetail.TotalDuration,
                    TotalAmount = lsTimeClockDetail.TotalAmount,
                    TotalTips = lsTimeClockDetail.TotalTips
                };

                timeClockDetailDTOs.Add(timeClockDetailDTO);
            }

            return timeClockDetailDTOs;
        }
    }
}
