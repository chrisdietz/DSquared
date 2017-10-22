using D_Squared.Data.Millers.Context;
using System.Data.Entity.Migrations;

namespace D_Squared.Data.Millers.Migrations
{
    internal sealed class ForecastDataDbConfiguration : DbMigrationsConfiguration<ForecastDataDbContext>
    {
        public ForecastDataDbConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            CommandTimeout = 60 * 5;
        }
    }
}
