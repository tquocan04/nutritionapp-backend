using Domains;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Datas.SeedConfigurations
{
    public class BreakfastConfiguration : IEntityTypeConfiguration<Breakfast>
    {
        public void Configure(EntityTypeBuilder<Breakfast> builder)
        {
            builder.HasOne(b => b.DailyPlan)
                .WithOne(d => d.Breakfast)
                .HasForeignKey<Breakfast>(b => b.DailyPlan_id);
        }
    }
}
