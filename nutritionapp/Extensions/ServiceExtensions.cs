using Datas;
using Features;
using Microsoft.EntityFrameworkCore;

namespace nutritionapp.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Connection"),
                                        sqlOptions => sqlOptions.MigrationsAssembly("Datas"))
                                           .EnableSensitiveDataLogging());
        }

        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
    }
}
