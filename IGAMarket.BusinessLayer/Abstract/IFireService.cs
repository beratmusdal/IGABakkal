using IGAMarket.DtoLayer.FireDtos;
using IGAMarket.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.BusinessLayer.Abstract
{
    public interface IFireService : IGenericService<Fire>
    {
        List<SelectFireDto> GetDetailList();
        void FireInsert(AddFireDto model);
        void UpdateById(long id);
    }
}
