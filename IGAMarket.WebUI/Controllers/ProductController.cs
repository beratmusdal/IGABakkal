using AutoMapper;
using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DtoLayer.ProductDtos;
using IGAMarket.EntityLayer.Concrete;
using IGAMarket.WebUI.Models;
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
            var productList = _productService.TGetList();
            var productDtoList = _mapper.Map<List<ResultProductDto>>(productList);

            var model = new ProductModelView
            {
                ResultProductDto = productDtoList
            };

            return View(model);

        }

        [HttpPost]
        public IActionResult CreateProductIndex(AddProductDto addProductDto)
        {
            if (addProductDto.ImageFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(addProductDto.ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    addProductDto.ImageFile.CopyTo(stream);
                }

                addProductDto.ImageUrl = "/images/" + fileName;
            }

            _productService.TInsert(_mapper.Map<Product>(addProductDto));

            // Ürün listesini güncelleme ve view için model oluşturma
            var productList = _productService.TGetList();
            var productDtoList = _mapper.Map<List<ResultProductDto>>(productList);

            var model = new ProductModelView
            {
                ResultProductDto = productDtoList
            };

            // Modeli View'a gönderme
            return View(model);
        }




        [HttpPost]
        public JsonResult DeleteProduct(int id)
        {
            try
            {
                _productService.UpdateById(id); // Silme işlemi
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Silme işlemi başarısız! " + ex.Message });
            }
        }
    }
}
