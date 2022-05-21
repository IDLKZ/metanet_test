using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Поле обязательно для заполнения")]
        [MaxLength(255,ErrorMessage ="Максимальная длина поля 255 знаков")]
        public string Title { get; set; }

        [Required]
        public string Alias { get; set; }

        public string? Description { get; set; }

       [Required(ErrorMessage ="Поле обязательно для заполнения")]
        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        
        public Course Course { get; set; }

        public List<Lesson> Lessons { get; set; }

    }
}
