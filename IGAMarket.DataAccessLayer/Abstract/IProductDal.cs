using IGAMarket.EntityLayer.Concrete;

namespace IGAMarket.DataAccessLayer.Abstract
{
    public interface IProductDal : IGenericDal<Product>
    {
        void UpdateProductStockQuantity(long id, int newStock);
    }
}
