
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.DTO
{
    public class MailRequest
    {

        [Required(ErrorMessage="Тема собщения обязательна для заполнения")]
        [MaxLength(255,ErrorMessage = "Тема сообщения не должна превышать размер более чем на 255 знаков")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Основной текст сообщения обязателен для заполнения")]
        public string Body { get; set; }

        public List<IFormFile>? Attachments { get; set; }
        public List<IBrowserFile>? CurrentFile { get; set; }

        public List<string>? Emails { get; set; }

    }
}
