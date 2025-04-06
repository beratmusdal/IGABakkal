using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.EntityLayer.Concrete;
using System.Linq.Expressions;

namespace IGAMarket.BusinessLayer.Concrete
{
   public class SepetManager : ISepetService
    {
        private readonly ISepetDal _SepetDal;
        private readonly IProductDal _productDal;
        public SepetManager(ISepetDal SepetDal, IProductDal productDal)
        {
            _SepetDal = SepetDal;
            _productDal = productDal;
        }

        // Yeni bir satış kaydı ekleme
        public void TInsert(Sepet entity)
        {
            _SepetDal.Insert(entity);
        }

        // Satış kaydını güncelleme
        public void TUpdate(Sepet entity)
        {
            _SepetDal.Update(entity);
        }

        // Satış kaydını silme (soft delete işlemi)
        public void TDelete(Sepet entity)
        {
            _SepetDal.Delete(entity);
        }

        // ID ile bir satış kaydını almak
        public Sepet TGetById(int id)
        {
            return _SepetDal.Get(x => x.Price == id);
        }

        // Tüm satışları listeleme
        public List<Sepet> TGetList()
        {
            return _SepetDal.GetAll();
        }

        public List<Sepet> GetAll(Expression<Func<Sepet, bool>> filter = null)
        {
            return _SepetDal.GetAll(filter);
        }

        public Sepet Get(Expression<Func<Sepet, bool>> filter)
        {
            return _SepetDal.Get(filter);
        }

        public List<Sepet> IncreaseOrAdd(Sepet entity)
        {
            var item = _SepetDal.Get(x => x.Barcode == entity.Barcode);
            if (item != null)
            {
                var product = _productDal.Get(x => x.Barcode == entity.Barcode && !x.IsDeleted);
                if (product.StockQuantity > item.Quantity)
                {
                    item.Quantity += 1;
                    _SepetDal.Update(item);
                }
               
                return _SepetDal.GetAll();
            }
            _SepetDal.Insert(entity);
            return _SepetDal.GetAll();
        }

        public List<Sepet> DecreaseOrDelete(string barcode)
        {
            var item = _SepetDal.Get(x => x.Barcode == barcode);

            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity -= 1;
                    _SepetDal.Update(item);
                }
                else
                {
                    _SepetDal.Remove(item);
                }
            }
            return _SepetDal.GetAll();
        }

        public List<Sepet> DeleteThenGetList(string barcode)
        {
            var item = _SepetDal.Get(x => x.Barcode == barcode);
            if (item != null)
            {
                _SepetDal.Remove(item);
            }
            return _SepetDal.GetAll();
        }

        public void RemoveAll()
        {
            _SepetDal.RemoveAll();
        }
    }
}
