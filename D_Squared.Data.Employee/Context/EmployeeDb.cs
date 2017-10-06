using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using D_Squared.Domain.Entities;

namespace D_Squared.Data.Employees.Context
{
    public class EmployeeDbContext : IdentityDbContext
    {
        public EmployeeDbContext() : base ("EmployeeDb")
        {
            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 60 * 30;
        }

        //was forced to specify full scope due to conflict with "Employee" namespace
        public DbSet<Employee> Employees { get; set; }

        public static EmployeeDbContext Create()
        {
            return new EmployeeDbContext();
        }
    }
}
