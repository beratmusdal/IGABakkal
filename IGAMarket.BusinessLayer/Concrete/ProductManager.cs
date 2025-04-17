using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.DataAccessLayer.EntityFramework;
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

        public void UpdateProductStockQuantity(long id, int newStock)
        {
            _ProductDal.UpdateProductStockQuantity(id, newStock);
        }

        public void AddProduct(Product product)
        {
            var data = _ProductDal.Get(x => x.Barcode == product.Barcode && !x.IsDeleted);
            if (data==null)
            {
                _ProductDal.Insert(product);
                return;
            }
            data.PurchasePrice = product.PurchasePrice;
            data.SalePrice = product.SalePrice;
            data.StockQuantity = product.StockQuantity;
            data.Category = product.Category;
            data.Name = product.Name;
            data.ImageUrl = product.ImageUrl;
            data.CreateDate = product.CreateDate;
            _ProductDal.Update(data);
        }
    }
}
