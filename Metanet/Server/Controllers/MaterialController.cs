
using Metanet.Server.Services;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Metanet.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class MaterialController : ControllerBase
    {
        protected IMaterialService materialService;
        public MaterialController(IMaterialService _materialService)
        {
            materialService = _materialService;
        }

        [HttpGet("{LessonId}")]
        public async Task<ActionResult<ServiceResponse<List<Material>>>> GetAllMaterials(int LessonId)
        {
            ServiceResponse<List<Material>> response = await materialService.GetAllMaterials(LessonId);
           return  Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ServiceResponse<MaterialDTO>>> Get(int Id)
        {
            ServiceResponse<MaterialDTO> response = await materialService.GetMaterial(Id);
           return  Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<Material>>>> Create([FromBody] MaterialCreateDTO material)
        {
            ServiceResponse<Material> response = await materialService.CreateMaterial(material);
           return  Ok(response);
        }


        [HttpPut("{Id}")]
        public async Task<ActionResult<ServiceResponse<List<Material>>>> Update(int Id, [FromBody] MaterialDTO material)
        {
            ServiceResponse<Material> response = await materialService.UpdateMaterial(Id,material);
           return  Ok(response);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<ServiceResponse<List<Material>>>> Delete(int Id)
        {
            ServiceResponse<bool> response = await materialService.DeleteMaterial(Id);
           return  Ok(response);
        }




    }
}
