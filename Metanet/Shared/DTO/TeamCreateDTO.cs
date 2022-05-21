using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.DTO
{
    public class TeamCreateDTO
    {
        [Required(ErrorMessage = "Поле имя обязательно для заполнения")]
        [MaxLength(255, ErrorMessage = "Максимальная длина поля имя 255 знаков")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле позиции обязательно для заполнения")]
        [MaxLength(255, ErrorMessage = "Максимальная длина поля позиции 255 знаков")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Поле изображения обязательно для заполнения")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Поле информации обязательно для заполнения")]
        [MaxLength(1000, ErrorMessage = "Максимальная длина поля информации 1000 знаков")]
        public string Info { get; set; }
    }
}
