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
            _productService = productService;
            _dailyClosurService = dailyClosurService;
            _fireService = fireService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult DailyIndex()
        {
            var productList = _productService.TGetList();
            var productDtoList = _mapper.Map<List<ResultProductDto>>(productList);
            var today = DateTime.Today;
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
        public IActionResult UpdateProductStock([FromBody] ProductStockUpdateDto model)
        {
            try
            {
                _productService.UpdateProductStockQuantity(model.Id, model.NewStock);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateDaily(CreateDailyDto createDailyDto)
        {
            try
            {
                _dailyClosurService.TInsert(_mapper.Map<DailyClosur>(createDailyDto));
                TempData["SuccessMessage"] = "Günlük kapanış başarıyla kaydedildi.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Günlük kapanış kaydedilirken bir hata oluştu: " + ex.Message);
                return View("DailyIndex", createDailyDto);
            }
        }
    }
}
