using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.DataAccessLayer.Concrete;
using IGAMarket.DataAccessLayer.Repositories;
using IGAMarket.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DataAccessLayer.EntityFramework;

public class EfSepetDal : GenericRepository<Sepet>, ISepetDal
{
    public EfSepetDal(Context context) : base(context)
    {
    }
}
