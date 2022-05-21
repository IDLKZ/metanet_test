using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.DTO
{
    public class ResetDTO
    {


        [MinLength(4, ErrorMessage = "Пароль должен содержать более 4 знаков")]
        [MaxLength(16, ErrorMessage = "Максимальная длина пароля 16 знаков")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Пожалуйста, заполните поле Пароля")]
        [Compare("Password", ErrorMessage = "Повторите пароль и пароль должны совпадать")]
        public string ConfirmPassword { get; set; }

        
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
