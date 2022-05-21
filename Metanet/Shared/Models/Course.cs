using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MaxLength(255, ErrorMessage = "Максимальная длина поля 255 знаков")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Alias { get; set; }

        public string? Description { get; set;}

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Range(0, 1000000000,ErrorMessage = "Поле должно быть больше 0")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public List<Blog> Blog { get; set; }

        public IList<Subscription> Subscriptions { get; set; }
    }
}
