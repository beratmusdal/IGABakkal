using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.DataAccessLayer.Repositories;
using IGAMarket.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace IGAMarket.DataAccessLayer.EntityFramework
{
    public class EfDailyClosurDal : GenericRepository<DailyClosur>, IDailyClosurDal
    {
        public EfDailyClosurDal(DbContext context) : base(context)
        {
        }
    }
}
