using Domains;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Datas.SeedConfigurations
{
    public class LunchConfiguration : IEntityTypeConfiguration<Lunch>
    {
        public void Configure(EntityTypeBuilder<Lunch> builder)
        {
            builder.HasOne(l => l.DailyPlan)
                .WithOne(d => d.Lunch)
                .HasForeignKey<Lunch>(l => l.DailyPlan_id);
        }
    }
}
