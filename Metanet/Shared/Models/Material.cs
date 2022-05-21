using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.Models
{
    public class Material
    {
        [Key]
        public int Id { get; set; }

        public string? Title { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string File { get; set; }

        [ForeignKey("Lesson")]
        public int LessonId { get; set; }

        public Lesson Lesson { get; set; }

    }
}
