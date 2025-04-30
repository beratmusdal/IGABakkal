using IGAMarket.DataAccessLayer.Concrete;
using IGAMarket.EntityLayer.Concrete;
using IGAMarket.WebUI.Extensions;
using IGAMarket.WebUI.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

// Cookie Ayarları
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/LoginIndex";
    options.AccessDeniedPath = "/Login/LoginIndex";
    options.SlidingExpiration = true;
});

// Dependency Injection (custom servislerin)
builder.Services.AddProjectDependencies();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// MVC & Authorization
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter());
}).AddRazorRuntimeCompilation();

var app = builder.Build();

// Error Handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // Geliştirme ortamında detaylı hata
}

// Middleware Pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
