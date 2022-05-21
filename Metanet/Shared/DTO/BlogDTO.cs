using Metanet.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.DTO
{
    public class BlogDTO
    {

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле Наименование обязательно для заполнения")]
        [MaxLength(255, ErrorMessage = "Максимальная длина поля 255 знаков")]
        public string Title { get; set; }

        
        public string? Alias { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Поле Дата начала обязательно для заполнения")]
        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        [Required(ErrorMessage = "Поле курс обязательно для заполнения")]
        public int CourseId { get; set; }

    }
}
