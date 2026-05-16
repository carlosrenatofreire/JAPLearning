using Doppler.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MundoDev.Data.Contexts;
using MundoDev.Mvc.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Doppler — fonte de configuração centralizada
builder.Configuration.AddDoppler(options =>
{
    options.ServiceToken = "dp.st.dev_carlos.VJxtuLmTSW0akkPfes2Wc95GceAk4Z5iDvS9yejGVWE";
    options.Project = "mundodev";
    options.Config = builder.Environment.IsProduction() ? "prd" : "dev_carlos";
});

var root = (IConfigurationRoot)builder.Configuration;
root.Reload();

var connectionString = builder.Configuration["CONNECTIONSTRINGS"];

// DbContext
builder.Services.AddDbContext<MainDbContext>(options =>
    options.UseSqlServer(connectionString));

// AutoMapper
builder.Services.AddAutoMapper(typeof(DomainProfile));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
