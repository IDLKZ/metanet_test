using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.Auth
{
    public class ForgetDTO
    {
        [Required(ErrorMessage ="Поле почта обязательна для заполнения")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Пожалуйста, введите валидную почту")]
        public string Email { get; set; }

    
    }
}
