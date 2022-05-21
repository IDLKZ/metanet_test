using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.DTO
{
    public class MaterialCreateDTO
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле Наименования обязательно для заполнения")]
        [MaxLength(255, ErrorMessage = "Максимальная длина поля Наименования 255 знаков")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Поле файла обязательно для заполнения")]
        public string File { get; set; }

        [Required(ErrorMessage = "Поле урока обязательно для заполнения")]
        public int LessonId { get; set; }
    }
}
