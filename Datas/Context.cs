using Datas.SeedConfigurations;
using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Datas
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect())
                    {
                        databaseCreator.Create();
                    }

                    if (!databaseCreator.HasTables())
                    {
                        databaseCreator.CreateTables();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database creation failed: {ex.Message}");
            }
        }

        public DbSet<DailyPlan> DailyPlans { get; set; } = null!;
        public DbSet<WeightTracking> WeightTrackings { get; set; } = null!;
        public DbSet<Dinner> Dinners { get; set; } = null!;
        public DbSet<ItemDinner> ItemDinners { get; set; } = null!;
        public DbSet<Breakfast> Breakfasts { get; set; } = null!;
        public DbSet<ItemBreakfast> ItemBreakfasts { get; set; } = null!;
        public DbSet<Lunch> Lunches { get; set; } = null!;
        public DbSet<ItemLunch> ItemLunches { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new BreakfastConfiguration());
            builder.ApplyConfiguration(new DinnerConfiguration());
            builder.ApplyConfiguration(new LunchConfiguration());
        }
    }
}
