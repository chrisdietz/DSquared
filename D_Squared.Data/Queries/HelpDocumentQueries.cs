using D_Squared.Data.Context;
using D_Squared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Queries
{
    public class HelpDocumentQueries
    {
        private readonly D_SquaredDbContext db;

        public HelpDocumentQueries(D_SquaredDbContext db)
        {
            this.db = db;
        }

        public bool CheckForExisitingHelpDocument(string action, string controller)
        {
            return db.HelpDocuments.Any(hd => hd.ActionName == action && hd.ControllerName == controller);
        }

        public HelpDocument GetHelpDocument(string action, string controller)
        {
            return db.HelpDocuments.Where(hd => hd.ActionName == action && hd.ControllerName == controller).FirstOrDefault();
        }
    }
}
