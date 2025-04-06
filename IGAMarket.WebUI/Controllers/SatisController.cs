using AutoMapper;
using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DtoLayer.ProductDtos;
using IGAMarket.DtoLayer.SaleDtos;
using IGAMarket.EntityLayer.Concrete;
using IGAMarket.EntityLayer.Enums;
using IGAMarket.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IGAMarket.WebUI.Controllers;

[Authorize(Roles = "Admin,Member")]
public class SatisController : Controller
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly ISaleService _saleService;
    private readonly ISaleItemService _saleItemService;
    private readonly ISepetService _sepetService;

    public SatisController(IProductService productService, IMapper mapper, ISaleService saleService, ISaleItemService saleItemService, ISepetService sepetService)
    {
        _productService = productService;
        _mapper = mapper;
        _saleService = saleService;
        _saleItemService = saleItemService;
        _sepetService = sepetService;
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
    public IActionResult GetProductByBarcode(string barcode)
    {
        if (barcode.Length < 13)
        {
            return BadRequest("Geçersiz barkod numarası.");
        }

        var product = _productService.Get(x => x.Barcode == barcode && !x.IsDeleted); 

        if (product == null)
        {
            return NotFound(new { message = "Ürün bulunamadı!" });
        }

        var productDto = _mapper.Map<ResultProductDto>(product);

        return Json(productDto);
    }


    [HttpGet]
    public IActionResult getAllItem()
    {
        var product = _sepetService.GetAll();
        return Json(product);
    }

    [HttpPost]
    public IActionResult increaseItem([FromBody] string barcode)
    {
        if (barcode.Length < 13)
        {
            return BadRequest("Geçersiz barkod numarası.");
        }

       
        var data = _sepetService.Get(x => x.Barcode == barcode);

        if (data == null)
        {
            Sepet item = new Sepet();
            item.Barcode = barcode;
            item.Name = _productService.Get(x => x.Barcode == barcode && !x.IsDeleted).Name;
            item.Quantity = 1;
            item.Price = _productService.Get(x => x.Barcode == barcode && !x.IsDeleted).SalePrice;

            var product = _sepetService.IncreaseOrAdd(item);
            return Json(product);
        }
        else
        {
            var product = _sepetService.IncreaseOrAdd(data);
            return Json(product);
        }


    }

    [HttpPost]
    public IActionResult removeAllItem([FromBody] string barcode)
    {
        _sepetService.RemoveAll();
        var product = _sepetService.GetAll();
        return Json(product);
    }

    [HttpPost]
    public IActionResult decreaseItem([FromBody] string barcode)
    {
        var product = _sepetService.DecreaseOrDelete(barcode);
        return Json(product);
    }

    [HttpPost]
    public IActionResult deleteItem([FromBody] string barcode)
    {
        var product = _sepetService.DeleteThenGetList(barcode);
        return Json(product);
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
            Status = StatusEnum.Pending,
            SaleItems = new List<SaleItem>()
        };

        foreach (var item in saleDto.SaleItems)
        {
            var product = _productService.Get(x => x.Barcode == item.Barcode && !x.IsDeleted);

            // Ürün yoksa veya yanlış barkod geldiyse hata dön
            if (product == null)
            {
                return BadRequest($"Ürün bulunamadı! (Barkod: {item.Barcode})");
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
        _saleService.TInsert(sale);
        _sepetService.RemoveAll();
        return Ok(new { message = "Satış başarıyla tamamlandı." });

    }


}