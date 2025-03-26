using AutoMapper;
using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DtoLayer.ProductDtos;
using IGAMarket.DtoLayer.SaleDtos;
using IGAMarket.EntityLayer.Concrete;
using IGAMarket.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IGAMarket.WebUI.Controllers;

public class SatisController : Controller
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly ISaleService _saleService;

    public SatisController(IProductService productService, IMapper mapper, ISaleService saleService)
    {
        _productService = productService;
        _mapper = mapper;
        _saleService = saleService;
    }

    [HttpGet]
    public IActionResult SatisIndex(int enumType = 0)
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

        var product = _productService.GetByBarcode(barcode); // GetByBarcode kullanılıyor

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

    [HttpPost]
    public IActionResult CompleteSale([FromBody] SaleDto saleDto)
    {
        if (saleDto == null || saleDto.SaleItems == null || !saleDto.SaleItems.Any())
        {
            return BadRequest("Geçersiz satış verisi. Lütfen ürün ekleyin.");
        }

        var sale = new Sale
        {
            TotalAmount = saleDto.TotalAmount,
            SaleItems = new List<SaleItem>()
        };

        foreach(var item in saleDto.SaleItems)
        {
            var product = _productService.GetByBarcode(item.Id);

            // Ürün yoksa veya yanlış barkod geldiyse hata dön
            if (product == null)
            {
                return BadRequest($"Ürün bulunamadı! (Barkod: {item.Id})");
            }

            // Stok yeterli mi kontrol et
            if (product.StockQuantity < item.Quantity)
            {
                return BadRequest($"Stok yetersiz! ({product.Name} - Stok: {product.StockQuantity}, İstenen: {item.Quantity})");
            }

            // Satış için ürünü ekle
            sale.SaleItems.Add(new SaleItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            });

            // Stok güncelleme
            product.StockQuantity -= item.Quantity;
            _productService.TUpdate(product);
        }


        // Satışı veritabanına kaydet
        _saleService.TInsert(sale);

        return Ok(new { message = "Satış başarıyla tamamlandı." });
    }


}