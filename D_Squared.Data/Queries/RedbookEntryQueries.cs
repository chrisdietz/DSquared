using D_Squared.Data.Context;
using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Queries
{
    public class RedbookEntryQueries
    {
        private readonly D_SquaredDbContext db;

        public RedbookEntryQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public RedbookEntry GetRedbookEntry(DateTime recordDate, string storeNumber)
        {
            return db.RedbookEntries.FirstOrDefault(rbe => rbe.BusinessDate == recordDate && rbe.LocationId == storeNumber);
        }

        public bool RedbookEntryExists(DateTime recordDate, string storeNumber)
        {
            return db.RedbookEntries.Any(rbe => rbe.BusinessDate == recordDate && rbe.LocationId == storeNumber);
        }

        public void SaveRedbookEntry(RedbookEntry redbookEntry, string currentUser)
        {
            if(RedbookEntryExists(redbookEntry.BusinessDate, redbookEntry.LocationId))
            {
                RedbookEntry exisitingRecord = GetRedbookEntry(redbookEntry.BusinessDate, redbookEntry.LocationId);

                exisitingRecord.SelectedEvents = redbookEntry.SelectedEvents;
                exisitingRecord.SelectedWeatherAM = redbookEntry.SelectedWeatherAM;
                exisitingRecord.SelectedWeatherPM = redbookEntry.SelectedWeatherPM;
                exisitingRecord.DailyNotes = redbookEntry.DailyNotes;
                exisitingRecord.ManagerOnDutyAM = redbookEntry.ManagerOnDutyAM;
                exisitingRecord.ManagerNoteAM = redbookEntry.ManagerNoteAM;
                exisitingRecord.ManagerOnDutyPM = redbookEntry.ManagerOnDutyPM;
                exisitingRecord.ManagerNotePM = redbookEntry.ManagerNotePM;
                exisitingRecord.UpdatedDate = DateTime.Now;
                exisitingRecord.UpdatedBy = currentUser;
            }
            else
            {
                redbookEntry.CreatedBy = currentUser;
                redbookEntry.UpdatedBy = currentUser;
                redbookEntry.CreatedDate = DateTime.Now;
                redbookEntry.UpdatedDate = DateTime.Now;

                db.RedbookEntries.Add(redbookEntry);
            }

            db.SaveChanges();
        }
    }
}
