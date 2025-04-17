using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.DataAccessLayer.EntityFramework;
using IGAMarket.DtoLayer.MonthlyDtos;
using IGAMarket.EntityLayer.Concrete;
using System.Linq.Expressions;

namespace IGAMarket.BusinessLayer.Concrete
{
    public class DailyClosurManager : IDailyClosurService
    {
        private readonly IDailyClosurDal _dailyClosurDal;
        private readonly ISaleDal _saleDal;

        public DailyClosurManager(IDailyClosurDal dailyClosurDal, ISaleDal saleDal)
        {
            _dailyClosurDal = dailyClosurDal;
            _saleDal = saleDal;
        }

        public void TInsert(DailyClosur entity)
        {
            _dailyClosurDal.Insert(entity);
        }

        public void TUpdate(DailyClosur entity)
        {
            _dailyClosurDal.Update(entity);
        }

        public void TDelete(DailyClosur entity)
        {
            _dailyClosurDal.Delete(entity);
        }

        public DailyClosur TGetById(int id)
        {
            return _dailyClosurDal.Get(x => x.Id == id);
        }

        public List<DailyClosur> TGetList()
        {
            return _dailyClosurDal.GetAll();
        }

        public List<DailyClosur> GetAll(Expression<Func<DailyClosur, bool>> filter = null)
        {
            return _dailyClosurDal.GetAll(filter);
        }

        public DailyClosur Get(Expression<Func<DailyClosur, bool>> filter)
        {
            return _dailyClosurDal.Get(filter);
        }

        public List<DailyClosur> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return GetAll(x => x.CreateDate >= startDate && x.CreateDate <= endDate && x.IsDeleted == false);
        }



    }
}
