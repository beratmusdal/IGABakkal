using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.EntityLayer.Concrete;

namespace IGAMarket.BusinessLayer.Concrete
{
    public class SaleManager : ISaleService
    {
        private readonly ISaleDal _saleDal;
        public SaleManager(ISaleDal saleDal)
        {
            _saleDal = saleDal;
        }

        // Yeni bir satış kaydı ekleme
        public void TInsert(Sale entity)
        {
            _saleDal.Insert(entity); 
        }

        // Satış kaydını güncelleme
        public void TUpdate(Sale entity)
        {
            _saleDal.Update(entity); 
        }

        // Satış kaydını silme (soft delete işlemi)
        public void TDelete(Sale entity)
        {
            _saleDal.Delete(entity);
        }

        // ID ile bir satış kaydını almak
        public Sale TGetById(int id)
        {
            return _saleDal.GetById(id);
        }

        // Tüm satışları listeleme
        public List<Sale> TGetList()
        {
            return _saleDal.GetList();
        }
    }
}
