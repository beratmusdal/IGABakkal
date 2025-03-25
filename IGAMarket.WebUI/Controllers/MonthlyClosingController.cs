using Microsoft.AspNetCore.Mvc;

namespace IGAMarket.WebUI.Controllers
{
    public class MonthlyClosingController : Controller
    {
        public IActionResult MonthlyClosingIndex()
        {
            return View();
        }
    }
}
