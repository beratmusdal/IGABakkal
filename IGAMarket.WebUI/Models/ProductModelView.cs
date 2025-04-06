using IGAMarket.DtoLayer.ProductDtos;

namespace IGAMarket.WebUI.Models
{
    public class ProductModelView
    {
        public AddProductDto AddProductDto { get; set; }
        public UpdatePorductDto UpdateProductDto { get; set; }
        public List<ResultProductDto> ResultProductDto { get; set; }
        public int EnumType { get; set; }

    }
}
