using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.EntityLayer.Concrete;

namespace IGAMarket.BusinessLayer.Concrete
{
    public class DailyClosurManager : IDailyClosurService
    {
        private readonly IDailyClosurDal _dailyClosurDal;

        public DailyClosurManager(IDailyClosurDal dailyClosurDal)
        {
            _dailyClosurDal = dailyClosurDal;
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
            return _dailyClosurDal.GetById(id);
        }

        public List<DailyClosur> TGetList()
        {
            return _dailyClosurDal.GetList();
        }
    }
}
