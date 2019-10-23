using D_Squared.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Squared.Data.Context
{
    public class StoreDB : IdentityDbContext
    {
        public StoreDB(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<StoreDB>(null);
            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 60 * 30;
        }
    }
}
