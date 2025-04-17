using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.BusinessLayer.Concrete;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.DataAccessLayer.Concrete;
using IGAMarket.DataAccessLayer.EntityFramework;
using IGAMarket.EntityLayer.Concrete;
using IGAMarket.WebUI.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/LoginIndex"; // Login yapılmadan erişilirse buraya yönlendirme
    options.AccessDeniedPath = "/Login/LoginIndex"; // Erişim izni olmayanlar bu sayfaya yönlendirilecek
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter()); 
}).AddRazorRuntimeCompilation();

// Dependency Injection
builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<IProductDal, EfProductDal>();

builder.Services.AddScoped<IFireService, FireManager>();
builder.Services.AddScoped<IFireDal, EfFireDal>();

builder.Services.AddScoped<ISaleService, SaleManager>();
builder.Services.AddScoped<ISaleDal, EfSaleDal>();

builder.Services.AddScoped<ISaleItemService, SaleItemManager>();
builder.Services.AddScoped<ISaleItemDal, EfSaleItemDal>();

builder.Services.AddScoped<IDailyClosurService, DailyClosurManager>();
builder.Services.AddScoped<IDailyClosurDal, EfDailyClosurDal>();

builder.Services.AddScoped<ISepetService, SepetManager>();
builder.Services.AddScoped<ISepetDal, EfSepetDal>();




// AutoMapper registration
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/LoginIndex"; // Login yapılmadan erişilirse buraya yönlendirme
    options.AccessDeniedPath = "/Login/LoginIndex"; // Erişim izni olmayanlar bu sayfaya yönlendirilecek
});

var app = builder.Build();


// Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication(); 
app.UseAuthorization();
//builder.WebHost.UseUrls("http://0.0.0.0:5050");


// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
