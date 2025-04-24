using AutoMapper;
using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DtoLayer.FireDtos;
using IGAMarket.DtoLayer.ProductDtos;
using IGAMarket.EntityLayer.Concrete;
using IGAMarket.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IGAMarket.WebUI.Controllers
{
    public class FireController : Controller
    {
        private readonly IFireService _fireService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public FireController(IFireService fireService, IProductService productService, IMapper mapper)
        {
            _fireService = fireService;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult CreateFireIndex()
        {
            var model = new FireModelView
            {
                ListFireDto = _fireService.GetDetailList(),
                ProductList = _productService.GetAll()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateFireIndex(AddFireDto addFireDto)
        {
            try
            {
                var fireEntity = _mapper.Map<Fire>(addFireDto);
                _fireService.TInsert(fireEntity);

                addFireDto.TotalPrice = addFireDto.Quantity * addFireDto.PurchasePrice;

                var product = _productService.Get(x => x.Barcode == addFireDto.Barcode && !x.IsDeleted);
                if (product == null)
                    return BadRequest($"Ürün bulunamadı! (Barkod: {addFireDto.Barcode})");

                if (product.StockQuantity < addFireDto.Quantity)
                    return BadRequest($"Stok yetersiz! ({product.Name} - Stok: {product.StockQuantity}, İstenen: {addFireDto.Quantity})");

                product.StockQuantity -= addFireDto.Quantity;
                _productService.TUpdate(product);

                var model = new FireModelView
                {
                    ListFireDto = _fireService.GetDetailList(),
                    ProductList = _productService.GetAll()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir hata oluştu: {ex.Message}");
            }
        }

        [HttpPost]
        public JsonResult DeleteFire(int id)
        {
            try
            {
                var fire = _fireService.Get(x => x.Id == id);

                if (fire == null)
                {
                    return Json(new { success = false, message = "Böyle bir hibe kaydı bulunamadı!" });
                }

                if (fire.IsDeleted)
                {
                    return Json(new { success = false, message = "Bu hibe kaydı zaten silinmiş." });
                }

                fire.IsDeleted = true;
                _fireService.TUpdate(fire);

                var product = _productService.Get(x => x.Barcode == fire.Barcode && !x.IsDeleted);

                if (product == null)
                {
                    return Json(new { success = false, message = "Ürün bulunamadı!" });
                }

                product.StockQuantity += fire.Quantity;
                _productService.TUpdate(product);

                return Json(new { success = true, message = "Hibe kaydı başarıyla silindi ve stok geri yüklendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Silme işlemi başarısız! " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ExportMonthlyFiresToExcel(string month)
        {
            if (!DateTime.TryParseExact(month, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out DateTime parsedMonth))
                return BadRequest("Geçersiz tarih");

            var startDate = parsedMonth;
            var endDate = parsedMonth.AddMonths(1).AddDays(-1);

            var fireList = _fireService.GetDetailList()
                .Where(x => x.CreateDate.Date >= startDate.Date && x.CreateDate.Date <= endDate.Date && !x.IsDeleted)
                .ToList();

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add($"{month} Hibe Raporu");

            // Başlıklar
            worksheet.Cell(1, 1).Value = "Tarih";
            worksheet.Cell(1, 2).Value = "Ürün Adı";
            worksheet.Cell(1, 3).Value = "Adet";
            worksheet.Cell(1, 4).Value = "Toplam Zarar (₺)";
            worksheet.Cell(1, 5).Value = "Açıklama";

            int row = 2;
            foreach (var fire in fireList)
            {
                worksheet.Cell(row, 1).Value = fire.CreateDate.ToString("dd.MM.yyyy");
                worksheet.Cell(row, 2).Value = fire.Name ?? "(Ürün Yok)";
                worksheet.Cell(row, 3).Value = fire.Quantity;
                worksheet.Cell(row, 4).Value = fire.TotalPurchasePrice.ToString("N2") + " ₺";
                worksheet.Cell(row, 5).Value = fire.Reason ?? "-";
                row++;
            }


            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"HibeRaporu_{month}.xlsx");
        }

    }
}
