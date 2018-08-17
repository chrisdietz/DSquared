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
using D_Squared.Domain;

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

        protected List<string> GetLocationList(EmployeeDTO employee, bool isRegional, bool isDivisional, bool isAdmin)
        {
            return isRegional ? eq.GetStoreLocationListByRegion(employee)
                                        : isDivisional ? eq.GetStoreLocationListByDivision(employee)
                                        : isAdmin ? eq.GetStoreLocationListForAdmin()
                                        : new List<string>();
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

            RedbookEntry redbookEntry = rbeq.GetExistingOrSeedEmpty(selectedDate, storeNumber, userName);

            RedbookEntryBaseViewModel model = new RedbookEntryBaseViewModel()
            {
                SelectedDateString = selectedDate,
                DateSelectList = GetCurrentWeek(currentDate).ToSelectList(selectedDate),
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
                TicketURL = ConfigurationManager.AppSettings["RedbookTicketURL"],
                CompetitiveEventListViewModel = new CompetitiveEventListViewModel(rbeq.GetCompetitiveEvents(redbookEntry.Id, redbookEntry.LocationId))
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

            model.TicketURL = ConfigurationManager.AppSettings["RedbookTicketURL"];
            model.EndingPeriod = GetCurrentWeek(currentDate).Last();

            model.CompetitiveEventListViewModel = new CompetitiveEventListViewModel(rbeq.GetCompetitiveEvents(model.RedbookEntry.Id, model.RedbookEntry.LocationId));

            return model;
        }

        public CompetitiveEventCreateEditViewModel InitializeCompetitiveEventCreateEditViewModel(int redbookId)
        {
            RedbookEntry parentEntry = rbeq.FindById(redbookId);

            return new CompetitiveEventCreateEditViewModel(parentEntry.BusinessDate, parentEntry.LocationId, redbookId)
            {
                DistanceRanges = DomainConstants.CompetitiveEventConstants.DistanceRanges().ToSelectList(null)
            };
        }

        public CompetitiveEventListViewModel InitializeCompetitiveEventListViewModel(int redbookId, string storeNumber)
        {
            return new CompetitiveEventListViewModel(rbeq.GetCompetitiveEvents(redbookId, storeNumber));
        }

        public CompetitiveEventCreateEditViewModel InitializeCompetitiveEventCreateEditSelectLists(CompetitiveEventCreateEditViewModel model)
        {
            model.DistanceRanges = DomainConstants.CompetitiveEventConstants.DistanceRanges().ToSelectList(model.CompetitiveEvent.Distance);

            return model;
        }

        public RedbookEntryDetailPartialViewModel InitializeRedbookEntryDetailPartialViewModel(int redbookId, string userName, bool isLastYear, string date = "")
        {
            RedbookEntry redbookEntry = redbookId > 0 ? rbeq.FindById(redbookId) : rbeq.GetExistingOrSeedEmpty(date, eq.GetEmployeeInfo(userName).StoreNumber, userName);
            RedbookEntry lastYearRedbook = rbeq.GetExistingOrSeedEmpty(isLastYear ? redbookEntry.BusinessDate.AddDays(-364).ToShortDateString() : redbookEntry.BusinessDate.ToShortDateString(), redbookEntry.LocationId, userName);

            RedbookEntryDetailPartialViewModel model = new RedbookEntryDetailPartialViewModel()
            {
                RedbookEntry = lastYearRedbook,
                SalesForecastDTO = sfq.GetLiveSalesForecastDTO(lastYearRedbook.BusinessDate, lastYearRedbook.LocationId),
                EventDTOs = !string.IsNullOrEmpty(lastYearRedbook.SelectedEvents) ? DeserializeSelectedEvents(lastYearRedbook.SelectedEvents).Where(e => e.IsChecked).ToList() : new List<EventDTO>(),
                CompetitiveEventListViewModel = new CompetitiveEventListViewModel(rbeq.GetCompetitiveEvents(lastYearRedbook.Id, lastYearRedbook.LocationId))
            };

            return model;
        }

        public RedbookEntrySearchViewModel InitializeRedbookEntrySearchViewModel(EmployeeDTO employee, bool isRegional, bool isDivisional, bool isAdmin)
        {
            List<string> locationList = GetLocationList(employee, isRegional, isDivisional, isAdmin);

            RedbookEntrySearchViewModel model = new RedbookEntrySearchViewModel()
            {
                SearchViewModel = new RedbookEntrySearchPartialViewModel()
                {
                    LocationSelectList = locationList.ToSelectList(null, null, null, true, "Any", "Any"),
                    WeatherSelectListAM = cq.GetDistinctListByCodeCategory("Weather").ToSelectList(null, true, "Any"),
                    WeatherSelectListPM = cq.GetDistinctListByCodeCategory("Weather").ToSelectList(null, true, "Any"),
                    ManagerSelectListAM = eq.GetManagersForLocation(locationList).ToSelectList("sAMAccountName", "FullName", null, true, "Any", string.Empty),
                    ManagerSelectListPM = eq.GetManagersForLocation(locationList).ToSelectList("sAMAccountName", "FullName", null, true, "Any", string.Empty),
                },

                EmployeeInfo = employee
            };

            return model;
        }

        public RedbookEntrySearchViewModel InitializeRedbookEntrySearchViewModel(RedbookEntrySearchViewModel model, bool isRegional, bool isDivisional, bool isAdmin)
        {
            List<string> locationList = GetLocationList(model.EmployeeInfo, isRegional, isDivisional, isAdmin);

            model.SearchViewModel.LocationSelectList = locationList.ToSelectList(null, null, model.SearchViewModel.SearchDTO.LocationId, true, "Any", "Any");

            model.SearchViewModel.WeatherSelectListAM = cq.GetDistinctListByCodeCategory("Weather").ToSelectList(model.SearchViewModel.SearchDTO.SelectedWeatherAM, true, "Any");
            model.SearchViewModel.WeatherSelectListPM = cq.GetDistinctListByCodeCategory("Weather").ToSelectList(model.SearchViewModel.SearchDTO.SelectedWeatherPM, true, "Any");
            model.SearchViewModel.ManagerSelectListAM = model.SearchViewModel.SearchDTO.LocationId == "Any" ? eq.GetManagersForLocation(locationList).ToSelectList("sAMAccountName", "FullName", model.SearchViewModel.SearchDTO.ManagerOnDutyAM, true, "Any", string.Empty)
                                                                                                                    : eq.GetManagersForLocation(model.SearchViewModel.SearchDTO.LocationId).ToSelectList("sAMAccountName", "FullName", model.SearchViewModel.SearchDTO.ManagerOnDutyAM, true, "Any", string.Empty);
            model.SearchViewModel.ManagerSelectListPM = model.SearchViewModel.SearchDTO.LocationId == "Any" ? eq.GetManagersForLocation(locationList).ToSelectList("sAMAccountName", "FullName", model.SearchViewModel.SearchDTO.ManagerOnDutyPM, true, "Any", string.Empty)
                                                                                                                    : eq.GetManagersForLocation(model.SearchViewModel.SearchDTO.LocationId).ToSelectList("sAMAccountName", "FullName", model.SearchViewModel.SearchDTO.ManagerOnDutyPM, true, "Any", string.Empty);

            return model;
        }

        public RedbookEntrySearchPartialViewModel FilterDropdownLists(EmployeeDTO employee, string lId, string mAM, string mPM, bool isRegional, bool isDivisional, bool isAdmin)
        {
            List<string> locationList = GetLocationList(employee, isRegional, isDivisional, isAdmin);

            RedbookEntrySearchPartialViewModel model = new RedbookEntrySearchPartialViewModel()
            {
                SearchDTO = new RedbookSearchDTO(lId, mAM, mPM),
                LocationSelectList = locationList.ToSelectList(null, null, lId, true, "Any", "Any"),
                ManagerSelectListAM = lId == "Any" ? eq.GetManagersForLocation(locationList).ToSelectList("sAMAccountName", "FullName", mAM, true, "Any", string.Empty)
                                                    : eq.GetManagersForLocation(lId).ToSelectList("sAMAccountName", "FullName", mAM, true, "Any", string.Empty),
                ManagerSelectListPM = lId == "Any" ? eq.GetManagersForLocation(locationList).ToSelectList("sAMAccountName", "FullName", mPM, true, "Any", string.Empty)
                                                    : eq.GetManagersForLocation(lId).ToSelectList("sAMAccountName", "FullName", mPM, true, "Any", string.Empty)
            };

            return model;
        }
    }
}