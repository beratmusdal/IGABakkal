using AutoMapper;
using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DtoLayer.ProductDtos;
using IGAMarket.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IGAMarket.WebUI.Controllers
{
    public class SatisController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public SatisController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult SatisIndex(int enumType=0)
        {
            var productList = _productService.GetAll(x => x.IsDeleted == false);
            if (enumType != 0)
            {
                productList = _productService.GetAll(x => (int)x.Category == enumType && x.IsDeleted == false);
            }
            var productDtoList = _mapper.Map<List<ResultProductDto>>(productList); 

            var model = new ProductModelView
            {
                ResultProductDto = productDtoList,
                EnumType = enumType
            };

            return View(model);
        }



        [HttpGet]
        public IActionResult GetProductByBarcode(long barcode)
        {
            if (barcode <= 0)
            {
                return BadRequest("Geçersiz barkod numarası.");
            }

            var product = _productService.TGetList()
                .FirstOrDefault(p => p.Barcode == barcode);

            if (product == null)
            {
                return NotFound(new { message = "Ürün bulunamadı!" });
            }

            var productDto = _mapper.Map<ResultProductDto>(product);

            return Json(productDto); 
        }

        [HttpGet]
        public IActionResult GetProductsAndCategories()
        {
            var products = _productService.TGetList();
            var productDtoList = _mapper.Map<List<ResultProductDto>>(products);

            var categories = _productService.TGetList().Select(x => x.Category).Distinct().ToList();


            return Json(new
            {
                products = productDtoList,
                categories = categories
            });
        }



    }
}
