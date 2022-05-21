using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.DTO
{
    public class LessonCreateDTO
    {
        [Required(ErrorMessage = "Поле наименования обязательно для заполнения")]
        [MaxLength(255, ErrorMessage = "Максимальная длина поля наименования 255 знаков")]
        public string Title { get; set; }

        public string? Description { get; set; }
        [Required(ErrorMessage = "Поле изображения обязательно для заполнения")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Поле видео обязательно для заполнения")]
        public string VideoUrl { get; set; }

        [Required(ErrorMessage = "Поле номера обязательно для заполнения")]
        [Range(0, 100, ErrorMessage = "Значение поля номера должно быть в промежутках от 0 до 100")]
        public int Number { get; set; }

        [Required(ErrorMessage = "Поле блока обязательно для заполнения")]
        public int BlogId { get; set; }

     
        public int? PrevLessonId { get; set; }

        public int? NextLessonId { get; set; }
      
    }
}
