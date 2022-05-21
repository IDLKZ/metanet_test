using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MaxLength(255, ErrorMessage = "Максимальная длина поля 255 знаков")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MaxLength(255, ErrorMessage = "Максимальная длина поля 255 знаков")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MaxLength(1000, ErrorMessage = "Максимальная длина поля 1000 знаков")]
        public string Info { get; set; }


    }
}
