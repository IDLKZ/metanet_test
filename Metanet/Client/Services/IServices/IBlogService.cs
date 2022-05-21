using Metanet.Shared.DTO;
using Metanet.Shared.Models;

namespace Metanet.Client.Services.IServices
{
    public interface IBlogService
    {
        public Task<List<Blog>> GetAllBlogs();

        public Task<bool> CreateBlog(BlogCreateDTO blog);

        public Task<BlogDTO> GetBlogByAlias(string Alias);

        public Task<bool> UpdateBlog(int Id, BlogDTO blog);

        public Task<bool> DeleteBlog(int Id);

        public Task<List<Lesson>> GetBlogLessons(int BlogId);
    }
}
