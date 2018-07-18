using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D_Squared.Web.Models
{
    public class CompetitiveEventCreateEditViewModel
    {
        public CompetitiveEvent CompetitiveEvent { get; set; }

        public List<string> DistanceRanges { get; set; }
    }
}