using IGAMarket.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.BusinessLayer.Abstract
{
    public interface IProductService : IGenericService<Product>
    {
        void UpdateById(long id);  
    }
}
