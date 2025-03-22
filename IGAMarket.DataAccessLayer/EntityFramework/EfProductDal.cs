using IGAMarket.DataAccessLayer.Abstract;
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
        public EfProductDal(DbContext context) : base(context)
        {
        }
    }
}
