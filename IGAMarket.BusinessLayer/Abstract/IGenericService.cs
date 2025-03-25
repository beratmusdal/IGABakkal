using System.Linq.Expressions;

namespace IGAMarket.BusinessLayer.Abstract
{
    public interface IGenericService <T> where T : class
    {
        void TInsert(T entity);
        void TDelete(T entity);
        void TUpdate(T entity);
        List<T> TGetList();
        T TGetById(int id);
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);

    }
}
