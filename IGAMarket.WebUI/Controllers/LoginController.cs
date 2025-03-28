using Microsoft.AspNetCore.Mvc;

namespace IGAMarket.WebUI.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult LoginIndex()
        {
            return View();
        }

        

    }
}
