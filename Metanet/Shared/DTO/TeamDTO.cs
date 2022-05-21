using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.DTO
{
    public class TeamDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле имя обязательно для заполнения")]
        [MaxLength(255, ErrorMessage = "Максимальная длина поля имя 255 знаков")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле позиции обязательно для заполнения")]
        [MaxLength(255, ErrorMessage = "Максимальная длина поля позиции 255 знаков")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Поле изображения обязательно для заполнения")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Поле информация обязательно для заполнения")]
        [MaxLength(1000, ErrorMessage = "Максимальная длина поля информация 1000 знаков")]
        public string Info { get; set; }
    }
}
