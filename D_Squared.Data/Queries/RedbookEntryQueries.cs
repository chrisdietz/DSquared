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

        public RedbookEntry UpdateRedbookEntryRecord(RedbookEntry model, string currentUser)
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
                RedbookEntry updatedRecord = UpdateRedbookEntryRecord(redbookEntry, currentUser);

                ec.SendRedbookSubmitEmail(updatedRecord);
            }
        }
    }
}
