using IGAMarket.DtoLayer.FireDtos;
using IGAMarket.EntityLayer.Concrete;

namespace IGAMarket.WebUI.Models
{
    public class FireModelView
    {
        public AddFireDto AddFireDto { get; set; }
        public List<SelectFireDto> ListFireDto { get; set; }
    }
}
