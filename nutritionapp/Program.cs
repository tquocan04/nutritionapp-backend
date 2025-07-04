﻿using DotNetEnv;
using Features.DailyJobs;
using Features.Externals.Services;
using Features.UserFeatures.Mapping;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Nest;
using nutritionapp.Extensions;

Env.Load("/app/.env");

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSqlContext(builder.Configuration);

// Hangfire
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseDefaultTypeSerializer()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("Connection")));

builder.Services.AddHangfireServer();
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Features.AssemblyReference).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureRepository();
builder.Services.ConfigureService();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IEmbeddingService, GoogleEmbeddingService>();

builder.Services.AddSingleton<IElasticClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var url = configuration["ElasticsearchSettings:Url"]; // Đọc từ biến môi trường Docker trước

    // Nếu không có, đọc từ appsettings
    if (string.IsNullOrEmpty(url))
        url = configuration.GetValue<string>("ElasticsearchSettings:Url");

    if (string.IsNullOrEmpty(url))
        throw new InvalidOperationException("Elasticsearch URL is not configured.");

    var settings = new ConnectionSettings(new Uri(url!)).DefaultIndex("recipes").PrettyJson();
    return new ElasticClient(settings);
});

builder.Services.AddScoped<IElasticSeeder, ElasticSeeder>();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Description = "Chèn JWT token vào đây"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
});

var app = builder.Build();

app.ConfigureExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Dashboard /hangfire
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new AllowAllAuthorizationFilter() }
});

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<Datas.Context>();
    try
    {
        var elasticSeeder = services.GetRequiredService<IElasticSeeder>();
        await elasticSeeder.SeedAsync();
        if (context.Database.GetPendingMigrations().Any())
        {
            Console.WriteLine("Applying pending migrations...");
            context.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration failed: {ex.Message}");
    }
}

// Lên lịch job chạy mỗi ngày lúc 00:00
RecurringJob.AddOrUpdate<DailyPlanJob>(
    "daily-plan-creation-job",
    job => job.Execute(),
    "0 0 * * *", // 00:00 hàng ngày
    new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh") // UTC+7 theo giờ của docker container
    });

app.Run();

public class AllowAllAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true; // Cho phép tất cả truy cập
    }
}