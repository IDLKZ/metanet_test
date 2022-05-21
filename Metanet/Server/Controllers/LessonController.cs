using Metanet.Server.Services;
using Metanet.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Metanet.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService lessonService;
        public LessonController(ILessonService _lessonService)
        {
            lessonService = _lessonService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] LessonCreateDTO model)
        {
            var response = await lessonService.CreateLesson(model);
           return  Ok(response);
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Update(int Id, [FromBody] LessonDTO model)
        {
            var response = await lessonService.UpdateLesson(Id, model);
           return  Ok(response);
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var response = await lessonService.DeleteLesson(Id);
           return  Ok(response);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult> Get(int Id)
        {
            var response = await lessonService.GetLesson(Id);
           return  Ok(response);
        }

        [HttpGet("{Alias}")]
        public async Task<ActionResult> GetByAlias(string Alias)
        {
            var response = await lessonService.GetLessonByAlias(Alias);
           return  Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllLessons()
        {
            var response = await lessonService.GetAllLessons();
           return  Ok(response);
        }

    }
}
