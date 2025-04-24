using ClosedXML.Excel;
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
    private readonly IFireService _fireService;

    public MonthlyController(IDailyClosurService dailyClosur, ISaleDal saleDal, ISaleItemDal saleItemDal, IProductService productService, IFireService fireService)
    {
        _dailyClosur = dailyClosur;
        _saleDal = saleDal;
        _saleItemDal = saleItemDal;
        _productService = productService;
        _fireService = fireService;
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
                g.DrawString(item.PersonelName, font, brush, 1180, y);                                  
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
        worksheet.Cell("A1").Value = "Tarih";
        worksheet.Cell("B1").Value = "Ürün Adı";
        worksheet.Cell("C1").Value = "Adet";
        worksheet.Cell("D1").Value = "Toplam Zarar (₺)";
        worksheet.Cell("E1").Value = "Açıklama";

        int row = 2;
        foreach (var fire in fireList)
        {
            worksheet.Cell(row, 1).Value = fire.CreateDate.ToString("dd.MM.yyyy");
            worksheet.Cell(row, 2).Value = fire.Name ?? "(Ürün Yok)";
            worksheet.Cell(row, 3).Value = fire.Quantity;
            worksheet.Cell(row, 4).Value = fire.TotalPurchasePrice;
            worksheet.Cell(row, 5).Value = fire.Reason ?? "-";

            // Açıklama satırı çok uzunsa otomatik sarma
            worksheet.Cell(row, 5).Style.Alignment.WrapText = true;

            row++;
        }

        // Başlık stili
        var headerRange = worksheet.Range("A1:E1");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGoldenrodYellow;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        // Font & Stil ayarları
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.FontSize = 11;

        // Otomatik hücre genişliği & yükseklik
        worksheet.Columns().AdjustToContents();
        worksheet.Rows().AdjustToContents();

        // TL kolonunu formatla
        worksheet.Column(4).Style.NumberFormat.Format = "#,##0.00";

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var content = stream.ToArray();

        return File(content,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"HibeRaporu_{month}.xlsx");
    }



}
