using Metanet.Client.Services.IServices;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Sotsera.Blazor.Toaster;
using System.Net.Http.Json;

namespace Metanet.Client.Services
{
    public class BlogService : IBlogService
    {
        private readonly HttpClient httpClient;
        private IToaster Toaster;
        public BlogService(HttpClient client, IToaster _Toaster)
        {
            httpClient = client;
            Toaster = _Toaster;
        }

        public async Task<bool> CreateBlog(BlogCreateDTO blog)
        {
           
             var response = await httpClient.PostAsJsonAsync("api/blog/create", blog);
                        var result = await response.Content.ReadFromJsonAsync<ServiceResponse<Blog>>();
                        if (result.Success)
                        {
                            Toaster.Success("Блок успешно создан");
                            return true;
             }
             Toaster.Warning(result.Message ?? "Что-то пошло не так");
              return false;

        }

        public async Task<bool> DeleteBlog(int Id)
        {
            var response = await httpClient.DeleteAsync($"api/blog/delete/{Id}");
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            if(result.Success == true)
            {
                Toaster.Success("Блок успешно удален");
                return true;
            }
            Toaster.Error(result.Message ?? "Что-то пошло не так");
            return false;
            

        }

        public async Task<List<Blog>> GetAllBlogs()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Blog>>>("api/blog/GetAllBlogs");
            if(result.Success == true)
            {
                return result.Data;
            }
            return null;
        }

        public async Task<BlogDTO> GetBlogByAlias(string Alias)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<BlogDTO>>($"api/blog/GetByAlias/{Alias}");
            if (result.Success)
            {
                return result.Data;
            }
            Toaster.Error(result.Message ?? "Блок не найден");
            return null;
        }

        public async Task<List<Lesson>> GetBlogLessons(int BlogId)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Lesson>>>($"api/blog/GetBlogLessons/{BlogId}");
            return result.Data;
            
        }

        public async Task<bool> UpdateBlog(int Id, BlogDTO blog)
        {
            var response = await httpClient.PutAsJsonAsync($"api/blog/Update/{Id}",blog);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<Blog>>();
            if (result.Success)
            {
                Toaster.Success("Блок успешно обновлен");
                return true;
            }
            Toaster.Error(result.Message ?? "Что-то пошло не так");
            return false;
        }
    }
}
