using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.EntityLayer.Concrete;

namespace IGAMarket.BusinessLayer.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _ProductDal;
        public ProductManager(IProductDal ProductDal)
        {
            _ProductDal = ProductDal;
        }

        // Yeni bir Ürünler kaydı ekleme
        public void TInsert(Product entity)
        {
            _ProductDal.Insert(entity);
        }

        // Ürünler kaydını güncelleme
        public void TUpdate(Product entity)
        {
            _ProductDal.Update(entity);
        }

        // Ürünler kaydını silme (soft delete işlemi)
        public void TDelete(Product entity)
        {
            _ProductDal.Delete(entity);
        }

        // ID ile bir Ürünler kaydını almak
        public Product TGetById(int id)
        {
            return _ProductDal.GetById(id);
        }

        // Tüm Ürünlerları listeleme
        public List<Product> TGetList()
        {
            return _ProductDal.GetList();
        }
    }
}
