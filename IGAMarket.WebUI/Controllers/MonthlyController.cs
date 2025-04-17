using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DataAccessLayer.Abstract;
using IGAMarket.DtoLayer.MonthlyDtos;
using IGAMarket.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Drawing;
using System.Drawing.Imaging;

[Authorize(Roles = "Admin,Member")]
public class MonthlyController : Controller
{
    private readonly IDailyClosurService _dailyClosur;
    private readonly ISaleDal _saleDal;
    private readonly ISaleItemDal _saleItemDal;
    private readonly IProductService _productService;

    public MonthlyController(IDailyClosurService dailyClosur, ISaleDal saleDal, ISaleItemDal saleItemDal, IProductService productService)
    {
        _dailyClosur = dailyClosur;
        _saleDal = saleDal;
        _saleItemDal = saleItemDal;
        _productService = productService;
    }

    public IActionResult Index()
    {
        MonthlyModelView model = new MonthlyModelView();
        model.ListOfMonthlyClosures = new List<MonthlyDto>();
        return View(model);
    }

    [HttpGet]
    public IActionResult GetMonthlyClosures(string month)
    {
        if (!DateTime.TryParseExact(month, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out DateTime parsedMonth))
        {
            return BadRequest("Geçersiz ay formatı");
        }

        var startDate = parsedMonth;
        var endDate = parsedMonth.AddMonths(1).AddDays(-1);

        var closures = _dailyClosur.GetByDateRange(startDate, endDate);
        var giftItems = _saleItemDal.GetAll(x =>
            x.CreateDate.Date >= startDate.Date &&
            x.CreateDate.Date <= endDate.Date &&
            x.UnitPrice == 0 &&
            !x.IsDeleted
        );

        var closureDtos = closures.Select(x => new MonthlyDto
        {
            CreateDate = x.CreateDate,
            ZNo = x.ZNo, 
            TotalAmount = x.TotalAmount,
            TotalRefund = x.TotalRefund,
            TotalFireQuantity = x.TotalFireQuantity,
            PersonelName = x.PersonelName,
            TotalGiftQuantity = giftItems
        .Where(g => g.CreateDate.Date == x.CreateDate.Date)
        .Sum(g => g.Quantity)
        }).ToList();


        return Json(closureDtos);
    }

    [HttpGet]
    public IActionResult GetDetailOfMonthlyClosing(DateTime date)
    {
        var start = date.Date;
        var end = date.Date.AddDays(1).AddTicks(-1);

        var sales = _saleDal.GetAll(x =>
            x.CreateDate >= start &&
            x.CreateDate <= end &&
            !x.IsDeleted
        );

        var result = sales.Select(x => new
        {
            id = x.Id,
            amount = x.TotalAmount,
            createDate = x.CreateDate.ToString("HH:mm:ss")
        }).ToList();

        return Json(result);
    }

    [HttpGet]
    public IActionResult GetSaleItems(int saleId)
    {
        var items = _saleItemDal.GetAll(x => x.SaleId == saleId && !x.IsDeleted);
        var products = _productService.TGetList();

        var result = items.Select(x => new
        {
            productName = products.FirstOrDefault(p => p.Id == x.ProductId)?.Name ?? "(Ürün Yok)",
            quantity = x.Quantity,
            unitPrice = x.UnitPrice,
            subtotal = x.Quantity * x.UnitPrice
        }).ToList();

        return Json(result);
    }

    [HttpPost]
    public IActionResult GenerateMonthlyPdf([FromBody] List<MonthlyDto> data)
    {
        if (data == null || !data.Any())
            return BadRequest("Veri alınamadı.");

        string root = Directory.GetCurrentDirectory();
        string templatePath = Path.Combine(root, "wwwroot", "templates", "White Simple Minimalist Business Invoice (1).png");
        string reportsFolder = Path.Combine(root, "wwwroot", "reports");

        if (!Directory.Exists(reportsFolder))
            Directory.CreateDirectory(reportsFolder);

        string outputImgPath = Path.Combine(reportsFolder, "monthly_filled.png");
        string outputPdfPath = Path.Combine(reportsFolder, "monthly_filled.pdf");

        using (var bmp = new Bitmap(templatePath))
        using (var g = Graphics.FromImage(bmp))
        {
            var font = new Font("Arial", 10);
            var brush = Brushes.Black;

            float y = 380;
            float rowHeight = 32;

            foreach (var item in data)
            {
                g.DrawString(item.CreateDate.ToString("dd.MM.yyyy"), font, brush, 30, y);             
                g.DrawString(item.ZNo ?? "-", font, brush, 255, y);                                     
                g.DrawString(item.TotalAmount.ToString("N0") + " ₺", font, brush, 435, y);           
                g.DrawString(item.TotalRefund.ToString("N0") + " ₺", font, brush, 650, y);           
                g.DrawString(item.TotalFireQuantity.ToString(), font, brush, 880, y);             
                g.DrawString(item.TotalGiftQuantity.ToString(), font, brush, 1055, y);                
                g.DrawString(item.PersonelName, font, brush, 1220, y);                                  
                y += rowHeight;
            }

            float summaryX = 1000;
            float summaryY = 1700;

            g.DrawString(data.Sum(x => x.TotalAmount).ToString("N0") + " ₺", font, brush, summaryX, summaryY + 50);
            g.DrawString(data.Sum(x => x.TotalRefund).ToString("N0") + " ₺", font, brush, summaryX, summaryY + 100);
            g.DrawString(data.Count.ToString(), font, brush, summaryX, summaryY + 150);
            g.DrawString(data.Sum(x => x.TotalFireQuantity).ToString(), font, brush, summaryX, summaryY + 200);
            g.DrawString(data.Sum(x => x.TotalGiftQuantity).ToString(), font, brush, summaryX, summaryY + 250);

            bmp.Save(outputImgPath, ImageFormat.Png);
        }

        var doc = new PdfDocument();
        var page = doc.AddPage();

        using (var gfx = XGraphics.FromPdfPage(page))
        {
            var img = XImage.FromFile(outputImgPath);
            page.Width = img.PixelWidth * 72 / img.HorizontalResolution;
            page.Height = img.PixelHeight * 72 / img.VerticalResolution;
            gfx.DrawImage(img, 0, 0, page.Width, page.Height);
        }

        doc.Save(outputPdfPath);

        return Ok();
    }


    [HttpGet]
    public IActionResult ExportMonthlyProductSalesToExcel(string month)
    {
        if (!DateTime.TryParseExact(month, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out DateTime parsedMonth))
            return BadRequest("Geçersiz tarih");

        var startDate = parsedMonth;
        var endDate = parsedMonth.AddMonths(1).AddDays(-1);

        // Satış itemlarını çekiyoruz
        var saleItems = _saleItemDal.GetAll(x =>
            x.CreateDate.Date >= startDate.Date &&
            x.CreateDate.Date <= endDate.Date &&
            !x.IsDeleted
        );

        var productList = _productService.TGetList();

        var productSales = saleItems
            .GroupBy(x => x.ProductId)
            .Select(g => new
            {
                ProductName = productList.FirstOrDefault(p => p.Id == g.Key)?.Name ?? "(Ürün Yok)",
                TotalQuantity = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
            }).ToList();

        using var workbook = new ClosedXML.Excel.XLWorkbook();
        var worksheet = workbook.Worksheets.Add($"{month} Satış Raporu");

        worksheet.Cell(1, 1).Value = "Ürün Adı";
        worksheet.Cell(1, 2).Value = "Toplam Satış Adedi";
        worksheet.Cell(1, 3).Value = "Toplam Gelir (₺)";

        for (int i = 0; i < productSales.Count; i++)
        {
            worksheet.Cell(i + 2, 1).Value = productSales[i].ProductName;
            worksheet.Cell(i + 2, 2).Value = productSales[i].TotalQuantity;
            worksheet.Cell(i + 2, 3).Value = productSales[i].TotalRevenue;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var content = stream.ToArray();

        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"UrunSatisRaporu_{month}.xlsx");
    }

}
