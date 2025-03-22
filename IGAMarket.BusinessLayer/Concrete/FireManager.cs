using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.BusinessLayer.Concrete
{
    public class FireManager : IFireService
    {
        private readonly IFireDal _FireDal;
        public FireManager(IFireDal FireDal)
        {
            _FireDal = FireDal;
        }

        // Yeni bir Ürünler kaydı ekleme
        public void TInsert(Fire entity)
        {
            _FireDal.Insert(entity);
        }

        // Ürünler kaydını güncelleme
        public void TUpdate(Fire entity)
        {
            _FireDal.Update(entity);
        }

        // Ürünler kaydını silme (soft delete işlemi)
        public void TDelete(Fire entity)
        {
            _FireDal.Delete(entity);
        }

        // ID ile bir Ürünler kaydını almak
        public Fire TGetById(int id)
        {
            return _FireDal.GetById(id);
        }

        // Tüm Ürünlerları listeleme
        public List<Fire> TGetList()
        {
            return _FireDal.GetList();
        }
    }
}
