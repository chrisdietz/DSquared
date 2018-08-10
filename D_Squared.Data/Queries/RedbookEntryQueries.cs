using D_Squared.Data.Context;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D_Squared.Data.Commands;

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

        public RedbookEntry GetExistingOrSeedEmpty(string selectedDateString, string storeNumber, string currentUser)
        {
            DateTime convertedDate;
            DateTime.TryParse(selectedDateString, out convertedDate);

            if (RedbookEntryExists(convertedDate, storeNumber))
                return GetRedbookEntry(convertedDate, storeNumber);
            else
            {
                SaveRedbookEntry(new RedbookEntry() { BusinessDate = convertedDate, LocationId = storeNumber }, currentUser);
                return GetRedbookEntry(convertedDate, storeNumber);
            }
        }

        public RedbookEntry UpdateRedbookEntryRecord(RedbookEntry model, string currentUser, bool wasSubmitted = false)
        {
            RedbookEntry exisitingRecord = GetRedbookEntry(model.BusinessDate, model.LocationId);

            exisitingRecord.SelectedWeatherAM = model.SelectedWeatherAM;
            exisitingRecord.SelectedWeatherPM = model.SelectedWeatherPM;
            exisitingRecord.DailyNotes = model.DailyNotes;
            exisitingRecord.ManagerOnDutyAM = model.ManagerOnDutyAM;
            exisitingRecord.ManagerOnDutyPM = model.ManagerOnDutyPM;
            exisitingRecord.SelectedEvents = model.SelectedEvents;
            exisitingRecord.ToDoToday = model.ToDoToday;
            exisitingRecord.RMIssues = model.RMIssues;
            exisitingRecord.EmployeeNotes = model.EmployeeNotes;
            exisitingRecord.FoodAndBeverage = model.FoodAndBeverage;
            exisitingRecord.MPower = model.MPower;
            exisitingRecord.IsReadOnly = wasSubmitted;

            exisitingRecord.UpdatedDate = DateTime.Now;
            exisitingRecord.UpdatedBy = currentUser;

            db.SaveChanges();

            return exisitingRecord;
        }

        public void InsertRedbookEntryRecord(RedbookEntry model, string currentUser)
        {
            model.CreatedBy = currentUser;
            model.UpdatedBy = currentUser;
            model.CreatedDate = DateTime.Now;
            model.UpdatedDate = DateTime.Now;

            db.RedbookEntries.Add(model);

            db.SaveChanges();
        }

        public void SaveRedbookEntry(RedbookEntry redbookEntry, string currentUser)
        {
            if (RedbookEntryExists(redbookEntry.BusinessDate, redbookEntry.LocationId))
                UpdateRedbookEntryRecord(redbookEntry, currentUser);
            else
                InsertRedbookEntryRecord(redbookEntry, currentUser);
        }

        public void SubmitRedbookEntry(RedbookEntry redbookEntry, string currentUser)
        {
            if (RedbookEntryExists(redbookEntry.BusinessDate, redbookEntry.LocationId))
            {
                RedbookEntry updatedRecord = UpdateRedbookEntryRecord(redbookEntry, currentUser, true);

                ec.SendRedbookSubmitEmail(updatedRecord);
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

        public List<RedbookEntry> GetRedbookEntries(RedbookSearchDTO searchDTO)
        {
            decimal zero = new decimal(0);
            string cleanLocation = searchDTO.LocationId == "Any" ? "Any" : searchDTO.LocationId.Substring(0, 3);

            DateTime realSearchEndDate = searchDTO.EndDate.AddDays(1);
            bool modifiedDateRange = searchDTO.StartDate == searchDTO.EndDate ? false : true;

            return db.RedbookEntries.Where(r => (cleanLocation == "Any" || r.LocationId == cleanLocation)
                                                    && (searchDTO.SelectedWeatherAM == "Any" || r.SelectedWeatherAM == searchDTO.SelectedWeatherAM)
                                                    && (searchDTO.SelectedWeatherPM == "Any" || r.SelectedWeatherPM == searchDTO.SelectedWeatherPM)
                                                    && (string.IsNullOrEmpty(searchDTO.ManagerOnDutyAM) || r.ManagerOnDutyAM == searchDTO.ManagerOnDutyAM)
                                                    && (string.IsNullOrEmpty(searchDTO.ManagerOnDutyPM) || r.ManagerOnDutyPM == searchDTO.ManagerOnDutyAM)
                                                    && (!modifiedDateRange || (r.BusinessDate >= searchDTO.StartDate && r.BusinessDate < realSearchEndDate)))
                                    .OrderBy(r => r.LocationId)
                                    .ThenBy(r => r.BusinessDate)
                                    .ToList();
        }
    }
}
