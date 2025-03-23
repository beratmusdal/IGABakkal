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
        private readonly IMapper _mapper;

        public FireController(IFireService fireService, IMapper mapper)
        {
            _fireService = fireService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult CreateFireIndex()
        {
            var model = new FireModelView
            {
                ListFireDto = _fireService.GetDetailList()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateFireIndex(AddFireDto AddFireDto)
        {
            _fireService.TInsert(_mapper.Map<Fire>(AddFireDto));
            
            var model = new FireModelView
            {
                ListFireDto = _fireService.GetDetailList()
            };
            return View(model);
        }

        //public IActionResult DeleteFire(int id)
        //{
        //    _fireService.UpdateById(id);
        //    var model = new FireModelView
        //    {
        //        ListFireDto = _fireService.GetDetailList()
        //    };
        //    return View(nameof(Index),model);
        //}
        [HttpPost]
        public JsonResult DeleteFire(int id)
        {
            try
            {
                _fireService.UpdateById(id); // Silme işlemi
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Silme işlemi başarısız! " + ex.Message });
            }
        }



    }
}
