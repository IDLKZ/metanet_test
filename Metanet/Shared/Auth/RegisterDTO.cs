using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.Auth
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Пожалуйста, заполните поле Ваше Логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Пожалуйста, заполните поле Ваше Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, заполните поле Почты")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле номер телефона обязателен для заполнения")]
        //[RegularExpression(@"^\+?77(\d{9})$", ErrorMessage = "Пожалуйста, введите корректный номер телефона")]
        public string PhoneNo { get; set; }


        [Required(ErrorMessage = "Пожалуйста, заполните поле Пароля")]
        [DataType(DataType.Password, ErrorMessage = "Почта должна быть более 8 знаков, содержать буквы(A-a Z-z) и цифры(0-9), спец знак(@_-)")]
        [MinLength(4, ErrorMessage = "Пароль должен содержать более 4 знаков")]
        [MaxLength(16,ErrorMessage = "Максимальная длина пароля 16 знаков")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Пожалуйста, заполните поле Пароля")]
        [Compare("Password",ErrorMessage ="Повторите пароль и пароль должны совпадать")]
        public string ConfirmPassword { get; set; }




    }
}
