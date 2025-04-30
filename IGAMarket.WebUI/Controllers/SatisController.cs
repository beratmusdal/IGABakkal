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
    private readonly IStockMovementService _stockMovementService;

    public SatisController(IProductService productService, IMapper mapper, ISaleService saleService, ISaleItemService saleItemService, ISepetService sepetService, IStockMovementService stockMovementService)
    {
        _productService = productService;
        _mapper = mapper;
        _saleService = saleService;
        _saleItemService = saleItemService;
        _sepetService = sepetService;
        _stockMovementService = stockMovementService;
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
        //if (barcode.Length < 13)
        //{
        //    return BadRequest("Geçersiz barkod numarası.");
        //}

        var existingItem = _sepetService.Get(x => x.Barcode == barcode);
        var product = _productService.Get(x => x.Barcode == barcode && !x.IsDeleted);

        if (product == null)
        {
            return NotFound("Ürün bulunamadı.");
        }

        if (existingItem == null && product.StockQuantity < 1)
        {
            return BadRequest($"{product.Name} ürünü stokta bulunmamaktadır.");
        }

        if (existingItem != null && existingItem.Quantity >= product.StockQuantity)
        {
            return BadRequest($"{product.Name} için yeterli stok yok. Mevcut stok: {product.StockQuantity}");
        }

        if (existingItem == null)
        {
            Sepet item = new Sepet
            {
                Barcode = barcode,
                Name = product.Name,
                Quantity = 1,
                Price = product.SalePrice
            };

            var result = _sepetService.IncreaseOrAdd(item);
            return Json(result);
        }
        else
        {
            var result = _sepetService.IncreaseOrAdd(existingItem);
            return Json(result);
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
    public IActionResult Gift([FromBody] string barcode)
    {
        var sepetItem = _sepetService.Get(x => x.Barcode == barcode);

        if (sepetItem == null)
        {
            return NotFound(new { success = false, message = "Ürün sepette bulunamadı." });
        }

        sepetItem.Price = 0;
        _sepetService.TUpdate(sepetItem);

        return Ok(new { success = true, message = "Ürün fiyatı başarıyla 0 TL olarak güncellendi." });
    }

    [HttpPost]
    public IActionResult CompleteSale([FromBody] SaleDto saleDto)
    {
        if (saleDto == null || saleDto.SaleItems == null || !saleDto.SaleItems.Any())
        {
            return BadRequest("Geçersiz satış verisi. Lütfen ürün ekleyin.");
        }

        var cashierName = User.Identity.Name;

        var sale = new Sale
        {
            TotalAmount = saleDto.TotalAmount,
            Status = StatusEnum.Pending,
            CashierName = cashierName,
            SaleItems = new List<SaleItem>()
        };

        foreach (var item in saleDto.SaleItems)
        {
            var product = _productService.Get(x => x.Barcode == item.Barcode && !x.IsDeleted);

            if (product == null)
            {
                return BadRequest($"Ürün bulunamadı! (Barkod: {item.Barcode})");
            }

            if (product.StockQuantity < item.Quantity)
            {
                return BadRequest($"Stok yetersiz! ({product.Name} - Stok: {product.StockQuantity}, İstenen: {item.Quantity})");
            }

            if (item.UnitPrice == 0)
            {
                item.UnitPrice = 0;
            }

            sale.SaleItems.Add(new SaleItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            });

            // 1. Stoktan düşüyoruz
            product.StockQuantity -= item.Quantity;
            _productService.TUpdate(product);

            // 2. Stok hareketi kaydediyoruz
            var stockMovement = new StockMovement
            {
                ProductId = product.Id,
                Quantity = -item.Quantity, // satış olduğu için negatif
                MovementDate = DateTime.Now,
                MovementType = "Satış"
            };
            _stockMovementService.TInsert(stockMovement);
        }

        _saleService.TInsert(sale);
        _sepetService.RemoveAll();

        return Ok(new { message = "Satış başarıyla tamamlandı." });
    }

    [HttpGet]
    public IActionResult StockMovementReport(DateTime startDate, DateTime endDate)
    {
        var products = _productService.GetAll(x => !x.IsDeleted);
        var movements = _stockMovementService.GetAll();

        var report = products.Select(product => new
        {
            ProductName = product.Name,

            Devir = movements
                .Where(m => m.ProductId == product.Id && m.MovementDate < startDate)
                .Sum(m => m.Quantity),

            Giris = movements
                .Where(m => m.ProductId == product.Id && m.MovementDate >= startDate && m.MovementDate <= endDate && m.Quantity > 0)
                .Sum(m => m.Quantity),

            Cikis = movements
                .Where(m => m.ProductId == product.Id && m.MovementDate >= startDate && m.MovementDate <= endDate && m.Quantity < 0)
                .Sum(m => Math.Abs(m.Quantity)),

            Bitis = movements
                .Where(m => m.ProductId == product.Id && m.MovementDate <= endDate)
                .Sum(m => m.Quantity)
        }).ToList();

        return Json(report);
    }
    [HttpGet]
    public IActionResult StockMovementReportView()
    {
        return View();
    }


}
