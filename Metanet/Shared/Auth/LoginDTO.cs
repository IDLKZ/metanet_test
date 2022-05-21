using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.Auth
{
    public class LoginDTO
    {

        [Required(ErrorMessage = "Пожалуйста, заполните поле Почты")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Введите валидную почту")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пожалуйста, заполните поле Пароля")]
        [MinLength(4, ErrorMessage = "Пароль должен содержать более 4 знаков")]
        [MaxLength(16, ErrorMessage = "Максимальная длина пароля 16 знаков")]
        public string Password { get; set; }


    }
}
