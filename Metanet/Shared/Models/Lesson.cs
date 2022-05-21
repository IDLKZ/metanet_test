using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metanet.Shared.Models
{
    public class Lesson
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MaxLength(255, ErrorMessage = "Максимальная длина поля 255 знаков")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Alias { get; set; }

        public string? Description { get; set; }
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string VideoUrl { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Range(0,100,ErrorMessage ="Значение поля должно быть в промежутках от 0 до 100")]
        public int Number { get; set; }

        [ForeignKey("Blog")]
        public int BlogId { get; set; }

        public Blog Blog { get; set; }

        [ForeignKey("Lesson")]
        public int? PrevLessonId { get; set; }
        public virtual Lesson? PrevLesson { get; set; }

        [ForeignKey("Lesson")]
        public int? NextLessonId { get; set; }
        public virtual Lesson? NextLesson { get; set; }
        public List<Material> Materials { get; set; }


    }
}
