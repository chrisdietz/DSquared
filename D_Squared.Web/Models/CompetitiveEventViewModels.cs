using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D_Squared.Web.Models
{
    public class CompetitiveEventCreateEditViewModel
    {
        public CompetitiveEventCreateEditViewModel()
        {

        }

        public CompetitiveEventCreateEditViewModel(DateTime date, string locationId, int redbookId)
        {
            CompetitiveEvent = new CompetitiveEvent
            {
                Date = date,
                StoreNumber = locationId,
                RedbookEntryId = redbookId
            };

            RedbookDate = date;
        }

        public CompetitiveEvent CompetitiveEvent { get; set; }

        public SelectList DistanceRanges { get; set; }

        public DateTime RedbookDate { get; set; }
    }

    public class CompetitiveEventListViewModel
    {
        public CompetitiveEventListViewModel(List<CompetitiveEvent> events)
        {
            CompetitiveEvents = events;
        }

        public List<CompetitiveEvent> CompetitiveEvents { get; set; }
    }
}