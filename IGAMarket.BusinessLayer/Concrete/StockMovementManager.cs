using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IGAMarket.BusinessLayer.Concrete
{
    public class StockMovementManager : IStockMovementService
    {
        private readonly IStockMovementDal _stockMovementDal;

        public StockMovementManager(IStockMovementDal stockMovementDal)
        {
            _stockMovementDal = stockMovementDal;
        }

        public void TInsert(StockMovement entity)
        {
            _stockMovementDal.Insert(entity);
        }

        public void TDelete(StockMovement entity)
        {
            _stockMovementDal.Delete(entity);
        }

        public void TUpdate(StockMovement entity)
        {
            _stockMovementDal.Update(entity);
        }

        public List<StockMovement> TGetList()
        {
            return _stockMovementDal.GetAll();
        }

        public StockMovement TGetById(int id)
        {
            return _stockMovementDal.Get(x => x.Id == id);
        }

        public List<StockMovement> GetAll(Expression<Func<StockMovement, bool>> filter = null)
        {
            return _stockMovementDal.GetAll(filter);
        }

        public StockMovement Get(Expression<Func<StockMovement, bool>> filter)
        {
            return _stockMovementDal.Get(filter);
        }
    }
}
