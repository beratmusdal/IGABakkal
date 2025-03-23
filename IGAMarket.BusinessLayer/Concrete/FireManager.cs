using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.DtoLayer.FireDtos;
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
        private readonly IProductDal _productDal;
        public FireManager(IFireDal FireDal, IProductDal productDal)
        {
            _FireDal = FireDal;
            _productDal = productDal;
        }

        // Yeni bir Ürünler kaydı ekleme
        public void TInsert(Fire entity)
        {
            var product = _productDal.Get(x=>x.Barcode == entity.Barcode);
            entity.PurchasePrice = product.PurchasePrice;
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
            return _FireDal.Get(x => x.Id == id);
        }

        // Tüm Ürünlerları listeleme
        public List<Fire> TGetList()
        {
            return _FireDal.GetAll();
        }

        public List<SelectFireDto> GetDetailList()
        {
            List<SelectFireDto> selectFireDtos = new List<SelectFireDto>();
            var listFire = _FireDal.GetAll(x=>x.IsDeleted==false);
           
            foreach (var item in listFire)
            {
                var data = _productDal.Get(x=>x.Barcode==item.Barcode && x.IsDeleted == false);
                if (data != null)
                {
                    SelectFireDto selectFireDto = new SelectFireDto();
                    selectFireDto.Id = item.Id;
                    selectFireDto.Barcode = item.Barcode;
                    selectFireDto.Name = data.Name;
                    selectFireDto.Quantity = item.Quantity;
                    selectFireDto.TotalPurchasePrice = item.PurchasePrice * item.Quantity;
                    selectFireDto.CreateDate = item.CreateDate;
                    selectFireDto.Reason = item.Reason;

                    selectFireDtos.Add(selectFireDto);
                }
            }
            return selectFireDtos;
        }

        public void UpdateById(long id)
        {
            var data = _FireDal.Get(x => x.Id == id);
            data.IsDeleted = true;
            _FireDal.Update(data);  
        }
    }
}
