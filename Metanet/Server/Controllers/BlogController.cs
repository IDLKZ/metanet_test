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
    [Authorize(Roles ="Admin")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService blogService;
        public BlogController(IBlogService blog)
        {
            blogService = blog;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Blog>>> Create([FromBody] BlogCreateDTO model)
        {
            ServiceResponse<Blog> service = await blogService.CreateBlog(model);
            return Ok(service);
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult<ServiceResponse<Blog>>> Update(int Id, [FromBody] BlogDTO model)
        {
            ServiceResponse<Blog> service = await blogService.UpdateBlog(Id, model);
            return Ok(service);
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult<ServiceResponse<bool>>> Delete(int Id)
        {
            ServiceResponse<bool> service = await blogService.DeleteBlog(Id);
            return Ok(service);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<ServiceResponse<Blog>>> Get(int Id)
        {
            ServiceResponse<Blog> service = await blogService.GetBlog(Id);
            return Ok(service);
        }
        [HttpGet("{Alias}")]
        public async Task<ActionResult<ServiceResponse<BlogDTO>>> GetByAlias(string Alias)
        {
            ServiceResponse<BlogDTO> service = await blogService.GetBlogByAlias(Alias);
            return Ok(service);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Blog>>>> GetAllBlogs()
        {
            ServiceResponse<List<Blog>> service = await blogService.GetAllBlogs();
            return Ok(service);
        }


        [HttpGet("{BlogId:int}")]

        public async Task<ActionResult<List<Lesson>>> GetBlogLessons(int BlogId){
            ServiceResponse<List<Lesson>> service = await blogService.GetBlogLessons(BlogId);
            return Ok(service);
        }
            
    }
}
