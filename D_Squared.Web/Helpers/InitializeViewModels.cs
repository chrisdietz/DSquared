using D_Squared.Data.Millers.Queries;
using D_Squared.Data.Queries;
using D_Squared.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using Newtonsoft.Json;
using System.Configuration;

namespace D_Squared.Web.Helpers
{
    public class RedbookEntryInitializer
    {
        private readonly RedbookEntryQueries rbeq;
        private readonly CodeQueries cq;
        private readonly SalesForecastQueries sfq;
        private readonly EmployeeQueries eq;

        public RedbookEntryInitializer(RedbookEntryQueries rbeq, CodeQueries cq, SalesForecastQueries sfq, EmployeeQueries eq)
        {
            this.rbeq = rbeq;
            this.cq = cq;
            this.sfq = sfq;
            this.eq = eq;
        }

        #region Helpers
        protected List<string> GetPastWeek()
        {
            return Enumerable.Range(0, 7).Select(i => DateTime.Now.ToLocalTime().Date.AddDays(-i).ToShortDateString()).ToList();
        }

        protected List<string> GetCurrentWeek(DateTime selectedDay)
        {
            int currentDayOfWeek = (int)selectedDay.DayOfWeek;
            DateTime sunday = selectedDay.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days).ToShortDateString()).ToList();

            return dates;
        }

        protected List<EventDTO> CreateEventDtos(List<string> eventCodeList, List<string> selectedEvents)
        {
            List<EventDTO> eventPairs = new List<EventDTO>();

            foreach (var eventCode in eventCodeList)
            {
                if (selectedEvents.Contains(eventCode))
                    eventPairs.Add(new EventDTO() { Event = eventCode, IsChecked = true });
                else
                    eventPairs.Add(new EventDTO() { Event = eventCode, IsChecked = false });
            }

            return eventPairs;
        }

        protected DateTime TryParseDateTimeString(string date)
        {
            DateTime.TryParse(date, out DateTime convertedDate);

            return convertedDate;
        }

        protected string SerializeSelectedEventDTOs(List<EventDTO> list)
        {
            return JsonConvert.SerializeObject(list);
        }

        protected List<EventDTO> DeserializeSelectedEvents(string selectedEvents)
        {
            return JsonConvert.DeserializeObject<List<EventDTO>>(selectedEvents);
        }
        #endregion

        public RedbookEntry BindPostValuesToEntity(RedbookEntry redbookEntry, List<EventDTO> eventDTOs, string selectedDateString, string selectedStoreNumber)
        {
            redbookEntry.SelectedEvents = SerializeSelectedEventDTOs(eventDTOs);
            redbookEntry.BusinessDate = TryParseDateTimeString(selectedDateString);
            redbookEntry.LocationId = selectedStoreNumber;

            return redbookEntry;
        }

        public RedbookEntryBaseViewModel InitializeBaseViewModel(string selectedDate, string storeNumber, string userName)
        {
            DateTime currentDate = DateTime.Today.ToLocalTime();
            DateTime convertedSelectedDate = TryParseDateTimeString(selectedDate);

            RedbookEntry redbookEntry = rbeq.RedbookEntryExists(convertedSelectedDate, storeNumber) ? rbeq.GetRedbookEntry(convertedSelectedDate, storeNumber) : new RedbookEntry();

            RedbookEntryBaseViewModel model = new RedbookEntryBaseViewModel()
            {
                SelectedDateString = selectedDate,
                DateSelectList = GetCurrentWeek(currentDate).ToSelectList(currentDate.ToShortDateString()),
                EndingPeriod = GetCurrentWeek(currentDate).Last(),
                SelectedLocation = storeNumber,
                LocationSelectList = eq.GetLocationList().ToSelectList(storeNumber),
                EmployeeInfo = eq.GetEmployeeInfo(userName),
                SalesForecastDTO = sfq.GetLiveSalesForecastDTO(convertedSelectedDate, storeNumber),
                EventDTOs = !string.IsNullOrEmpty(redbookEntry.SelectedEvents) ? DeserializeSelectedEvents(redbookEntry.SelectedEvents) : CreateEventDtos(cq.GetDistinctListByCodeCategory("Event"), new List<string>()),
                //EventDTOs = redbookEntry.SalesEvents.Count > 0 ? CreateEventDtos(cq.GetDistinctListByCodeCategory("Event"), redbookEntry.SalesEvents.Select(e => e.Event).ToList()) : CreateEventDtos(cq.GetDistinctListByCodeCategory("Event"), new List<string>()),
                //EventDTOs = CreateEventDtos(cq.GetDistinctListByCodeCategory("Event"), redbookEntry.SalesEvents.Select(e => e.Event).ToList()),
                WeatherSelectListAM = cq.GetDistinctListByCodeCategory("Weather").ToSelectList(null, true, "N/A"),
                WeatherSelectListPM = cq.GetDistinctListByCodeCategory("Weather").ToSelectList(null, true, "N/A"),
                ManagerSelectListAM = eq.GetManagersForLocation(storeNumber).ToSelectList("sAMAccountName", "FullName", null, true, "--Select--", string.Empty),
                ManagerSelectListPM = eq.GetManagersForLocation(storeNumber).ToSelectList("sAMAccountName", "FullName", null, true, "--Select--", string.Empty),
                RedbookEntry = redbookEntry,
                TicketURL = ConfigurationManager.AppSettings["RedbookTicketURL"]
            };

            return model;
        }

        public RedbookEntryBaseViewModel InitializeBaseViewModel(RedbookEntryBaseViewModel model, string userName)
        {
            DateTime currentDate = DateTime.Today.ToLocalTime();

            model.DateSelectList = GetPastWeek().ToSelectList(currentDate.ToShortDateString());
            model.LocationSelectList = eq.GetLocationList().ToSelectList(model.RedbookEntry.LocationId);
            model.WeatherSelectListAM = cq.GetDistinctListByCodeCategory("Weather").ToSelectList(model.RedbookEntry.SelectedWeatherAM, true, "N/A");
            model.WeatherSelectListPM = cq.GetDistinctListByCodeCategory("Weather").ToSelectList(model.RedbookEntry.SelectedWeatherPM, true, "N/A");
            model.ManagerSelectListAM = eq.GetManagersForLocation(model.RedbookEntry.LocationId).ToSelectList("sAMAccountName", "FullName", model.RedbookEntry.ManagerOnDutyAM, true, "--Select--", string.Empty);
            model.ManagerSelectListPM = eq.GetManagersForLocation(model.RedbookEntry.LocationId).ToSelectList("sAMAccountName", "FullName", model.RedbookEntry.ManagerOnDutyPM, true, "--Select--", string.Empty);

            model.EmployeeInfo = eq.GetEmployeeInfo(userName);
            model.SalesForecastDTO = sfq.GetLiveSalesForecastDTO(model.RedbookEntry.BusinessDate, model.RedbookEntry.LocationId);

            return model;
        }
    }
}