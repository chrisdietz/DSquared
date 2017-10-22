using D_Squared.Data.Millers.Context;
using System.Data.Entity.Migrations;
using D_Squared.Domain.Entities;

namespace D_Squared.Data.Millers.Migrations
{
    internal sealed class EmployeeDbConfiguration : DbMigrationsConfiguration<EmployeeDbContext>
    {
        public EmployeeDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            CommandTimeout = 60 * 5;
        }
    }
}
