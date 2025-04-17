using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IGAMarket.WebUI.Models;
using Microsoft.AspNetCore.Authorization;

namespace IGAMarket.WebUI.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult IslemlerIndex()
        {
            return View();
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult ClosingIndex()
        {
            return View();
        }

        [Authorize]
        public IActionResult SatisIndex()
        {
            return View();
        }

    }
}


