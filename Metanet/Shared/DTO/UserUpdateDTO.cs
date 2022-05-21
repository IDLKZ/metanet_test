using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.DTO
{
	public class UserUpdateDTO
	{
        [Key]
        public string Id { get; set; }


        [Required(ErrorMessage = "Пожалуйста, заполните поле Почты")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage ="Поле логин обязателен для заполнения")]
        [MaxLength(255, ErrorMessage = "Поле логин не должно быть более 255 знаков")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле имя обязателен для заполнения")]
        [MaxLength(1000,ErrorMessage = "Поле имя не должно быть более 1000 знаков")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле номер телефона обязателен для заполнения")]
        [RegularExpression(@"^\+?77(\d{9})$", ErrorMessage = "Пожалуйста, введите корректный номер телефона")]
        public string PhoneNumber { get; set; }

        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage ="Поле роль обязательно для заполнения")]
        public string Role { get; set; }


        [DataType(DataType.Password, ErrorMessage = "Почта должна быть более 8 знаков, содержать буквы(A-a Z-z) и цифры(0-9), спец знак(@_-)")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Повторите пароль и пароль должны совпадать")]
        public string? ConfirmPassword { get; set; }

    }
}
