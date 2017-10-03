using D_Squared.Data.Context;
using System.Data.Entity.Migrations;

namespace D_Squared.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<D_SquaredDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            CommandTimeout = 60 * 5;
        }
    }
}
