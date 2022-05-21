using Metanet.Shared.DTO;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.IO;
using System; 
namespace Metanet.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class FileUploaderController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        string folder = "uploads";
        string subFolder = "uploads";
        long size = 0;
        public FileUploaderController(IWebHostEnvironment webHost)
        {
            env = webHost;
        }

        [DisableRequestSizeLimit]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<string>>> FileUpload([FromForm] IFormFile file)
        {
            try
            {
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folder);
                var fileName = Guid.NewGuid() + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return Ok(new ServiceResponse<string>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status201Created,
                    Data = dbPath
                });
            }


            catch (Exception ex)
            {
                return Ok(new ServiceResponse<string> {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Что-то пошло не так"
                });
            }
        }




        [HttpGet]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteFile([FromQuery]string path)
        {
            var fullpath = env.ContentRootPath + path;
            if (System.IO.File.Exists(fullpath))
            {
                 System.IO.File.Delete(fullpath);
                return Ok(
                    new ServiceResponse<bool>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Успешно удалено"
                    }
                    );
            }
            return Ok(
                   new ServiceResponse<bool>
                   {
                       Success = false,
                       StatusCode = StatusCodes.Status404NotFound,
                       Message = $"{fullpath}"
                   }
                   );

        }




     

    }



}
