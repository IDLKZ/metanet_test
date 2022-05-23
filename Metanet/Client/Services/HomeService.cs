using Metanet.Client.Services.IServices;
using Metanet.Shared;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Sotsera.Blazor.Toaster;
using System.Net.Http.Json;
namespace Metanet.Client.Services
{
    public class HomeService : IHomeService
    {
        private readonly HttpClient httpClient;
        private IToaster Toaster;
        public HomeService(HttpClient client, IToaster _Toaster)
        {
            httpClient = client;
            Toaster = _Toaster;
        }

        public async Task<List<Blog>> GetAllBlogs()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Blog>>>("api/home/GetAllBlogs");
            
            return result.Data;
        }

        public async Task<List<Team>> GetAllTeams()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Team>>>("api/home/GetAllTeams");
            return result.Data;
        }

        public async Task<Blog> GetBlogByAlias(string Alias)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<Blog>>($"api/home/GetBlogByAlias/{Alias}");
            if(result.Success != true)
            {
                Toaster.Error(result.Message ?? "Блок не найден");
            }
            return result.Data;
        }
        public async Task<Blog> GetBlogById(int Id)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<Blog>>($"api/home/GetBlogById/{Id}");
            if (result.Success != true)
            {
                Toaster.Error(result.Message ?? "Блок не найден");
            }
            return result.Data;
        }
        public async Task<List<Blog>> GetBlogByCourseId(int CourseId)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Blog>>>($"api/home/GetBlogByCourseId/{CourseId}");
            return result.Data;
        }

        public async Task<Course> Course()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<Course>>("api/Home/GetCourse");
            return result.Data;
        }

        public async Task<Course> GetCourseByAlias(string Alias)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<Course>>($"api/home/GetCourseByAlias/{Alias}");
            if (result.Success != true)
            {
                Toaster.Error(result.Message ?? "Курс не найден");
            }
            return result.Data;
        }

        public async Task<Lesson> GetLessonByAlias(string Alias)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<Lesson>>($"api/home/GetLessonByAlias/{Alias}");
            if (result.Success != true)
            {
                Toaster.Error(result.Message ?? "Урок не найден");
            }
            return result.Data;
        }

        public async Task<List<Lesson>> GetLessonByBlogId(int BlogId)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Lesson>>>($"api/home/GetLessonByBlogId/{BlogId}");
            
            return result.Data;
        }

        public async Task<CurrencyDTO> GetCurrency()
        {
            var result = await httpClient.GetFromJsonAsync<CurrencyDTO>($"api/Home/GetCurrency");
            return result;
        }

        public async Task<StatsDTO> GetStats()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<StatsDTO>>($"api/Home/GetAllStats");
            return result.Data;
            
        }

        public async Task<bool> CheckPay()
        {
            var result = await httpClient.GetFromJsonAsync<bool>("api/home/checkpay");
            
            return result;
        }
    }
}
