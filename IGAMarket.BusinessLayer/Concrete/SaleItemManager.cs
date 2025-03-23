using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.EntityLayer.Concrete;

namespace IGAMarket.BusinessLayer.Concrete
{
    public class SaleItemManager : ISaleItemService
    {
        private readonly ISaleItemDal _SaleItemDal;
        public SaleItemManager(ISaleItemDal SaleItemDal)
        {
            _SaleItemDal = SaleItemDal;
        }

        // Yeni bir Satış Kalemleri kaydı ekleme
        public void TInsert(SaleItem entity)
        {
            _SaleItemDal.Insert(entity);
        }

        // Satış Kalemleri kaydını güncelleme
        public void TUpdate(SaleItem entity)
        {
            _SaleItemDal.Update(entity);
        }

        // Satış Kalemleri kaydını silme (soft delete işlemi)
        public void TDelete(SaleItem entity)
        {
            _SaleItemDal.Delete(entity);
        }

        // ID ile bir Satış Kalemleri kaydını almak
        public SaleItem TGetById(int id)
        {
            return _SaleItemDal.Get(x => x.Id == id);
        }

        // Tüm Satış Kalemleriları listeleme
        public List<SaleItem> TGetList()
        {
            return _SaleItemDal.GetAll();
        }
    }
}
