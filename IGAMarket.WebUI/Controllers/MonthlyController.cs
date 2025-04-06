using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IGAMarket.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MonthlyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
