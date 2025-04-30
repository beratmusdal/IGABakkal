using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.BusinessLayer.Concrete;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.DataAccessLayer.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace IGAMarket.WebUI.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IProductDal, EfProductDal>();

            services.AddScoped<IFireService, FireManager>();
            services.AddScoped<IFireDal, EfFireDal>();

            services.AddScoped<ISaleService, SaleManager>();
            services.AddScoped<ISaleDal, EfSaleDal>();

            services.AddScoped<ISaleItemService, SaleItemManager>();
            services.AddScoped<ISaleItemDal, EfSaleItemDal>();

            services.AddScoped<IDailyClosurService, DailyClosurManager>();
            services.AddScoped<IDailyClosurDal, EfDailyClosurDal>();

            services.AddScoped<ISepetService, SepetManager>();
            services.AddScoped<ISepetDal, EfSepetDal>();

            services.AddScoped<IStockMovementService, StockMovementManager>();
            services.AddScoped<IStockMovementDal, EfStockMovementDal>();

            return services;
        }
    }
}
