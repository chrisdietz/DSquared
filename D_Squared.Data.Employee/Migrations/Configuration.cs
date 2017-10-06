using D_Squared.Data.Employees.Context;
using System.Data.Entity.Migrations;
using D_Squared.Domain.Entities;

namespace D_Squared.Data.Employees.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EmployeeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            CommandTimeout = 60 * 5;
        }

        protected override void Seed(EmployeeDbContext context)
        {
            Employee dummy = new Employee()
            {
                EmployeeId = "Rober123456",
                First = "Robert",
                Last = "Tuttle",
                FullName = "Robert Tuttle",
                Location = "123",
                sAMAccountName = "DESKTOP-5Q5V2PH\\Robert",
                ObjectType = "type"
            };

            context.Employees.AddOrUpdate(dummy);
            context.SaveChanges();
        }
    }
}
