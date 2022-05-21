using Metanet.Client.Services.IServices;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Sotsera.Blazor.Toaster;
using System.Net.Http.Json;

namespace Metanet.Client.Services
{
    public class CourseService : ICourseService
    {
        private readonly HttpClient httpClient;
        private IToaster Toaster;
        public CourseService(HttpClient client,IToaster _Toaster)
        {
            httpClient = client;
            Toaster = _Toaster;
        }

        public async Task<bool> CreateCourse(CourseCreateDTO course)
        {
        
                var response = await httpClient.PostAsJsonAsync<CourseCreateDTO>("api/course/create", course);
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<Course>>();
                if (result.Success == true)
                {
                    Toaster.Success("Курс успешно создан");
                    return true;
                }
                else
                {
                    Toaster.Warning(result.Message ?? "Упс, что-то пошло не так");
                    return false;
                }
            
           
            
        }

        public async Task<List<Course>> GetAllCourses()
        {
            
                var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Course>>>("api/course/GetAllCourses");
                            if(result.Success == true)
                            {
                                return result.Data;
                            }
                            else
                            {
                                return null;
                            }
            
            
        }

        public async Task<CourseDTO> GetCourseByAlias(string Alias)
        {
            
                var result = await httpClient.GetFromJsonAsync<ServiceResponse<CourseDTO>>($"api/course/get/{Alias}");
                if (result.Success)
                {
                    return result.Data;
                }
                else
                {
                    Toaster.Warning(result.Message ?? "Упс, курс не найден");
                    return null;
                }
            
            
            
        }

        public async Task<bool> UpdateCourse(int Id,CourseDTO course)
        {
            
                var response = await httpClient.PutAsJsonAsync($"api/course/update/{Id}", course);
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<Course>>();
                if (result.Success == true)
                {
                    Toaster.Success("Курс успешно обновлен");
                    return true;
                }
                else
                {
                    Toaster.Warning(result.Message ?? "Упс, что-то пошло не так");
                    return false;
                }
             return false;
            
            

        }
    }
}
