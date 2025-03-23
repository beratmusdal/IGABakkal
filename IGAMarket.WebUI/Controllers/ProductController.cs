using AutoMapper;
using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DtoLayer.ProductDtos;
using IGAMarket.EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace IGAMarket.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult CreateProductIndex()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProductIndex(AddProductDto AddProductDto)
        {
            _productService.TInsert(_mapper.Map<Product>(AddProductDto));
            return View();
        }
        //[HttpPost]
        //public IActionResult CreateProductIndex(AddProductDto addProductDto)
        //{

        //    var product = new Product
        //    {
        //        Name = addProductDto.Name,
        //        Barcode = addProductDto.Barcode,
        //        PurchasePrice = addProductDto.PurchasePrice,
        //        SalePrice = addProductDto.SalePrice,
        //        StockQuantity = addProductDto.StockQuantity,
        //        ImageUrl = addProductDto.ImageUrl
        //    };
        //    _productService.TInsert(product);
        //    return View();
        //}

    }
}
