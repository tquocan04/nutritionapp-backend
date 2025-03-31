using Domains;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Datas.SeedConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = "ADMIN",
                    Name = "Admin",
                },
                new Role
                {
                    Id = "USER",
                    Name = "User",
                }
            );
        }
    }
}
