using Domains;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Datas.SeedConfigurations
{
    public class DinnerConfiguration : IEntityTypeConfiguration<Dinner>
    {
        public void Configure(EntityTypeBuilder<Dinner> builder)
        {
            builder.HasOne(d => d.DailyPlan)
                .WithOne(d => d.Dinner)
                .HasForeignKey<Dinner>(d => d.DailyPlan_id);
        }
    }
}
