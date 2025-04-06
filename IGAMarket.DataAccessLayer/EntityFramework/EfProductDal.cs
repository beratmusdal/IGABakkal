using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.DataAccessLayer.Concrete;
using IGAMarket.DataAccessLayer.Repositories;
using IGAMarket.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DataAccessLayer.EntityFramework
{
    public class EfProductDal : GenericRepository<Product>, IProductDal
    {
        private readonly Context _context; 

        public EfProductDal(Context context) : base(context)
        {
            _context = context; 
        }

        public void UpdateProductStockQuantity(long id, int newStock)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                product.StockQuantity = newStock;
                _context.SaveChanges();
            }
        }
    }
}