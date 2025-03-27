using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.DtoLayer.FireDtos;
using IGAMarket.EntityLayer.Concrete;
using System.Linq.Expressions;

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
            return _ProductDal.Get(x=>x.Id==id);
        }

        // Tüm Ürünlerları listeleme
        public List<Product> TGetList()
        {
            return _ProductDal.GetAll(x => !x.IsDeleted);
        }

        public void UpdateById(long id)
        {
            var data = _ProductDal.Get(x => x.Id == id);
            data.IsDeleted = true;
            _ProductDal.Update(data);
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            return _ProductDal.GetAll(filter);
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            return _ProductDal.Get(filter);
        }

      

    }
}
