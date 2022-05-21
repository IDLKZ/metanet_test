using AutoMapper;
using Metanet.Server.Database;
using Metanet.Server.Services;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlugGenerator;
namespace Metanet.Server.Controllers
{

    [ApiController]
    [Route("/api/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class CourseController : ControllerBase
    {

        private readonly ICourseService courseService;
        public CourseController(ICourseService _courseService)
        {
            courseService = _courseService;
        }


        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Course>>> Create([FromBody] CourseCreateDTO courseDTO)
        {
            ServiceResponse<Course> response = await courseService.CreateCourse(courseDTO);
            return Ok(response);

        }


        [HttpPut("{Id:int}")]
        public async Task<ActionResult<ServiceResponse<Course>>> Update(int Id, [FromBody] CourseDTO courseDTO)
        {
            ServiceResponse<Course> response = await courseService.UpdateCourse(Id,courseDTO);
            return Ok(response);

        }


        [HttpGet("{Alias}")]
        public async Task<ActionResult<ServiceResponse<CourseDTO>>> Get(string Alias)
        {
            ServiceResponse<CourseDTO> response = await courseService.GetCourseByAlias(Alias);
            return  Ok(response);
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Course>>>> GetAllCourses()
        {
            ServiceResponse<List<Course>> response = await courseService.GetAllCourses();
            return Ok(response);
        }

    }
}
