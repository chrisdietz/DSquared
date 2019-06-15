using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using D_Squared.Domain.Entities;

namespace D_Squared.Data.Context
{
    public class D_SquaredDbContext : IdentityDbContext
    {
        public D_SquaredDbContext() : base ("D_SquaredDb")
        {
            //// Set Initializer to drop and Re-Create (overides and resets migrations, and Stored Procedures)
            //Database.SetInitializer<D_SquaredDbContext>(new DropCreateDatabaseAlways<D_SquaredDbContext>()); 

            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 60 * 30;
        }

        public DbSet<DailyDeposit> DailyDeposits { get; set; }

        public DbSet<SalesForecast> SalesForecasts { get; set; }

        public DbSet<RedbookEntry> RedbookEntries { get; set; }

        public DbSet<RedbookSalesEvent> RedbookSalesEvents { get; set; }

        public DbSet<CompetitiveEvent> CompetitiveEvents { get; set; }

        public DbSet<HelpDocument> HelpDocuments { get; set; }

        public DbSet<FY18Budget> FY18Budgets { get; set; }

        public DbSet<Budget> Budgets { get; set; }

        public DbSet<Code> Codes { get; set; }

        public DbSet<TipCredit> TipCredits { get; set; }

        public DbSet<MakeUpPay> MakeUpPay { get; set; }

        public DbSet<SpreadHour> SpreadHours { get; set; }

        public DbSet<MinimumWage> MimumumWages { get; set; }

        public DbSet<TipPercentage> TipPercentages { get; set; }

        public DbSet<EmployeeJob> EmployeeJobs { get; set; }

        public DbSet<LSSales> LSSales { get; set; }

        public DbSet<RedbookSalesData> RedbookSalesDatas { get; set; }

        public static D_SquaredDbContext Create()
        {
            return new D_SquaredDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FY18Budget>().ToTable("FY18Budgets");
            modelBuilder.Entity<SpreadHour>().ToTable("SpreadHours");
            modelBuilder.Entity<MinimumWage>().ToTable("MinimumWages");
            modelBuilder.Entity<EmployeeJob>().ToTable("EmployeeJob");
            modelBuilder.Entity<TipPercentage>().ToTable("TipPercentage");

            base.OnModelCreating(modelBuilder);
        }
    }
}
