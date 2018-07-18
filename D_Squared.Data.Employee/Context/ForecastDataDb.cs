using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using D_Squared.Domain.Entities;

namespace D_Squared.Data.Millers.Context
{
    public class ForecastDataDbContext : IdentityDbContext
    {
        public ForecastDataDbContext() : base("ForecastDataDb")
        {
            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 60 * 30;
        }

        public DbSet<ForecastDatum> ForecastData { get; set; }

        public DbSet<SalesForecast> SalesForecasts { get; set; }

        public static ForecastDataDbContext Create()
        {
            return new ForecastDataDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ForecastDatum>().ToTable("ForecastData");
            modelBuilder.Entity<SalesForecast>().ToTable("SalesForecasts");

            base.OnModelCreating(modelBuilder);
        }
    }
}
