using Datas;
using Features;
using Features.Externals.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        
        public static void ConfigureService(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IIndexingService, IndexingService>();
            services.AddScoped<IRecommendationService, RecommendationService>();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSetting = configuration.GetSection("Jwt");
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters   //tham so xac thuc cho jwt
                {
                    //cap token: true-> dich vu, false->tu cap
                    ValidateIssuer = false,
                    //ValidIssuer = jwtSetting["Issuer"],

                    ValidateAudience = false,
                    //ValidAudience = jwtSetting["Audience"],

                    ClockSkew = TimeSpan.Zero, // bo tg chenh lech
                    ValidateLifetime = true,    //xac thuc thoi gian ton tai cua token

                    //ky vao token
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting["Secret"])),
                    ValidateIssuerSigningKey = true
                };
            });
        }
    }
}
