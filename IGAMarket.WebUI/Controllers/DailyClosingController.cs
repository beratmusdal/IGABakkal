using Microsoft.AspNetCore.Mvc;

namespace IGAMarket.WebUI.Controllers
{
    public class DailyClosingController : Controller
    {
        public IActionResult DailyClosingIndex()
        {
            return View();
        }
    }
}
