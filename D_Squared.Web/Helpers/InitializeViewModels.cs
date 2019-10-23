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
using System.Globalization;
using System.Web.Mvc;

namespace D_Squared.Web.Helpers
{
    public abstract class InitializerBase
    {
        protected readonly EmployeeQueries eq;

        protected InitializerBase(EmployeeQueries eq)
        {
            this.eq = eq;
        }

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

        public List<DateTime> GetCurrentWeekAsDates(DateTime selectedDay)
        {
            int currentDayOfWeek = (int)selectedDay.DayOfWeek;
            DateTime sunday = selectedDay.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);

            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            return dates;
        }

        protected DateTime GetStartingDateOfCurrentYear()
        {
            return DateTime.Parse($"1/1/{DateTime.Today.Year}");
        }

        protected DateTime TryParseDateTimeString(string date)
        {
            DateTime.TryParse(date, out DateTime convertedDate);

            return convertedDate;
        }
        
        protected List<string> GetLocationList(EmployeeDTO employee, bool isRegional, bool isDivisional, bool isAdmin, bool includeClosed = true)
        {
            return isRegional ? eq.GetStoreLocationListByRegion(employee, includeClosed)
                                        : isDivisional ? eq.GetStoreLocationListByDivision(employee, includeClosed)
                                        : isAdmin ? eq.GetStoreLocationListForAdmin(includeClosed)
                                        : new List<string>();
        }

        protected List<SelectListItem> GetLocationSelectList(EmployeeDTO employee, CustomClaimsPrincipal user)
        {
            List<string> locationList = GetLocationList(employee, user.IsRegionalManager(), user.IsDivisionalVP(), user.IsDSquaredAdmin(), false);

            List<SelectListItem> locSelectList = null;
            if (locationList.Count() == 0)
            {
                locSelectList = locationList.ToSelectList(null, null, null, true, employee.StoreNumber, employee.StoreNumber);
            }
            else
            {
                locSelectList = locationList.ToSelectList(null, null, null, true, "Select", "Select");
            }

            return locSelectList;
        }

    }

    public class RedbookEntryInitializer : InitializerBase
    {
        private readonly RedbookEntryQueries rbeq;
        private readonly CodeQueries cq;
        private readonly SalesForecastQueries sfq;
        private readonly SalesDataQueries sd;
        //private readonly EmployeeQueries eq; 

        public RedbookEntryInitializer(RedbookEntryQueries rbeq, CodeQueries cq, SalesForecastQueries sfq, SalesDataQueries sd, EmployeeQueries eq) : base(eq)
        {
            this.rbeq = rbeq;
            this.cq = cq;
            this.sfq = sfq;
            this.sd = sd;
            //this.eq = eq;
        }

        #region Helpers

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

        protected List<EventDTO> CreateEventDtos(List<string> eventCodeList, List<RedbookSalesEvent> selectedEvents)
        {
            List<EventDTO> eventPairs = new List<EventDTO>();

            foreach (var eventCode in eventCodeList)
            {
                if (selectedEvents.Select(se => se.Event).Contains(eventCode))
                    eventPairs.Add(new EventDTO() { Event = eventCode, IsChecked = true });
                else
                    eventPairs.Add(new EventDTO() { Event = eventCode, IsChecked = false });
            }

            return eventPairs;
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

        public RedbookEntry BindPostValuesToEntity(RedbookEntry redbookEntry, string selectedDateString, string selectedStoreNumber)
        {
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
                SalesDataDTO = sd.GetSelectedBusinessDaySales(convertedSelectedDate, storeNumber),
                EventDTOs = CreateEventDtos(cq.GetDistinctListByCodeCategory("Event"), redbookEntry.SalesEvents == null ? new List<RedbookSalesEvent>() : redbookEntry.SalesEvents.ToList()),
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

        public SalesDataDTO InitializeSalesDataDTO(string selectedDateString, string storeNumber)
        {
            DateTime convertedSelectedDate = TryParseDateTimeString(selectedDateString);
            return sd.GetSelectedBusinessDaySales(convertedSelectedDate, storeNumber);
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
                EventDTOs = CreateEventDtos(cq.GetDistinctListByCodeCategory("Event"), lastYearRedbook.SalesEvents == null ? new List<RedbookSalesEvent>() : lastYearRedbook.SalesEvents.ToList()).Where(e => e.IsChecked).ToList(),
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

        public SalesDataPartialViewModel InitializeSalesDataPartialViewModel(int redbookEntryId)
        {
            SalesDataPartialViewModel salesDataPartialViewModel = new SalesDataPartialViewModel();
            RedbookEntry redbookEntry = rbeq.FindById(redbookEntryId);
            salesDataPartialViewModel.DailySales = rbeq.GetRedbookDailySalesData(redbookEntryId);
            salesDataPartialViewModel.DateOfEntry = redbookEntry.BusinessDate;
            salesDataPartialViewModel.StoreNumber = redbookEntry.LocationId;
            return salesDataPartialViewModel;
        }
    }

    public class SalesForecastInitializer : InitializerBase
    {
        //private readonly EmployeeQueries eq;
        private readonly BudgetQueries bq;
        private readonly SalesForecastQueries sfq;
        private readonly CodeQueries cq;

        public SalesForecastInitializer(SalesForecastQueries sfq, BudgetQueries bq, EmployeeQueries eq, CodeQueries cq) : base(eq)
        {
            this.sfq = sfq;
            this.bq = bq;
            //this.eq = eq;
            this.cq = cq;
        }

        protected List<SalesForecastDTO> GetSpecificWeekAsSalesForecastDTOList(DateTime selectedDay, string storeNumber)
        {
            List<SalesForecastDTO> theList = new List<SalesForecastDTO>();

            var dates = GetCurrentWeekAsDates(selectedDay);

            foreach (var day in dates)
            {
                if (!sfq.CheckForExistingSalesForecastByDate(day, storeNumber))
                    theList.Add(new SalesForecastDTO(day, sfq.fdq.GetSalesPriorYear(storeNumber, day), sfq.fdq.GetSalesPriorTwoYears(storeNumber, day), sfq.fdq.GetSalesPriorThreeYears(storeNumber, day), sfq.fdq.GetAverageSalesPerMonth(storeNumber, day), sfq.fdq.GetLaborForecast(storeNumber, day)));
                else
                    theList.Add(new SalesForecastDTO(sfq.GetSalesForecastsByDate(day, storeNumber)));
            }

            return theList;
        }

        protected decimal CalculateRecommendedLabor(BudgetDTO dto, decimal forecastAmountTotal, List<string> validLocations, string employeeStore)
        {
            if (validLocations.Contains(employeeStore) && dto.Budget.SalesBudgetAmount != 0 && dto.NumberOfWeeks != 0)
            {
                return ((decimal)dto.Budget.LaborBudgetAmount / dto.NumberOfWeeks) +
                                                ((forecastAmountTotal - ((decimal)dto.Budget.SalesBudgetAmount / dto.NumberOfWeeks))
                                                *
                                                (((decimal)dto.Budget.LaborBudgetAmount / (decimal)dto.Budget.SalesBudgetAmount) / 2));
            }
            else
            {
                return new decimal(-1);
            }
        }

        protected decimal CalculateRecommendedFOHLabor(FY18BudgetDTO dto, decimal recommendedLabor, List<string> validLocations, string employeeStore)
        {
            if (validLocations.Contains(employeeStore) && recommendedLabor != -1)
            {
                decimal numerator = dto.Account60205 + dto.Account60206;
                decimal denominator = dto.Account60205 + dto.Account60206 + dto.Account60210 + dto.Account60211;

                if (numerator == 0)
                    return new decimal(-1);
                else
                {
                    return recommendedLabor * (numerator / denominator);
                }
            }
            else
            {
                return new decimal(-1);
            }
        }

        protected decimal CalculateRecommendedBOHLabor(FY18BudgetDTO dto, decimal reccommendedLabor, List<string> validLocations, string employeeStore)
        {
            if (validLocations.Contains(employeeStore) && reccommendedLabor != -1)
            {
                decimal numerator = dto.Account60210 + dto.Account60211;
                decimal denominator = dto.Account60205 + dto.Account60206 + dto.Account60210 + dto.Account60211;

                if (numerator == 0)
                    return new decimal(-1);
                else
                {
                    return reccommendedLabor * (numerator / denominator);
                }
            }
            else
            {
                return new decimal(-1);
            }
        }

        protected SalesForecastCalculationDTO GetSalesForecastCalculationDTO(List<SalesForecastDTO> weekdays, string storeNumber)
        {
            List<string> validLocations = eq.GetAllValidStoreLocations();
            DateTime now = DateTime.Now.ToLocalTime();
            DateTime thursday = weekdays.Where(w => w.DayOfWeek == "Thursday").FirstOrDefault().DateOfEntry;

            SalesForecastColumnTotalsDTO columnTotalsDTO = new SalesForecastColumnTotalsDTO(weekdays);
            BudgetDTO budgetDTO = bq.GetBudgetByDate(thursday, storeNumber);
            FY18BudgetDTO fy18dto = new FY18BudgetDTO(bq.GetFY18Budgets(storeNumber), now);

            decimal recommendedLabor = CalculateRecommendedLabor(budgetDTO, columnTotalsDTO.ForecastAmountTotal, validLocations, storeNumber);
            decimal recommendedFOH = CalculateRecommendedFOHLabor(fy18dto, recommendedLabor, validLocations, storeNumber);
            decimal recommendedBOH = CalculateRecommendedBOHLabor(fy18dto, recommendedLabor, validLocations, storeNumber);

            SalesForecastCalculationDTO dto = new SalesForecastCalculationDTO(weekdays)
            {
                RecommendedLabor = recommendedLabor,
                Variance = columnTotalsDTO.LaborForecastTotal - recommendedLabor,

                RecommendedFOHLabor = recommendedFOH,
                VarianceFOH = weekdays.Sum(w => w.LaborFOH) - recommendedFOH,

                RecommendedBOHLabor = recommendedBOH,
                VarianceBOH = weekdays.Sum(w => w.LaborBOH) - recommendedBOH
            };

            return dto;
        }

        public SalesForecastViewModel InitializeSalesForecastEntryViewModel(string username, string selectedDate = "")
        {
            DateTime now = DateTime.Now.ToLocalTime();
            DateTime today = DateTime.Today.ToLocalTime();
            DateTime startDate = GetCurrentWeekAsDates(now).First();
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            DateTime.TryParse(selectedDate, out DateTime convertedSelectedDate);

            List<SalesForecastDTO> weekdays = GetSpecificWeekAsSalesForecastDTOList(string.IsNullOrEmpty(selectedDate) ? today : convertedSelectedDate, employee.StoreNumber);
            //auto refresh
            sfq.RefreshSalesForecastData(weekdays, employee.StoreNumber, username);

            SalesForecastViewModel model = new SalesForecastViewModel()
            {
                Weekdays = weekdays,
                AccessTime = now,
                StartDate = startDate,
                EndDate = startDate.AddDays(42),
                EndingPeriod = weekdays.Last().DateOfEntry,
                SelectedDateString = string.IsNullOrEmpty(selectedDate) ? today.ToShortDateString() : selectedDate,

                EmployeeInfo = employee,
                TicketURL = ConfigurationManager.AppSettings["SalesForecastTicketURL"],

                Calculations = GetSalesForecastCalculationDTO(weekdays, employee.StoreNumber)
            };

            return model;
        }

        public SalesForecastDetailPartialViewModel InitializeSalesForecastDetailPartialViewModel(string username, string selectedDate, string storeNumber)
        {
            DateTime now = DateTime.Now.ToLocalTime();
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            DateTime.TryParse(selectedDate, out DateTime convertedSelectedDate);

            List<SalesForecastDTO> weekdays = GetSpecificWeekAsSalesForecastDTOList(convertedSelectedDate, storeNumber);

            SalesForecastDetailPartialViewModel model = new SalesForecastDetailPartialViewModel()
            {
                SalesForecastDTO = weekdays.Where(w => w.DateOfEntry == convertedSelectedDate).FirstOrDefault(),
                Weekdays = weekdays,
                Calculations = GetSalesForecastCalculationDTO(weekdays, storeNumber)
            };

            return model;
        }

        public SalesForecastExportDTO GetSalesForecastExportDTO(string username, string selectedDate)
        {
            DateTime now = DateTime.Now.ToLocalTime();
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            DateTime.TryParse(selectedDate, out DateTime convertedSelectedDate);

            List<SalesForecastDTO> weekdays = GetSpecificWeekAsSalesForecastDTOList(convertedSelectedDate, employee.StoreNumber);

            SalesForecastExportDTO dto = new SalesForecastExportDTO()
            {
                Record = weekdays.Where(w => w.DateOfEntry == convertedSelectedDate).FirstOrDefault(),
                Weekdays = weekdays,
                Calculations = GetSalesForecastCalculationDTO(weekdays, employee.StoreNumber)
            };

            return dto;
        }

        public List<SalesForecastSummaryDTO> GetSalesForecastSummaryList(DateTime selectedDate, List<string> locationList)
        {
            List<SalesForecastSummaryDTO> summaryList = new List<SalesForecastSummaryDTO>();

            foreach (string location in locationList)
            {
                summaryList.Add(new SalesForecastSummaryDTO(location, GetSpecificWeekAsSalesForecastDTOList(selectedDate, location)));
            }

            return summaryList;
        }

        public List<SalesForecastSummaryColumnDTO> GetWeeklyReportColumnTotals(DateTime selectedDay)
        {
            List<DateTime> dates = GetCurrentWeekAsDates(selectedDay);
            List<SalesForecast> theList = sfq.GetSalesForecastByDates(dates);

            List<SalesForecastSummaryColumnDTO> columnSums = new List<SalesForecastSummaryColumnDTO>();

            foreach (var day in dates)
            {
                columnSums.Add(new SalesForecastSummaryColumnDTO(day, theList.Where(tl => tl.BusinessDate == day).ToList()));
            }

            return columnSums;
        }

        public SalesForecastReportViewModel InitializeSalesForecastReportViewModel()
        {
            DateTime now = DateTime.Now.ToLocalTime();
            List<DateTime> theWeek = GetCurrentWeekAsDates(now);

            SalesForecastReportViewModel model = new SalesForecastReportViewModel()
            {
                CurrentDate = now,
                SearchDTO = new SalesForecastSummarySearchDTO(now),
                SummaryList = GetSalesForecastSummaryList(now, eq.GetLocationList()),
                ColumnTotalList = GetWeeklyReportColumnTotals(now),
                EndingPeriod = theWeek.LastOrDefault(),
                StartingPeriod = theWeek.FirstOrDefault()
            };

            return model;
        }

        public SalesForecastReportViewModel InitializeSalesForecastReportViewModel(SalesForecastReportViewModel model)
        {
            DateTime desiredDate = model.SearchDTO.DesiredDate;

            DateTime now = DateTime.Now.ToLocalTime();
            List<DateTime> theWeek = GetCurrentWeekAsDates(desiredDate);

            model.CurrentDate = now;
            model.SearchDTO = new SalesForecastSummarySearchDTO(desiredDate);
            model.SummaryList = GetSalesForecastSummaryList(desiredDate, eq.GetLocationList());
            model.ColumnTotalList = GetWeeklyReportColumnTotals(desiredDate);
            model.EndingPeriod = theWeek.LastOrDefault();
            model.StartingPeriod = theWeek.FirstOrDefault();

            return model;
        }

        public SalesForecastSearchViewModel InitializeSalesForecastSearchViewModel(string username, bool isRegional, bool isDivisional, bool isAdmin)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            List<string> locationList = GetLocationList(employee, isRegional, isDivisional, isAdmin);

            SalesForecastSearchViewModel model = new SalesForecastSearchViewModel()
            {
                SearchViewModel = new SalesForecastSearchPartialViewModel()
                {
                    LocationSelectList = locationList.ToSelectList(null, null, null, true, "Any", "Any"),
                    //WeekdaySelectList = DomainConstants.WeekdayConstants.WeekdayList().ToSelectList(null, null, null, true, "Any", "Any")
                },

                EmployeeInfo = employee
            };

            return model;
        }

        public SalesForecastSearchViewModel InitializeSalesForecastSearchViewModel(SalesForecastSearchDTO searchDTO, string username, bool isRegional, bool isDivisional, bool isAdmin)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            List<string> locationList = GetLocationList(employee, isRegional, isDivisional, isAdmin).Select(l => l.Substring(0, 3)).ToList();
            SalesForecast salesForecast = sfq.GetSalesForecastRecordsByDate(searchDTO.EndDate, searchDTO.LocationId);

            SalesForecastSearchViewModel model = new SalesForecastSearchViewModel()
            {
                SearchViewModel = new SalesForecastSearchPartialViewModel(salesForecast, searchDTO.StartDate, searchDTO.EndDate)
                {
                    LocationSelectList = locationList.ToSelectList(null, null, searchDTO.LocationId, true, "Any", "Any"),
                },

                EmployeeInfo = employee
            };

            model.SearchResults = CreateSearchResultDTOs(sfq.GetSalesForecastEntries(model.SearchViewModel.SearchDTO, locationList, searchDTO.StartDate, searchDTO.EndDate));

            return model;
        }

        protected List<SalesForecastSearchResultDTO> CreateSearchResultDTOs(List<SalesForecast> searchResults)
        {
            List<SalesForecastSearchResultDTO> results = new List<SalesForecastSearchResultDTO>();
            var test1 = searchResults.GroupBy(x => x.StoreNumber);

            foreach (var store in searchResults.GroupBy(x => x.StoreNumber))
            {
                var storeByWeeks = store.GroupBy(x => CultureInfo.CurrentCulture.DateTimeFormat.Calendar
                                                    .GetWeekOfYear(x.BusinessDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday)).ToList();

                foreach(var storeByWeek in storeByWeeks)
                {
                    List<SalesForecast> salesForecasts = storeByWeek.ToList();
                    List<SalesForecastDTO> salesForecastDTOs = new List<SalesForecastDTO>();

                    foreach (var sf in salesForecasts)
                    {
                        salesForecastDTOs.Add(new SalesForecastDTO(sf));                        
                    }

                    var resultDTO = new SalesForecastSearchResultDTO(salesForecasts)
                    {
                        Calculations = GetSalesForecastCalculationDTO(salesForecastDTOs, salesForecasts.FirstOrDefault().StoreNumber)
                    };

                    results.Add(resultDTO);
                }
            }

            return results;
        }

        public SalesForecastSearchViewModel InitializeSalesForecastSearchViewModel(SalesForecastSearchViewModel model, bool isRegional, bool isDivisional, bool isAdmin)
        {
            List<string> locationList = GetLocationList(model.EmployeeInfo, false, false, isAdmin).Select(l => l.Substring(0, 3)).ToList();

            DateTime fiscStart = model.SearchViewModel.SearchDTO.StartDate = GetCurrentWeekAsDates(model.SearchViewModel.SearchDTO.StartDate).FirstOrDefault();
            DateTime fiscEnd = model.SearchViewModel.SearchDTO.EndDate = GetCurrentWeekAsDates(model.SearchViewModel.SearchDTO.EndDate).LastOrDefault();

            model.SearchViewModel.LocationSelectList = locationList.ToSelectList(null, null, model.SearchViewModel.SearchDTO.LocationId, true, "Any", "Any");
            //model.SearchViewModel.WeekdaySelectList = DomainConstants.WeekdayConstants.WeekdayList().ToSelectList(null, null, model.SearchViewModel.SearchDTO.DayOfWeek, true, "Any", "Any");

            List<SalesForecast> queryResults = sfq.GetSalesForecastEntries(model.SearchViewModel.SearchDTO, locationList, fiscStart, fiscEnd);

            model.SearchResults = CreateSearchResultDTOs(queryResults);
            //model.SearchResults = sfq.GetSalesForecastEntries(model.SearchViewModel.SearchDTO, locationList.Select(l => l.Substring(0, 3)).ToList(), fiscStart, fiscEnd);

            return model;
        }


        //public SalesForecastCreateEditPartialViewModel InitializeSalesForecastEditViewModel(int id, string username)
        //{
        //    SalesForecast salesForecast = sfq.FindById(id);
        //    EmployeeDTO employee = eq.GetEmployeeInfo(username);
        //    List<SalesForecastDTO> weekdays = GetSpecificWeekAsSalesForecastDTOList(salesForecast.BusinessDate, employee.StoreNumber);

        //    SalesForecastCreateEditPartialViewModel model = new SalesForecastCreateEditPartialViewModel()
        //    {
        //        SalesForecast = salesForecast,
        //        Calculations = GetSalesForecastCalculationDTO(weekdays, employee)
        //    };

        //    return model;
        //}

        public SalesForecastEditDetailPartialViewModel InitializeSalesForecastEditDetailPartialViewModel(string weekEnding, string storeNumber, string username)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            DateTime.TryParse(weekEnding, out DateTime convertedWeekEnding);

            List<SalesForecastDTO> weekdays = GetSpecificWeekAsSalesForecastDTOList(convertedWeekEnding, storeNumber);

            SalesForecastEditDetailPartialViewModel model = new SalesForecastEditDetailPartialViewModel()
            {
                Weekdays = weekdays,
                Calculations = GetSalesForecastCalculationDTO(weekdays, storeNumber),
                StoreNumber = storeNumber
            };

            return model;
        }
    }

    public class TipReportingInitializer : InitializerBase
    {
        //private readonly EmployeeQueries eq;
        private readonly TipQueries tq;

        public TipReportingInitializer(EmployeeQueries eq, TipQueries tq) : base(eq)
        {
            //this.eq = eq;
            this.tq = tq;
        }

        public TipReportingViewModel InitializeTipReportingViewModel(string username, bool isRegional, bool isDivisional, bool isAdmin, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(username);

            // Get last two weeks
            //List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-14));
            List<MakeUpPay> makeUpPays = new List<MakeUpPay>();
            DateTime lastFiscalWeekEnding = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7)).LastOrDefault();
            makeUpPays.AddRange(tq.GetOutstandingMakeUps(employee.StoreNumber, lastFiscalWeekEnding));
            if (isLastWeek)
            {
                DateTime lastBeforeFiscalWeekEnding = GetCurrentWeekAsDates(DateTime.Today.AddDays(-14)).LastOrDefault();
                makeUpPays.AddRange(tq.GetOutstandingMakeUps(employee.StoreNumber, lastBeforeFiscalWeekEnding));
            }
            TipReportingViewModel model = new TipReportingViewModel()
            {
                EmployeeInfo = employee,
                MakeUpPayList = makeUpPays,
                AccessTime = DateTime.Now,
                EndingPeriod = lastFiscalWeekEnding,
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public TipReportingSearchViewModel InitializeTipReportingSearchViewModel(string username, bool isRegional, bool isDivisional, bool isAdmin)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            List<string> locationList = GetLocationList(employee, isRegional, isDivisional, isAdmin);

            TipReportingSearchViewModel model = new TipReportingSearchViewModel()
            {
                EmployeeInfo = employee,
                SearchResults = new List<MakeUpPay>(),
                LocationSelectList = locationList.ToSelectList(null, null, null, true, "Any", "Any"),
                SearchDTO = new TipReportingSearchDTO()
            };

            return model;
        }

        public TipReportingSearchViewModel InitializeTipReportingSearchViewModel(TipReportingSearchDTO searchDTO, string username, bool isRegional, bool isDivisional, bool isAdmin)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            List<string> locationList = GetLocationList(employee, isRegional, isDivisional, isAdmin);

            DateTime fiscStart = searchDTO.StartDate = GetCurrentWeekAsDates(searchDTO.StartDate).LastOrDefault();
            DateTime fiscEnd = searchDTO.EndDate = GetCurrentWeekAsDates(searchDTO.EndDate).LastOrDefault();

            TipReportingSearchViewModel model = new TipReportingSearchViewModel()
            {
                EmployeeInfo = employee,
                SearchResults = tq.GetOutstandingMakeUps(searchDTO, locationList.Select(l => l.Substring(0, 3)).ToList(), fiscStart, fiscEnd),
                LocationSelectList = locationList.ToSelectList(null, null, searchDTO.SelectedLocation, true, "Any", "Any"),
                SearchDTO = searchDTO
            };

            return model;
        }
    }

    public class SpreadHoursInitializer : InitializerBase
    {
        //private readonly EmployeeQueries eq;
        private readonly SpreadHourQueries shq;

        public SpreadHoursInitializer(EmployeeQueries eq, SpreadHourQueries shq) : base(eq)
        {
            //this.eq = eq;
            this.shq = shq;
        }

        protected List<SpreadHourDTO> GetSpreadHourDTOs(List<SpreadHour> spreadHours)
        {
            List<SpreadHourDTO> dtoList = new List<SpreadHourDTO>();
            List<MinimumWage> minimumWages = shq.GetMinimumWages();

            foreach (SpreadHour sp in spreadHours)
            {
                MinimumWage minWage = new MinimumWage();

                if (minimumWages.Any(mw => mw.StoreNumber == sp.StoreNumber))
                    minWage = minimumWages.Where(mw => mw.StoreNumber == sp.StoreNumber).FirstOrDefault();

                dtoList.Add(new SpreadHourDTO(sp, minWage));
            }

            return dtoList;
        }

        public SpreadHourViewModel InitializeSpreadHourViewModel(string username, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(username);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            SpreadHourViewModel model = new SpreadHourViewModel()
            {
                EmployeeInfo = employee,
                SpreadHours = GetSpreadHourDTOs(shq.GetSpreadHoursByWeek(employee.StoreNumber, daysInWeek.FirstOrDefault(), daysInWeek.LastOrDefault())),
                AccessTime = DateTime.Now,
                EndingPeriod = daysInWeek.LastOrDefault(),
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public SpreadHourSearchViewModel InitializeSpreadHourSearchViewModel(string username, bool isRegional, bool isDivisional, bool isAdmin)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            List<string> locationList = GetLocationList(employee, isRegional, isDivisional, isAdmin);

            SpreadHourSearchViewModel model = new SpreadHourSearchViewModel()
            {
                LocationSelectList = locationList.ToSelectList(null, null, null, true, "Any", "Any"),
                SearchDTO = new SpreadHourSearchDTO(),
                SearchResults = new List<SpreadHourDTO>(),
                EmployeeInfo = employee
            };

            return model;
        }


        public SpreadHourSearchViewModel InitializeSpreadHourSearchViewModel(SpreadHourSearchDTO searchDTO, string username, bool isRegional, bool isDivisional, bool isAdmin)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            List<string> locationList = GetLocationList(employee, isRegional, isDivisional, isAdmin);

            DateTime fiscStart = searchDTO.StartDate = GetCurrentWeekAsDates(searchDTO.StartDate).FirstOrDefault();
            DateTime fiscEnd = searchDTO.EndDate = GetCurrentWeekAsDates(searchDTO.EndDate).LastOrDefault();

            SpreadHourSearchViewModel model = new SpreadHourSearchViewModel()
            {
                EmployeeInfo = employee,
                SearchResults = GetSpreadHourDTOs(shq.GetSpreadHours(searchDTO, locationList.Select(l => l.Substring(0, 3)).ToList(), fiscStart, fiscEnd)),
                LocationSelectList = locationList.ToSelectList(null, null, searchDTO.SelectedLocation, true, "Any", "Any"),
                SearchDTO = searchDTO
            };

            return model;
        }
    }

    public class TipPercentageInitializer : InitializerBase
    {
        //private readonly EmployeeQueries eq;
        private readonly TipPercentageQueries tpq;

        public TipPercentageInitializer(EmployeeQueries eq, TipPercentageQueries tpq) : base(eq)
        {
            //this.eq = eq;
            this.tpq = tpq;
        }

        public List<TipPercentageDTO> GetTipPercentageDTOs(List<TipPercentage> tipPercentages)
        {
            var groupedTipPercentages = tipPercentages.GroupBy(t => t.EmployeeNumber);
            List<TipPercentageDTO> tipPercentageDTOs = new List<TipPercentageDTO>();
            foreach (var group in groupedTipPercentages)
            {
                var totalSales = group.Sum(tp => tp.Sales.Value);
                var totalTips = group.Sum(tp => tp.Tips.Value);
                // Sometimes there are tips but no sales. To avoid devideByZero exception..
                var tipPercent = (totalSales != 0) ? (totalTips * 100) / totalSales : (totalTips * 100);
                tipPercentageDTOs.Add(new TipPercentageDTO
                {
                    EmployeeNumber = group.Key,
                    EmployeeName = group.First().EmployeeName,
                    StoreNumber = group.First().StoreNumber,
                    TotalSalesForTheWeek = totalSales,
                    TotalTipsForTheWeek = totalTips,
                    TipPercentageForTheWeek = tipPercent
                });
            }

            return tipPercentageDTOs;
        }

        public TipPercentageViewModel InitializeTipPercentageViewModel(CustomClaimsPrincipal User, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            List<TipPercentage> tipPercentages = tpq.GetTipPercentagesByWeek(employee.StoreNumber, daysInWeek.FirstOrDefault(), daysInWeek.LastOrDefault());

            var totalStoreSales = tipPercentages.Sum(tp => tp.Sales.Value);
            var totalStoreTips = tipPercentages.Sum(tp => tp.Tips.Value);
            TipPercentageViewModel model = new TipPercentageViewModel()
            {
                EmployeeInfo = employee,
                TipPercentageList = GetTipPercentageDTOs(tipPercentages),
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                // Calculate MAH and store average tip %s for YTD sales and tips
                YTDMAHAverageTipPercentage = tpq.GetMAHAverageTipPercentForGivenDates(GetStartingDateOfCurrentYear(), DateTime.Today),
                YTDStoreAverageTipPercentage = tpq.GetStoreAverageTipPercentForGivenDates(GetStartingDateOfCurrentYear(), DateTime.Today, employee.StoreNumber),
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public TipPercentageSearchViewModel InitializeTipPercentageSearchViewModel(CustomClaimsPrincipal User, TipPercentageSearchDTO searchDTO)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<string> locationList = GetLocationList(employee, User.IsRegionalManager(), User.IsDivisionalVP(), User.IsDSquaredAdmin(), false);

            if (locationList.Count() == 0) locationList.Add(employee.StoreNumber);

            string selectedLocation = employee.StoreNumber;

            if (!string.IsNullOrEmpty(searchDTO.SelectedLocation)) selectedLocation = searchDTO.SelectedLocation.Substring(0, 3);

            List<SelectListItem> tippedEmployees = tpq.GetTippedEmployees(selectedLocation).Select(tp => new SelectListItem
            {
                Text = tp.EmployeeName,
                Value = tp.EmployeeNumber
            }).ToList();
            tippedEmployees.Insert(0, new SelectListItem { Value = "", Text = "All" });

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(searchDTO.SelectedDate);

            List<TipPercentage> tipPercentages = (string.IsNullOrEmpty(searchDTO.SelectedEmployee))
                                                    ? tpq.GetTipPercentagesByWeek(selectedLocation, daysInWeek.FirstOrDefault(), daysInWeek.LastOrDefault())
                                                    : tpq.GetTipPercentagesByEmployeeByWeek(searchDTO.SelectedEmployee, selectedLocation, daysInWeek.FirstOrDefault(), daysInWeek.LastOrDefault());
            var totalStoreSales = tipPercentages.Sum(tp => tp.Sales.Value);
            var totalStoreTips = tipPercentages.Sum(tp => tp.Tips.Value);
            TipPercentageSearchViewModel model = new TipPercentageSearchViewModel()
            {
                EmployeeInfo = employee,
                LocationSelectList = locationList.ToSelectList(null, null, null, true, "Any", "Any"),
                EmployeeSelectList = tippedEmployees,
                SearchResults = GetTipPercentageDTOs(tipPercentages),
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                // Calculate MAH and store average tip %s for YTD sales and tips
                YTDMAHAverageTipPercentage = tpq.GetMAHAverageTipPercentForGivenDates(GetStartingDateOfCurrentYear(), DateTime.Today),
                YTDStoreAverageTipPercentage = tpq.GetStoreAverageTipPercentForGivenDates(GetStartingDateOfCurrentYear(), DateTime.Today, selectedLocation)
            };

            return model;
        }

        public TipPercentagePartialViewModel InitializeTipPercentagePartialViewModel(string storeNumber, string employeeNumber, DateTime startDate, DateTime endDate)
        {
            List<TipPercentage> tipPercentages = tpq.GetTipPercentagesByEmployeeByWeek(employeeNumber, storeNumber, startDate, endDate);
            List<TipPercentageDTO> tipPercentDTOs = tipPercentages.ConvertAll(tp => new TipPercentageDTO(tp));
            List<string> employeeJobs = tpq.GetTippedEmployeeByEmployeeNumber(employeeNumber);
            string jobs = string.Join(", ", employeeJobs);
            TipPercentagePartialViewModel model = new TipPercentagePartialViewModel()
            {
                DetailResults = tipPercentDTOs,
                BusinessWeekStartDate = startDate,
                BusinessWeekEndDate = endDate,
                StoreNumber = storeNumber,
                EmployeeName = tipPercentDTOs.FirstOrDefault().EmployeeName,
                Job = jobs,
                // Calculate MAH and store average tip %s for YTD sales and tips
                YTDMAHAverageTipPercentage = tpq.GetMAHAverageTipPercentForGivenDates(GetStartingDateOfCurrentYear(), DateTime.Today),
                YTDStoreAverageTipPercentage = tpq.GetStoreAverageTipPercentForGivenDates(GetStartingDateOfCurrentYear(), DateTime.Today, storeNumber)
            };

            return model;
        }
        
    }

    public class NYSInitializer : InitializerBase
    {
        //private readonly EmployeeQueries eq;
        private readonly NYSQueries nysq;

        public NYSInitializer(EmployeeQueries eq, NYSQueries nysq) : base(eq)
        {
            this.nysq = nysq;
        }

        protected List<NYSDTO> GetNYSDTOs(List<NYS> nysList)
        {
            List<NYSDTO> dtoList = new List<NYSDTO>();
            List<MinimumWage> minimumWages = nysq.GetMinimumWages();

            foreach (NYS nys in nysList)
            {
                MinimumWage minWage = new MinimumWage();

                if (minimumWages.Any(mw => mw.StoreNumber == nys.StoreNumber))
                    minWage = minimumWages.Where(mw => mw.StoreNumber == nys.StoreNumber).FirstOrDefault();

                dtoList.Add(new NYSDTO(nys, minWage));
            }

            return dtoList;
        }

        public NYSViewModel InitializeNYSViewModel(string username, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(username);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            NYSViewModel model = new NYSViewModel()
            {
                EmployeeInfo = employee,
                NYSDTOs = GetNYSDTOs(nysq.GetNYSByWeek(employee.StoreNumber, daysInWeek.FirstOrDefault(), daysInWeek.LastOrDefault())),
                AccessTime = DateTime.Now,
                EndingPeriod = daysInWeek.LastOrDefault(),
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public NYSSearchViewModel InitializeNYSSearchViewModel(string username, bool isRegional, bool isDivisional, bool isAdmin)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            List<string> locationList = GetLocationList(employee, isRegional, isDivisional, isAdmin);

            NYSSearchViewModel model = new NYSSearchViewModel()
            {
                LocationSelectList = locationList.ToSelectList(null, null, null, true, "Any", "Any"),
                SearchDTO = new NYSSearchDTO(),
                SearchResults = new List<NYSDTO>(),
                EmployeeInfo = employee
            };

            return model;
        }


        public NYSSearchViewModel InitializeNYSSearchViewModel(NYSSearchDTO searchDTO, string username, bool isRegional, bool isDivisional, bool isAdmin)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(username);
            List<string> locationList = GetLocationList(employee, isRegional, isDivisional, isAdmin);

            DateTime fiscStart = searchDTO.StartDate = GetCurrentWeekAsDates(searchDTO.StartDate).FirstOrDefault();
            DateTime fiscEnd = searchDTO.EndDate = GetCurrentWeekAsDates(searchDTO.EndDate).LastOrDefault();

            NYSSearchViewModel model = new NYSSearchViewModel()
            {
                EmployeeInfo = employee,
                SearchResults = GetNYSDTOs(nysq.GetNYS(searchDTO, locationList.Select(l => l.Substring(0, 3)).ToList(), fiscStart, fiscEnd)),
                LocationSelectList = locationList.ToSelectList(null, null, searchDTO.SelectedLocation, true, "Any", "Any"),
                SearchDTO = searchDTO
            };

            return model;
        }
    }

    public class SalesReportingInitializer : InitializerBase
    {
        private readonly SalesDataQueries sdq;

        public SalesReportingInitializer(EmployeeQueries eq, SalesDataQueries sdq) : base(eq)
        {
            this.sdq = sdq;
        }

        public SalesReportViewModel InitializeSalesReportViewModel(CustomClaimsPrincipal User, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            List<SalesDataDTO> salesDataDTOs = sdq.GetSalesDataByDateRange(employee.StoreNumber.Substring(0, 3), daysInWeek.FirstOrDefault(), daysInWeek.LastOrDefault());
            SalesReportViewModel model = new SalesReportViewModel
            {
                SalesList = salesDataDTOs,
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                EmployeeInfo = employee,
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public SalesReportSearchViewModel InitializeSalesReportSearchViewModel(CustomClaimsPrincipal user, SalesDataSearchDTO searchDTO)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(user.TruncatedName);

            List<SelectListItem> locSelectList = GetLocationSelectList(employee, user);

            if (string.IsNullOrEmpty(searchDTO.SelectedLocation)) searchDTO.SelectedLocation = employee.StoreNumber;

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(DateTime.Now);
            if(searchDTO.SelectedDateRangeBegin == DateTime.MinValue)
            {
                searchDTO.SelectedDateRangeBegin = daysInWeek.FirstOrDefault();
                searchDTO.SelectedDateRangeEnd = daysInWeek.LastOrDefault();
            }
            var startDate = searchDTO.SelectedDateRangeBegin;
            var endDate = searchDTO.SelectedDateRangeEnd;

            List<SalesDataDTO> salesDataDTOs = (searchDTO.SelectedDateFilter == SalesDataSearchDTO.ReportByDay) ? sdq.GetSalesDataByDay(searchDTO.SelectedLocation.Substring(0, 3), searchDTO.SelectedDate)
                                                                    : sdq.GetSalesDataByDateRange(searchDTO.SelectedLocation.Substring(0, 3), startDate, endDate);
            SalesReportSearchViewModel model = new SalesReportSearchViewModel
            {
                SearchResults = salesDataDTOs,
                BusinessWeekStartDate = startDate,
                BusinessWeekEndDate = endDate,
                EmployeeInfo = employee,
                SearchDTO = searchDTO,
                LocationSelectList = locSelectList
            };

            return model;
        }

        public IdealCashReportViewModel InitializeIdealCashReportViewModel(CustomClaimsPrincipal User, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            List<IdealCashDTO> idealCashDTOs = sdq.GetIdealCashDataByDateRange(employee.StoreNumber.Substring(0, 3), daysInWeek.FirstOrDefault(), daysInWeek.LastOrDefault());
            IdealCashReportViewModel model = new IdealCashReportViewModel
            {
                IdealCashList = idealCashDTOs,
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                EmployeeInfo = employee,
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public IdealCashReportSearchViewModel InitializeIdealCashReportSearchViewModel(CustomClaimsPrincipal user, IdealCashSearchDTO searchDTO)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(user.TruncatedName);

            List<SelectListItem> locSelectList = GetLocationSelectList(employee, user);

            if (string.IsNullOrEmpty(searchDTO.SelectedLocation)) searchDTO.SelectedLocation = employee.StoreNumber;

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(DateTime.Now);
            if (searchDTO.SelectedDateRangeBegin == DateTime.MinValue)
            {
                searchDTO.SelectedDateRangeBegin = daysInWeek.FirstOrDefault();
                searchDTO.SelectedDateRangeEnd = daysInWeek.LastOrDefault();
            }
            var startDate = searchDTO.SelectedDateRangeBegin;
            var endDate = searchDTO.SelectedDateRangeEnd;

            List<IdealCashDTO> idealCashDTOs = sdq.GetIdealCashDataByDateRange(searchDTO.SelectedLocation.Substring(0, 3), startDate, endDate);

            IdealCashReportSearchViewModel model = new IdealCashReportSearchViewModel()
            {
                EmployeeInfo = employee,
                LocationSelectList = locSelectList,
                SearchResults = idealCashDTOs,
                BusinessWeekStartDate = startDate,
                BusinessWeekEndDate = endDate,
                SearchDTO = searchDTO
            };

            return model;
        }

        public PaidInOutViewModel InitializePaidInOutViewModel(CustomClaimsPrincipal User, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            List<PaidInOutDTO> paidInOutDTOs = sdq.GetPaidInOutByDateRangeAndAccountTypeFilter(employee.StoreNumber.Substring(0, 3), daysInWeek.FirstOrDefault(),
                                                                                            daysInWeek.LastOrDefault());
            PaidInOutViewModel model = new PaidInOutViewModel
            {
                PaidInOutList = paidInOutDTOs,
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                EmployeeInfo = employee,
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public PaidInOutSearchViewModel InitializePaidInOutSearchViewModel(CustomClaimsPrincipal user, PaidInOutSearchDTO searchDTO)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(user.TruncatedName);

            List<SelectListItem> locSelectList = GetLocationSelectList(employee, user);

            if (string.IsNullOrEmpty(searchDTO.SelectedLocation)) searchDTO.SelectedLocation = employee.StoreNumber;

            List<PaidInOutDTO> paidInOutDTOs = null;

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(DateTime.Now);
            if (searchDTO.SelectedDateRangeBegin == DateTime.MinValue)
            {
                searchDTO.SelectedDateRangeBegin = daysInWeek.FirstOrDefault();
                searchDTO.SelectedDateRangeEnd = daysInWeek.LastOrDefault();
            }
            var startDate = searchDTO.SelectedDateRangeBegin;
            var endDate = searchDTO.SelectedDateRangeEnd;

            if (searchDTO.SelectedDateFilter == PaidInOutSearchDTO.ReportByDay)
            {
                paidInOutDTOs = sdq.GetPaidInOutByDayAndAccountTypeFilter(searchDTO.SelectedLocation.Substring(0, 3), searchDTO.SelectedDate, searchDTO.SelectedAccountTypeFilter);
            }
            else
            {
                paidInOutDTOs = sdq.GetPaidInOutByDateRangeAndAccountTypeFilter(searchDTO.SelectedLocation.Substring(0, 3), startDate, endDate, searchDTO.SelectedAccountTypeFilter);
            }

            PaidInOutSearchViewModel model = new PaidInOutSearchViewModel
            {
                SearchResults = paidInOutDTOs,
                BusinessWeekStartDate = startDate,
                BusinessWeekEndDate = endDate,
                EmployeeInfo = employee,
                SearchDTO = searchDTO,
                LocationSelectList = locSelectList
            };

            return model;
        }

        public ServerSalesViewModel InitializeServerSalesViewModel(CustomClaimsPrincipal User, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            List<ServerSalesDTO> serverSalesDTOs = sdq.GetServerSalesDTOsByDateRange(employee.StoreNumber, daysInWeek.FirstOrDefault(), daysInWeek.LastOrDefault(), -1);

            ServerSalesViewModel model = new ServerSalesViewModel
            {
                ServerSalesList = serverSalesDTOs,
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                EmployeeInfo = employee,
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public ServerSalesSearchViewModel InitializeServerSalesSearchViewModel(CustomClaimsPrincipal user, ServerSalesSearchDTO searchDTO)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(user.TruncatedName);

            List<SelectListItem> locSelectList = GetLocationSelectList(employee, user);

            if (string.IsNullOrEmpty(searchDTO.SelectedLocation)) searchDTO.SelectedLocation = employee.StoreNumber;

            List<SelectListItem> employees = sdq.GetStoreEmployees(searchDTO.SelectedLocation).Select(e => new SelectListItem
            {
                Text = e.EmployeeName,
                Value = e.EmployeeNumber
            }).ToList();
            employees.Insert(0, new SelectListItem { Value = "-1", Text = "All" });

            List<ServerSalesDTO> serverSalesDTOs = null;

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(DateTime.Now);
            if (searchDTO.SelectedDateRangeBegin == DateTime.MinValue)
            {
                searchDTO.SelectedDateRangeBegin = daysInWeek.FirstOrDefault();
                searchDTO.SelectedDateRangeEnd = daysInWeek.LastOrDefault();
            }
            var startDate = searchDTO.SelectedDateRangeBegin;
            var endDate = searchDTO.SelectedDateRangeEnd;

            if (searchDTO.SelectedDateFilter == ServerSalesSearchDTO.ReportByDay)
            {
                serverSalesDTOs = sdq.GetServerSalesDTOsByDate(searchDTO.SelectedLocation, searchDTO.SelectedDate, Convert.ToInt32(searchDTO.SelectedEmployee));
            }
            else
            {
                serverSalesDTOs = sdq.GetServerSalesDTOsByDateRange(searchDTO.SelectedLocation, startDate, endDate, Convert.ToInt32(searchDTO.SelectedEmployee));
            }

            ServerSalesSearchViewModel model = new ServerSalesSearchViewModel()
            {
                EmployeeInfo = employee,
                LocationSelectList = locSelectList,
                EmployeeSelectList = employees,
                SearchResults = serverSalesDTOs,
                BusinessWeekStartDate = startDate,
                BusinessWeekEndDate = endDate,
                SearchDTO = searchDTO
            };

            return model;
        }

        public HourlySalesViewModel InitializeHourlySalesViewModel(CustomClaimsPrincipal User, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            List<HourlySalesDTO> hourlySalesDTOs = sdq.GetHourlySalesDTOsByDateRange(employee.StoreNumber, daysInWeek.FirstOrDefault(), daysInWeek.LastOrDefault());

            HourlySalesViewModel model = new HourlySalesViewModel
            {
                HourlySalesList = hourlySalesDTOs,
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                EmployeeInfo = employee,
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public HourlySalesSearchViewModel InitializeHourlySalesSearchViewModel(CustomClaimsPrincipal user, HourlySalesSearchDTO searchDTO)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(user.TruncatedName);

            List<SelectListItem> locSelectList = GetLocationSelectList(employee, user);

            if (string.IsNullOrEmpty(searchDTO.SelectedLocation)) searchDTO.SelectedLocation = employee.StoreNumber;

            List<HourlySalesDTO> hourlySalesDTOs = null;

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(DateTime.Now);
            if (searchDTO.SelectedDateRangeBegin == DateTime.MinValue)
            {
                searchDTO.SelectedDateRangeBegin = daysInWeek.FirstOrDefault();
                searchDTO.SelectedDateRangeEnd = daysInWeek.LastOrDefault();
            }
            var startDate = searchDTO.SelectedDateRangeBegin;
            var endDate = searchDTO.SelectedDateRangeEnd;

            if (searchDTO.SelectedDateFilter == HourlySalesSearchDTO.ReportByDay)
            {
                hourlySalesDTOs = sdq.GetHourlySalesDTOsByDate(searchDTO.SelectedLocation, searchDTO.SelectedDate);
            }
            else
            {
                hourlySalesDTOs = sdq.GetHourlySalesDTOsByDateRange(searchDTO.SelectedLocation, startDate, endDate);
            }

            HourlySalesSearchViewModel model = new HourlySalesSearchViewModel()
            {
                EmployeeInfo = employee,
                LocationSelectList = locSelectList,
                SearchResults = hourlySalesDTOs,
                BusinessWeekStartDate = startDate,
                BusinessWeekEndDate = endDate,
                SearchDTO = searchDTO
            };

            return model;
        }

        public MenuMixViewModel InitializeMenuMixViewModel(CustomClaimsPrincipal User, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            List<MenuMixDTO> menuMixDTOs = sdq.GetMenuMixDTOsByDateRange(employee.StoreNumber, daysInWeek.FirstOrDefault(), daysInWeek.LastOrDefault());

            MenuMixViewModel model = new MenuMixViewModel
            {
                MenuMixDTOList = menuMixDTOs,
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                EmployeeInfo = employee,
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public MenuMixSearchViewModel InitializeMenuMixSearchViewModel(CustomClaimsPrincipal user, MenuMixSearchDTO searchDTO)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(user.TruncatedName);

            List<SelectListItem> locSelectList = GetLocationSelectList(employee, user);

            if (string.IsNullOrEmpty(searchDTO.SelectedLocation)) searchDTO.SelectedLocation = employee.StoreNumber;

            List<MenuMixDTO> menuMixDTOs = null;

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(DateTime.Now);
            if (searchDTO.SelectedDateRangeBegin == DateTime.MinValue)
            {
                searchDTO.SelectedDateRangeBegin = daysInWeek.FirstOrDefault();
                searchDTO.SelectedDateRangeEnd = daysInWeek.LastOrDefault();
            }
            var startDate = searchDTO.SelectedDateRangeBegin;
            var endDate = searchDTO.SelectedDateRangeEnd;

            if (searchDTO.SelectedDateFilter == HourlySalesSearchDTO.ReportByDay)
            {
                menuMixDTOs = sdq.GetMenuMixDTOsByDate(searchDTO.SelectedLocation, searchDTO.SelectedDate);
            }
            else
            {
                menuMixDTOs = sdq.GetMenuMixDTOsByDateRange(searchDTO.SelectedLocation, startDate, endDate);
            }

            MenuMixSearchViewModel model = new MenuMixSearchViewModel()
            {
                EmployeeInfo = employee,
                LocationSelectList = locSelectList,
                SearchResults = menuMixDTOs,
                BusinessWeekStartDate = startDate,
                BusinessWeekEndDate = endDate,
                SearchDTO = searchDTO
            };

            return model;
        }

    }

    public class LaborReportsInitializer : InitializerBase
    {
        private readonly LaborDataQueries ldq;
        private readonly CodeQueries cq;

        public LaborReportsInitializer(EmployeeQueries eq, LaborDataQueries ldq, CodeQueries cq) : base(eq)
        {
            this.ldq = ldq;
            this.cq = cq;
        }

        public OvertimeReportingViewModel InitializeOvertimeReportingViewModel(CustomClaimsPrincipal User, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            int defaultHours = 35;

            List<WeeklyTotalDurationDTO> weeklyTotalDurationDTOs = ldq.GetWeeklyTotalDurationDTOs(employee.StoreNumber.Substring(0, 3), daysInWeek.LastOrDefault(), defaultHours);
            OvertimeReportingViewModel model = new OvertimeReportingViewModel
            {
                WeeklyTotalDurationList = weeklyTotalDurationDTOs,
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                EmployeeInfo = employee,
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public OvertimeReportingSearchViewModel InitializeOvertimeReportingSearchViewModel(CustomClaimsPrincipal user, WeeklyTotalDurationSearchDTO searchDTO)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(user.TruncatedName);

            List<SelectListItem> locSelectList = GetLocationSelectList(employee, user);

            if (string.IsNullOrEmpty(searchDTO.SelectedLocation)) searchDTO.SelectedLocation = employee.StoreNumber;

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(searchDTO.SelectedDate);

            List<WeeklyTotalDurationDTO> weeklyTotalDurationDTOs = ldq.GetWeeklyTotalDurationDTOs(searchDTO.SelectedLocation.Substring(0, 3), daysInWeek.LastOrDefault(), searchDTO.SelectedHours);
         
            OvertimeReportingSearchViewModel model = new OvertimeReportingSearchViewModel()
            {
                EmployeeInfo = employee,
                LocationSelectList = locSelectList,
                SearchResults = weeklyTotalDurationDTOs,
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
            };

            return model;
        }

        public LaborSummaryViewModel InitializeLaborSummaryViewModel(CustomClaimsPrincipal User, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            List<LaborDataDTO> laborDataDTOs = ldq.GetLaborDataByDateRangeAndJob(employee.StoreNumber.Substring(0, 3), daysInWeek.FirstOrDefault(), daysInWeek.LastOrDefault());
            LaborSummaryViewModel model = new LaborSummaryViewModel
            {
                LaborDataList = laborDataDTOs,
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                EmployeeInfo = employee,
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public LaborSummarySearchViewModel InitializeLaborSummarySearchViewModel(CustomClaimsPrincipal user, LaborDataSearchDTO searchDTO)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(user.TruncatedName);

            List<SelectListItem> locSelectList = GetLocationSelectList(employee, user);

            if (string.IsNullOrEmpty(searchDTO.SelectedLocation)) searchDTO.SelectedLocation = employee.StoreNumber;

            List<LaborDataDTO> laborDataDTOs = null;

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(DateTime.Now);
            if (searchDTO.SelectedDateRangeBegin == DateTime.MinValue)
            {
                searchDTO.SelectedDateRangeBegin = daysInWeek.FirstOrDefault();
                searchDTO.SelectedDateRangeEnd = daysInWeek.LastOrDefault();
            }
            var startDate = searchDTO.SelectedDateRangeBegin;
            var endDate = searchDTO.SelectedDateRangeEnd;

            if (searchDTO.SelectedDateFilter == LaborDataSearchDTO.ReportByDay)
            {
                if(searchDTO.SelectedJobOrCenterFilter == LaborDataSearchDTO.ReportByJob)
                {
                    laborDataDTOs = ldq.GetLaborDataByDayAndJob(searchDTO.SelectedLocation.Substring(0, 3), searchDTO.SelectedDate, searchDTO.SelectedCenter);
                }
                else
                {
                    laborDataDTOs = ldq.GetLaborDataByDayAndCenter(searchDTO.SelectedLocation.Substring(0, 3), searchDTO.SelectedDate);
                }
            }
            else
            {
                if (searchDTO.SelectedJobOrCenterFilter == LaborDataSearchDTO.ReportByJob)
                {
                    laborDataDTOs = ldq.GetLaborDataByDateRangeAndJob(searchDTO.SelectedLocation.Substring(0, 3), startDate, endDate, searchDTO.SelectedCenter);
                }
                else
                {
                    laborDataDTOs = ldq.GetLaborDataByDateRangeAndCenter(searchDTO.SelectedLocation.Substring(0, 3), startDate, endDate);
                }
            }

            LaborSummarySearchViewModel model = new LaborSummarySearchViewModel
            {
                SearchResults = laborDataDTOs,
                BusinessWeekStartDate = startDate,
                BusinessWeekEndDate = endDate,
                EmployeeInfo = employee,
                SearchDTO = searchDTO,
                LocationSelectList = locSelectList,
                CenterSelectList = cq.GetDistinctListByCodeCategory("Center").ToSelectList(null, true, "All")
            };

            return model;
        }

        public Labor8020ViewModel InitializeLabor8020ViewModel(CustomClaimsPrincipal User, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            List<Labor8020DTO> labor8020DTOs = ldq.GetLabor8020ByDateRangeAnd8020Filter(employee.StoreNumber.Substring(0, 3), daysInWeek.FirstOrDefault(),
                                                        daysInWeek.LastOrDefault());
            Labor8020ViewModel model = new Labor8020ViewModel
            {
                Labor8020List = labor8020DTOs,
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                EmployeeInfo = employee,
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public Labor8020SearchViewModel InitializeLabor8020SearchViewModel(CustomClaimsPrincipal user, Labor8020SearchDTO searchDTO)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(user.TruncatedName);

            List<SelectListItem> locSelectList = GetLocationSelectList(employee, user);

            if (string.IsNullOrEmpty(searchDTO.SelectedLocation)) searchDTO.SelectedLocation = employee.StoreNumber;

            List<Labor8020DTO> labor8020DTOs = null;

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(DateTime.Now);
            if (searchDTO.SelectedDateRangeBegin == DateTime.MinValue)
            {
                searchDTO.SelectedDateRangeBegin = daysInWeek.FirstOrDefault();
                searchDTO.SelectedDateRangeEnd = daysInWeek.LastOrDefault();
            }
            var startDate = searchDTO.SelectedDateRangeBegin;
            var endDate = searchDTO.SelectedDateRangeEnd;

            if (searchDTO.SelectedDateFilter == Labor8020SearchDTO.ReportByDay)
            {
                labor8020DTOs = ldq.GetLabor8020ByDayAnd8020Filter(searchDTO.SelectedLocation.Substring(0, 3), searchDTO.SelectedDate, searchDTO.Selected8020Filter);
            }
            else
            {
                labor8020DTOs = ldq.GetLabor8020ByDateRangeAnd8020Filter(searchDTO.SelectedLocation.Substring(0, 3), startDate, endDate, searchDTO.Selected8020Filter);
            }

            Labor8020SearchViewModel model = new Labor8020SearchViewModel
            {
                SearchResults = labor8020DTOs,
                BusinessWeekStartDate = startDate,
                BusinessWeekEndDate = endDate,
                EmployeeInfo = employee,
                SearchDTO = searchDTO,
                LocationSelectList = locSelectList
            };

            return model;
        }

        public TimeClockDetailViewModel InitializeTimeClockDetailViewModel(CustomClaimsPrincipal User, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            List<TimeClockDetailDTO> timeClockDetailDTOs = ldq.GetTimeClockDetailDTOsByDateRange(employee.StoreNumber.Substring(0, 3), daysInWeek.FirstOrDefault(),
                                                        daysInWeek.LastOrDefault());
            TimeClockDetailViewModel model = new TimeClockDetailViewModel
            {
                TimeClockDetailList = timeClockDetailDTOs,
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                EmployeeInfo = employee,
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public TimeClockDetailSearchViewModel InitializeTimeClockDetailSearchViewModel(CustomClaimsPrincipal user, TimeClockDetailSearchDTO searchDTO)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(user.TruncatedName);

            List<SelectListItem> locSelectList = GetLocationSelectList(employee, user);

            if (string.IsNullOrEmpty(searchDTO.SelectedLocation)) searchDTO.SelectedLocation = employee.StoreNumber;

            List<TimeClockDetailDTO> timeClockDetailDTOs = null;

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(DateTime.Now);
            if (searchDTO.SelectedDateRangeBegin == DateTime.MinValue)
            {
                searchDTO.SelectedDateRangeBegin = daysInWeek.FirstOrDefault();
                searchDTO.SelectedDateRangeEnd = daysInWeek.LastOrDefault();
            }
            var startDate = searchDTO.SelectedDateRangeBegin;
            var endDate = searchDTO.SelectedDateRangeEnd;

            if (searchDTO.SelectedDateFilter == TimeClockDetailSearchDTO.ReportByDay)
            {
                timeClockDetailDTOs = ldq.GetTimeClockDetailDTOsByDate(searchDTO.SelectedLocation.Substring(0, 3), searchDTO.SelectedDate);
            }
            else
            {
                timeClockDetailDTOs = ldq.GetTimeClockDetailDTOsByDateRange(searchDTO.SelectedLocation.Substring(0, 3), startDate, endDate);
            }

            TimeClockDetailSearchViewModel model = new TimeClockDetailSearchViewModel
            {
                SearchResults = timeClockDetailDTOs,
                BusinessWeekStartDate = startDate,
                BusinessWeekEndDate = endDate,
                EmployeeInfo = employee,
                SearchDTO = searchDTO,
                LocationSelectList = locSelectList
            };

            return model;
        }

        public ForcedOutEmployeesDetailViewModel InitializeForcedOutEmployeesDetailViewModel(CustomClaimsPrincipal User, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            List<ForcedOutEmployeeDTO> forcedOutEmployeeDTOs = ldq.GetForcedOutEmployeeDTOsByDateRange(employee.StoreNumber.Substring(0, 3), daysInWeek.FirstOrDefault(),
                                                        daysInWeek.LastOrDefault());
            ForcedOutEmployeesDetailViewModel model = new ForcedOutEmployeesDetailViewModel
            {
                ForcedOutEmployeeDTOList = forcedOutEmployeeDTOs,
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                EmployeeInfo = employee,
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public ForcedOutEmployeesDetailSearchViewModel InitializeForcedOutEmployeesDetailSearchViewModel(CustomClaimsPrincipal user, ForcedOutEmployeeSearchDTO searchDTO)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(user.TruncatedName);

            List<SelectListItem> locSelectList = GetLocationSelectList(employee, user);

            if (string.IsNullOrEmpty(searchDTO.SelectedLocation)) searchDTO.SelectedLocation = employee.StoreNumber;

            List<ForcedOutEmployeeDTO> forcedOutEmployeeDTOs = null;

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(DateTime.Now);
            if (searchDTO.SelectedDateRangeBegin == DateTime.MinValue)
            {
                searchDTO.SelectedDateRangeBegin = daysInWeek.FirstOrDefault();
                searchDTO.SelectedDateRangeEnd = daysInWeek.LastOrDefault();
            }
            var startDate = searchDTO.SelectedDateRangeBegin;
            var endDate = searchDTO.SelectedDateRangeEnd;

            if (searchDTO.SelectedDateFilter == TimeClockDetailSearchDTO.ReportByDay)
            {
                forcedOutEmployeeDTOs = ldq.GetForcedOutEmployeeDTOsByDate(searchDTO.SelectedLocation.Substring(0, 3), searchDTO.SelectedDate);
            }
            else
            {
                forcedOutEmployeeDTOs = ldq.GetForcedOutEmployeeDTOsByDateRange(searchDTO.SelectedLocation.Substring(0, 3), startDate, endDate);
            }

            ForcedOutEmployeesDetailSearchViewModel model = new ForcedOutEmployeesDetailSearchViewModel
            {
                SearchResults = forcedOutEmployeeDTOs,
                BusinessWeekStartDate = startDate,
                BusinessWeekEndDate = endDate,
                EmployeeInfo = employee,
                SearchDTO = searchDTO,
                LocationSelectList = locSelectList
            };

            return model;
        }
    }

    public class MeetingNotesInitializer : InitializerBase
    {
        private readonly StoreManagerQueries smq;

        public MeetingNotesInitializer(StoreManagerQueries smq, EmployeeQueries eq) : base(eq)
        {
            this.smq = smq;
        }

        public MeetingNotesBaseViewModel InitializeMeetingNotesBaseViewModel(string userName, bool createNew = false)
        {
            MeetingNotesBaseViewModel model = new MeetingNotesBaseViewModel()
            {
                EmployeeInfo = eq.GetEmployeeInfo(userName),
            };
            if (!createNew)
            {
                var notesDTO = smq.GetMostRecentNotes(model.EmployeeInfo.StoreNumber);
                if (notesDTO != null) model.NotesDTO = notesDTO;
            }

            return model;
        }

        
        public MeetingNotesListViewModel InitializeMeetingNotesListViewModel(CustomClaimsPrincipal User, bool isLastWeek = false)
        {
            EmployeeDTO employee = eq.GetEmployeeInfo(User.TruncatedName);

            List<DateTime> daysInWeek = GetCurrentWeekAsDates(!isLastWeek ? DateTime.Today : DateTime.Today.AddDays(-7));

            List<MeetingNotesDTO> meetingNotesDTOs = smq.GetMeetingNotes(employee.StoreNumber.Substring(0, 3), daysInWeek.FirstOrDefault(),
                                                                                            daysInWeek.LastOrDefault());
            MeetingNotesListViewModel model = new MeetingNotesListViewModel
            {
                NotesDTOList = meetingNotesDTOs,
                BusinessWeekStartDate = daysInWeek.FirstOrDefault(),
                BusinessWeekEndDate = daysInWeek.LastOrDefault(),
                EmployeeInfo = employee,
                CurrentWeekFlag = !isLastWeek
            };

            return model;
        }

        public MeetingNotesBaseViewModel InitializeMostRecentMeetingNotesViewModel(string store)
        {
            MeetingNotesDTO meetingNotesDTO = smq.GetMostRecentMeetingNotes(store);

            EmployeeDTO noteCreatedByMgr = eq.GetEmployeeInfo(meetingNotesDTO.UpdatedBy ?? meetingNotesDTO.CreatedBy);

            MeetingNotesBaseViewModel model = new MeetingNotesBaseViewModel
            {
                EmployeeInfo = noteCreatedByMgr,
                NotesDTO = meetingNotesDTO
            };

            return model;
        }
    }
}