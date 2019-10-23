using D_Squared.Data.Context;
using D_Squared.Domain.Entities;
using D_Squared.Domain.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace D_Squared.Data.Queries
{
    public class StoreManagerQueries
    {
        private readonly D_SquaredDbContext db;

        public StoreManagerQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public void InsertMeetingNotes(MeetingNotesDTO model, string currentUser)
        {
            MeetingNote mNote = new MeetingNote
            {
                Store = model.Store,
                Notes = model.Notes,
                HuddleDate = model.HuddleDate.Value,
                CreatedBy = currentUser,
                CreatedDate = DateTime.Now
            };

            db.MeetingNotes.Add(mNote);
            db.SaveChanges();
        }

        public void UpdateMeetingNotes(MeetingNotesDTO model, string currentUser)
        {
            MeetingNote mNote = db.MeetingNotes.Where(m => m.ID == model.ID).First();
            mNote.Notes = model.Notes;
            mNote.HuddleDate = model.HuddleDate.Value;
            mNote.UpdatedBy = currentUser;
            mNote.UpdatedDate = DateTime.Now;

            db.SaveChanges();
        }

        public MeetingNotesDTO GetMostRecentNotes(string storeNumber)
        {
            var notes = db.MeetingNotes.Where(m => m.Store == storeNumber).OrderByDescending(m => m.HuddleDate).FirstOrDefault();
            if (notes != null)
            {
                MeetingNotesDTO notesDTO = new MeetingNotesDTO
                {
                    ID = notes.ID,
                    Store = notes.Store,
                    Notes = notes.Notes,
                    HuddleDate = notes.HuddleDate

                };
                return notesDTO;
            }
            else
            {
                return null;
            }
        }

        public MeetingNotesDTO GetMostRecentMeetingNotes(string storeNumber)
        {
            MeetingNotesDTO meetingNotesDTO = db.MeetingNotes.Where(m => m.Store == storeNumber)
                                                    .OrderByDescending(m => m.HuddleDate)
                                                    .Select(m => new MeetingNotesDTO
                                                    {
                                                        HuddleDate = m.HuddleDate,
                                                        Store = m.Store,
                                                        Notes = m.Notes,
                                                        CreatedBy = m.CreatedBy,
                                                        UpdatedBy = m.UpdatedBy
                                                    }).FirstOrDefault();

            return meetingNotesDTO;
        }

        public List<MeetingNotesDTO> GetMeetingNotes(string storeNumber, DateTime startDate, DateTime endDate)
        {
            DateTime realEndDate = endDate.AddDays(1);
            List<MeetingNotesDTO> meetingNotesDTOs = db.MeetingNotes.Where(m => m.Store == storeNumber && m.HuddleDate >= startDate && m.HuddleDate < realEndDate)
                                                    .OrderByDescending(m => m.HuddleDate)
                                                    .Select(m => new MeetingNotesDTO
                                                    {
                                                        HuddleDate = m.HuddleDate,
                                                        Store = m.Store,
                                                        Notes = m.Notes
                                                    }).ToList();

            return meetingNotesDTOs;
        }
    }
}
