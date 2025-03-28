using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.DtoLayer.UserDtos
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "Kullanıcı Adı Alanı Gereklidir")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Şifre Alanı Gereklidir")]
        public string Password { get; set; }
        
    }
}
