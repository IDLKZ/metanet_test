using Metanet.Client.Services.IServices;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Sotsera.Blazor.Toaster;
using System.Net.Http.Json;

namespace Metanet.Client.Services
{
    public class LessonService : ILessonService
    {

        private readonly HttpClient httpClient;
        private IToaster Toaster;
        public LessonService(HttpClient _client,IToaster _Toaster)
        {
            httpClient = _client;
            Toaster = _Toaster;
        }

        public async Task<Lesson> Create(LessonCreateDTO lesson)
        {
            var response = await httpClient.PostAsJsonAsync("api/lesson/create",lesson);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<Lesson>>();
            if (result.Success)
            {
                Toaster.Success("Урок успешно создан");
                return result.Data;
            }
            else
            {
                Toaster.Error(result.Message ?? "Что-то пошло не так");
                return null;
            }
        }

        public async Task<bool> Delete(int Id)
        {
            var response = await httpClient.DeleteAsync($"api/lesson/delete/{Id}");
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            if (result.Success == true)
            {
                Toaster.Success("Урок успешно удален");
                return true;
            }
            Toaster.Error(result.Message ?? "Что-то пошло не так");
            return false;
        }

        public async Task<List<Lesson>> GetAllLesson()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Lesson>>>("api/lesson/GetAllLessons");
            if (result.Success)
            {
                return result.Data;
            }
            else
            {
                return null;
            }
        }

        public async Task<Lesson> GetLesson(int Id)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<Lesson>>($"api/lesson/get/{Id}");
            if (result.Success)
            {
                return result.Data;
            }
            else
            {
                Toaster.Error(result.Message ?? "Урок не найден");
                return null;
            }
        }

        public async Task<LessonDTO> GetLessonByAlias(string Alias)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<LessonDTO>>($"api/lesson/GetByAlias/{Alias}");
            if (result.Success)
            {
                return result.Data;
            }
            else
            {
                Toaster.Error(result.Message ?? "Урок не найден");
                return null;
            }
        }

        public async Task<Lesson> Update(int Id, LessonDTO lesson)
        {
            var response = await httpClient.PutAsJsonAsync($"api/lesson/Update/{Id}", lesson);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<Lesson>>();
            if(result.Success == true)
            {
                Toaster.Success("Урок успешно обновлен");
                return result.Data;
            }
            else
            {
                Toaster.Error(result.Message ?? "Что-то пошло не так");
                return null;
            }
        }
    }
}
