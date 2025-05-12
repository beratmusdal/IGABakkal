using IGAMarket.DtoLayer.FireDtos;
using IGAMarket.DtoLayer.MonthlyDtos;
using IGAMarket.EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IGAMarket.BusinessLayer.Abstract
{
    public interface IDailyClosurService : IGenericService<DailyClosur>
    {
        List<DailyClosur> GetByDateRange(DateTime startDate, DateTime endDate);   

    }
}
