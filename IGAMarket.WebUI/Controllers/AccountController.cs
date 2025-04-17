
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace IGAMarket.WebUI.Controllers;
public class AccountController : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Identity.Application");

        Response.Cookies.Delete(".AspNetCore.Identity.Application");

        return RedirectToAction("LoginIndex", "Login");
    }



}
