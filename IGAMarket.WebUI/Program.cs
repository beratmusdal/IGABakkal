using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.BusinessLayer.Concrete;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.DataAccessLayer.Concrete;
using IGAMarket.DataAccessLayer.EntityFramework;
using IGAMarket.EntityLayer.Concrete;
using IGAMarket.WebUI.Mapping;
using Microsoft.AspNetCore.Identity;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Context>();

builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<IProductDal, EfProductDal>();

builder.Services.AddScoped<IFireService, FireManager>();
builder.Services.AddScoped<IFireDal, EfFireDal>();

builder.Services.AddScoped<ISaleService, SaleManager>();
builder.Services.AddScoped<ISaleDal, EfSaleDal>();

builder.Services.AddScoped<ISaleItemService, SaleItemManager>();
builder.Services.AddScoped<ISaleItemDal, EfSaleItemDal>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

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
app.UseAuthentication();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
