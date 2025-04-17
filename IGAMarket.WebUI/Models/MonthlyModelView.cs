using IGAMarket.DtoLayer.FireDtos;
using IGAMarket.DtoLayer.MonthlyDtos;

namespace IGAMarket.WebUI.Models
{
    public class MonthlyModelView
    {
        public AddMonthlyDto AddFireDto { get; set; }  
        public List<SelectMonthlyDto> ListMonthlyDto { get; set; }  
        public List<MonthlyDto> ListOfMonthlyClosures { get; set; }  
    }
}
