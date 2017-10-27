using D_Squared.Data.Millers.Context;
using System.Data.Entity.Migrations;
using D_Squared.Domain.Entities;
using System;
using System.Linq;

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
