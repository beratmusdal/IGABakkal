using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.BusinessLayer.Concrete;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.DataAccessLayer.Concrete;
using IGAMarket.DataAccessLayer.EntityFramework;
using IGAMarket.WebUI.Mapping;
using Microsoft.EntityFrameworkCore;

namespace IGAMarket.WebUI.Registiration

{
    public static class ServiceRegistiration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<Context>(options =>
                 options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
             );
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IProductDal, EfProductDal>();
            services.AddScoped<ISaleService, SaleManager>();
            services.AddScoped<ISaleDal, EfSaleDal>();
            services.AddScoped<ISaleItemService, SaleItemManager>();
            services.AddScoped<ISaleItemDal, EfSaleItemDal>();
            services.AddScoped<IFireService, FireManager>();
            services.AddScoped<IFireDal, EfFireDal>();
            services.AddScoped<IDailyClosurService, DailyClosurManager>();
            services.AddScoped<IDailyClosurDal, EfDailyClosurDal>();

            return services;
        }
    }
}
