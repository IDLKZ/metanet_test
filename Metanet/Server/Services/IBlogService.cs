using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;

namespace Metanet.Server.Services
{
    public interface IBlogService
    {


        public Task<ServiceResponse<List<Blog>>> GetAllBlogs();
        public Task<ServiceResponse<Blog>> CreateBlog(BlogCreateDTO model);
        public Task<ServiceResponse<Blog>> UpdateBlog(int Id,BlogDTO model);

        public Task<ServiceResponse<bool>> DeleteBlog(int Id);
        public Task<ServiceResponse<Blog>> GetBlog(int Id);
        public Task<ServiceResponse<BlogDTO>> GetBlogByAlias(string Alias);

        public Task<ServiceResponse<List<Lesson>>> GetBlogLessons(int BlogId);
    }
}
