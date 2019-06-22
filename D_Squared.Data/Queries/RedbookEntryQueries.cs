using D_Squared.Data.Context;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D_Squared.Data.Commands;
using Newtonsoft.Json;

namespace D_Squared.Data.Queries
{
    public class RedbookEntryQueries
    {
        private readonly D_SquaredDbContext db;

        private EmailCommands ec;

        public RedbookEntryQueries(D_SquaredDbContext db)
        {
            this.db = db;
            ec = new EmailCommands();
        }

        public RedbookEntry FindById(int id)
        {
            return db.RedbookEntries.Find(id);
        }

        public RedbookEntry GetRedbookEntry(DateTime recordDate, string storeNumber)
        {
            return db.RedbookEntries.FirstOrDefault(rbe => rbe.BusinessDate == recordDate && rbe.LocationId == storeNumber);
        }

        public bool RedbookEntryExists(DateTime recordDate, string storeNumber)
        {
            return db.RedbookEntries.Any(rbe => rbe.BusinessDate == recordDate && rbe.LocationId == storeNumber);
        }

        public bool RedbookSalesEventExists(int redbookId, string eventName)
        {
            return db.RedbookSalesEvents.Any(rbse => rbse.RedbookEntryId == redbookId && rbse.Event == eventName);
        }

        public RedbookSalesEvent GetRedbookSalesEvent(int redbookId, string eventName)
        {
            return db.RedbookSalesEvents.Where(rbse => rbse.RedbookEntryId == redbookId && rbse.Event == eventName).FirstOrDefault();
        }

        public List<RedbookSalesEvent> GetRedbookSalesEvents(int redbookId)
        {
            return db.RedbookSalesEvents.Where(rbse => rbse.RedbookEntryId == redbookId).ToList();
        }

        public void AddRedbookSalesEvent(int redbookId, string salesEvent, string username)
        {
            if(!RedbookSalesEventExists(redbookId, salesEvent))
                db.RedbookSalesEvents.Add(new RedbookSalesEvent(redbookId, salesEvent, username));
        }

        public void RemoveRedbookSalesEvent(int redbookId, string salesEvent)
        {
            if (RedbookSalesEventExists(redbookId, salesEvent))
                db.RedbookSalesEvents.Remove(GetRedbookSalesEvent(redbookId, salesEvent));
        }

        public void CompareAndUpdateRedbookSalesEvents(string username, int redbookId, List<EventDTO> salesEvents)
        {
            foreach (EventDTO dtoEvent in salesEvents)
            {
                if(dtoEvent.IsChecked)
                    AddRedbookSalesEvent(redbookId, dtoEvent.Event, username);
                else
                    RemoveRedbookSalesEvent(redbookId, dtoEvent.Event);
            }

            db.SaveChanges();
        }

        public RedbookEntry GetExistingOrSeedEmpty(string selectedDateString, string storeNumber, string currentUser)
        {
            DateTime convertedDate;
            DateTime.TryParse(selectedDateString, out convertedDate);

            if (RedbookEntryExists(convertedDate, storeNumber))
                return GetRedbookEntry(convertedDate, storeNumber);
            else
            {
                SaveRedbookEntry(new RedbookEntry() { BusinessDate = convertedDate, LocationId = storeNumber }, new List<EventDTO>(), currentUser, false);
                return GetRedbookEntry(convertedDate, storeNumber);
            }
        }

        public RedbookEntry UpdateRedbookEntryRecord(RedbookEntry model, List<EventDTO> salesEvents, string currentUser, bool wasSubmitted = false)
        {
            RedbookEntry exisitingRecord = GetRedbookEntry(model.BusinessDate, model.LocationId);

            exisitingRecord.SelectedWeatherAM = model.SelectedWeatherAM;
            exisitingRecord.SelectedWeatherPM = model.SelectedWeatherPM;
            exisitingRecord.DailyNotes = model.DailyNotes;
            exisitingRecord.ManagerOnDutyAM = model.ManagerOnDutyAM;
            exisitingRecord.ManagerOnDutyPM = model.ManagerOnDutyPM;
            exisitingRecord.ToDoToday = model.ToDoToday;
            exisitingRecord.RMIssues = model.RMIssues;
            exisitingRecord.EmployeeNotes = model.EmployeeNotes;
            exisitingRecord.FoodAndBeverage = model.FoodAndBeverage;
            exisitingRecord.MPower = model.MPower;
            exisitingRecord.IsReadOnly = wasSubmitted;
            exisitingRecord.Sales = model.Sales;
            exisitingRecord.Discounts = model.Discounts;
            exisitingRecord.Checks = model.Checks;
            exisitingRecord.LSMActivities = model.LSMActivities;

            exisitingRecord.UpdatedDate = DateTime.Now;
            exisitingRecord.UpdatedBy = currentUser;

            // Save Sales data to RedbookSalesData
            InsertRedbookSalesData(model, currentUser);

            db.SaveChanges();

            CompareAndUpdateRedbookSalesEvents(currentUser, exisitingRecord.Id, salesEvents);

            return exisitingRecord;
        }

        public void InsertRedbookEntryRecord(RedbookEntry model, List<EventDTO> salesEvents, string currentUser, bool insertSalesData)
        {
            model.CreatedBy = currentUser;
            model.UpdatedBy = currentUser;
            model.CreatedDate = DateTime.Now;
            model.UpdatedDate = DateTime.Now;

            RedbookEntry savedEntry = db.RedbookEntries.Add(model);

            // Save Sales data to RedbookSalesData
            if(insertSalesData) InsertRedbookSalesData(model, currentUser);

            db.SaveChanges();

            CompareAndUpdateRedbookSalesEvents(currentUser, savedEntry.Id, salesEvents);
        }

        private void InsertRedbookSalesData(RedbookEntry model, string currentUser)
        {
            RedbookSalesData redbookSalesData = new RedbookSalesData();
            redbookSalesData.RedbookEntryId = model.Id;
            redbookSalesData.Sales = model.Sales;
            redbookSalesData.Discounts = model.Discounts;
            redbookSalesData.Checks = model.Checks;
            redbookSalesData.CreatedBy = currentUser;
            redbookSalesData.CreatedDate = DateTime.Now;

            db.RedbookSalesDatas.Add(redbookSalesData);
        }

        public void SaveRedbookEntry(RedbookEntry redbookEntry, List<EventDTO> salesEvents, string currentUser, bool insertSalesData = true)
        {
            if (RedbookEntryExists(redbookEntry.BusinessDate, redbookEntry.LocationId))
                UpdateRedbookEntryRecord(redbookEntry, salesEvents, currentUser);
            else
                InsertRedbookEntryRecord(redbookEntry, salesEvents, currentUser, insertSalesData);
        }

        public void SubmitRedbookEntry(RedbookEntry redbookEntry, List<EventDTO> salesEvents, SalesForecastExportDTO salesForecast, string currentUser)
        {
            if (RedbookEntryExists(redbookEntry.BusinessDate, redbookEntry.LocationId))
            {
                RedbookEntry updatedRecord = UpdateRedbookEntryRecord(redbookEntry, salesEvents, currentUser, true);

                ec.SendRedbookSubmitEmail(updatedRecord, salesForecast);
            }
        }

        public List<CompetitiveEvent> GetCompetitiveEvents(int redbookId, string storeNumber)
        {
            return db.CompetitiveEvents.Where(ce => ce.RedbookEntryId == redbookId 
                                                        && ce.StoreNumber == storeNumber)
                                       .ToList();
        }

        public List<CompetitiveEvent> GetCompetitiveEvents(RedbookEntry redbookEntry)
        {
            return db.CompetitiveEvents.Where(ce => ce.RedbookEntryId == redbookEntry.Id
                                                        && ce.StoreNumber == redbookEntry.LocationId)
                                       .ToList();
        }

        public void SaveCompetitiveEvent(CompetitiveEvent model, string currentUser)
        {
            model.CreatedBy = currentUser;
            model.UpdatedBy = currentUser;
            model.CreatedDate = DateTime.Now;
            model.UpdatedDate = DateTime.Now;

            db.CompetitiveEvents.Add(model);

            db.SaveChanges();
        }

        public List<RedbookEntry> GetRedbookEntries(RedbookSearchDTO searchDTO, List<string> accessibleLocations)
        {
            decimal zero = new decimal(0);
            string cleanLocation = searchDTO.LocationId == "Any" ? "Any" : searchDTO.LocationId.Substring(0, 3);

            DateTime realSearchEndDate = searchDTO.EndDate.AddDays(1);
            bool modifiedDateRange = searchDTO.StartDate == searchDTO.EndDate ? false : true;

            return db.RedbookEntries.Where(r => (cleanLocation == "Any" ? accessibleLocations.Any(al => al == r.LocationId) : r.LocationId == cleanLocation)
                                                    && (searchDTO.SelectedWeatherAM == "Any" || r.SelectedWeatherAM == searchDTO.SelectedWeatherAM)
                                                    && (searchDTO.SelectedWeatherPM == "Any" || r.SelectedWeatherPM == searchDTO.SelectedWeatherPM)
                                                    && (string.IsNullOrEmpty(searchDTO.ManagerOnDutyAM) || r.ManagerOnDutyAM == searchDTO.ManagerOnDutyAM)
                                                    && (string.IsNullOrEmpty(searchDTO.ManagerOnDutyPM) || r.ManagerOnDutyPM == searchDTO.ManagerOnDutyAM)
                                                    && (!modifiedDateRange ? r.BusinessDate == searchDTO.StartDate : (r.BusinessDate >= searchDTO.StartDate && r.BusinessDate < realSearchEndDate)))
                                    .OrderBy(r => r.LocationId)
                                    .ThenBy(r => r.BusinessDate)
                                    .ToList();
        }

        public void AdminConvertRedbookEventsToChildTable(string username)
        {
            //get all Redbook records where SelectedEvents != null
            List<RedbookEntry> redbookRecordsWithEvents = db.RedbookEntries.Where(rbe => rbe.SelectedEvents != null).ToList();

            foreach (var entry in redbookRecordsWithEvents)
            {
                List<EventDTO> events = JsonConvert.DeserializeObject<List<EventDTO>>(entry.SelectedEvents);

                CompareAndUpdateRedbookSalesEvents(username, entry.Id, events);

                entry.SelectedEvents = null;

                db.SaveChanges();
            }
        }

        public List<SalesDataDTO> GetRedbookDailySalesData(int redbookEntryId)
        {
            List<RedbookSalesData> dailySales = db.RedbookSalesDatas.Where(r => r.RedbookEntryId == redbookEntryId).OrderByDescending(r => r.CreatedDate).ToList();
            List<SalesDataDTO> salesDataDTOs = new List<SalesDataDTO>();
            dailySales.ForEach(ds => salesDataDTOs.Add(new SalesDataDTO(ds)));
            return salesDataDTOs;
        }
    }
}
