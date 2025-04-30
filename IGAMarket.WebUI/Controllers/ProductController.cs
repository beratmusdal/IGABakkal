using AutoMapper;
using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DtoLayer.ProductDtos;
using IGAMarket.EntityLayer.Concrete;
using IGAMarket.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
        public IActionResult CreateProductIndex(UpdateProductDto addProductDto)
        {
            var existingProduct = _productService.Get(x => x.Id == addProductDto.Id);

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
            else
            {
                addProductDto.ImageUrl = existingProduct?.ImageUrl ?? "/images/default.png";
            }

            addProductDto.PurchasePrice = addProductDto.PurchasePrice.Replace(".", ",").Trim();
            addProductDto.SalePrice = addProductDto.SalePrice.Replace(".", ",").Trim();

            _productService.AddProduct(_mapper.Map<Product>(addProductDto));

            var productList = _productService.TGetList();
            var productDtoList = _mapper.Map<List<ResultProductDto>>(productList);

            var model = new ProductModelView
            {
                ResultProductDto = productDtoList
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteProduct(int id)
        {
            try
            {
                _productService.UpdateById(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Silme işlemi başarısız! " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult UpdateProductIndex(long id)
        {
            var product = _productService.Get(x => x.Id == id);
            var productDto = _mapper.Map<UpdateProductDto>(product);
            var productList = _productService.TGetList();
            var productDtoList = _mapper.Map<List<ResultProductDto>>(productList);

            var model = new ProductModelView
            {
                UpdateProductDto = productDto,
                ResultProductDto = productDtoList
            };

            return View("CreateProductIndex", model);
        }
    }
}
