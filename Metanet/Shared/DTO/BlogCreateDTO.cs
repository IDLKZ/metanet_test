using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.DTO
{
    public class BlogCreateDTO
    {

        [Required(ErrorMessage = "Поле Наименование Блога обязательно для заполнения")]
        [MaxLength(255, ErrorMessage = "Максимальная длина поля наименование 255 знаков")]
        public string Title { get; set; }

       
        public string? Description { get; set; }

        [Required(ErrorMessage = "Поле Дата начала обязательно для заполнения")]
        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        [Required(ErrorMessage = "Поле Курс обязательно для заполнения")]

        public int CourseId { get; set; }
    }
}
