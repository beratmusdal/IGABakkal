using AutoMapper;
using IGAMarket.DtoLayer.UserDtos;
using IGAMarket.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IGAMarket.WebUI.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RegisterController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public IActionResult RegisterIndex()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = _mapper.Map<User>(createUserDto);

            // Kullanıcı oluştur
            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("LoginIndex", "Login");
            }
            return View();
        }

    }
}
