using AutoMapper;
using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.DtoLayer.DailyClosurDtos;
using IGAMarket.DtoLayer.ProductDtos;
using IGAMarket.EntityLayer.Concrete;
using IGAMarket.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IGAMarket.WebUI.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class DailyController : Controller
    {
        private readonly IProductService _productService;
        private readonly IDailyClosurService _dailyClosurService;
        private readonly IFireService _fireService;
        private readonly IMapper _mapper;

        public DailyController(
            IProductService productService,
            IDailyClosurService dailyClosurService,
            IFireService fireService,
            IMapper mapper)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _dailyClosurService = dailyClosurService ?? throw new ArgumentNullException(nameof(dailyClosurService));
            _fireService = fireService ?? throw new ArgumentNullException(nameof(fireService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult DailyIndex()
        {
            var today = DateTime.Today;

            var productList = _productService.TGetList();
            var productDtoList = _mapper.Map<List<ResultProductDto>>(productList);

            var totalFireQuantityToday = _fireService.GetAll()
                .Where(x => x.CreateDate.Date == today && !x.IsDeleted)
                .Sum(x => x.Quantity);

            var model = new ProductModelView
            {
                ResultProductDto = productDtoList,
                TodayFireCount = totalFireQuantityToday
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateProductStock([FromBody] ProductStockUpdateDto model)
        {
            if (model == null || model.Id <= 0 || model.NewStock < 0)
            {
                return Json(new { success = false, message = "Geçersiz ürün bilgisi." });
            }

            try
            {
                _productService.UpdateProductStockQuantity(model.Id, model.NewStock);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Stok güncellenemedi: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult CreateDaily(CreateDailyDto createDailyDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Form verileri geçerli değil.";
                return RedirectToAction("DailyIndex");
            }

            try
            {
                var entity = _mapper.Map<DailyClosur>(createDailyDto);
                _dailyClosurService.TInsert(entity);

                TempData["SuccessMessage"] = "Günlük kapanış başarıyla kaydedildi.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Kapanış sırasında hata oluştu: {ex.Message}";
                return RedirectToAction("DailyIndex");
            }
        }
    }
}
