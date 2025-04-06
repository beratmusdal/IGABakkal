using IGAMarket.DtoLayer.UserDtos;
using IGAMarket.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IGAMarket.WebUI.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult LoginIndex()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginUserDto.UserName, loginUserDto.Password, false, false);

                if (result.Succeeded)
                {

                    var user = await _userManager.FindByNameAsync(loginUserDto.UserName);
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (roles.Contains("Member"))
                    {
                        return RedirectToAction("SatisIndex", "Satis");
                    }
                    else
                    {
                        return RedirectToAction("LoginIndex", "Login");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Giriş verileri geçersiz.");
            }

            return View("LoginIndex", loginUserDto);

        }







    }
}
