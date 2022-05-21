using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.DTO
{
    public class CourseCreateDTO
    {

        [Required(ErrorMessage = "Поле наименование обязательно для заполнения")]
        [MaxLength(255, ErrorMessage = "Максимальная длина поля 255 знаков")]
        public string Title { get; set; }

     
        public string? Description { get; set; }

        [Required(ErrorMessage = "Поле изображения обязательно для заполнения")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Поле цены обязательно для заполнения")]
        [Range(50, 1000000000, ErrorMessage = "Поле цены должно быть больше 50")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Поле даты начала обязательно для заполнения")]
        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }
    }
}
