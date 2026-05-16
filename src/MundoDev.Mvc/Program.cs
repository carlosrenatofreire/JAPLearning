using MundoDev.Mvc.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddDopplerConfiguration();

builder.Services
    .AddContextConfiguration(builder.Configuration)
    .AddAutoMapperConfiguration()
    .AddDependencyInjectionConfiguration()
    .AddFluentValidationConfiguration()
    .AddControllersWithViews();

var app = builder.Build();

app.UseWebAppConfiguration();

app.Run();
