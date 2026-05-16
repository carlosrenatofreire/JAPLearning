using MundoDev.Mvc.Configurations;
using MundoDev.Mvc.Seeds;

var builder = WebApplication.CreateBuilder(args);

builder.AddDopplerConfiguration();

builder.Services
    .AddContextConfiguration(builder.Configuration)
    .AddAutoMapperConfiguration()
    .AddDependencyInjectionConfiguration()
    .AddAuthConfiguration()
    .AddControllersWithViews();

var app = builder.Build();

app.UseWebAppConfiguration();

await DataSeeder.SeedAsync(app.Services);

await app.RunAsync();
