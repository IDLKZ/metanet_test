using Metanet.Client.Services.IServices;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Sotsera.Blazor.Toaster;
using System.Net.Http.Json;

namespace Metanet.Client.Services
{
    public class MaterialService : IMaterialService
    {
        protected readonly HttpClient httpClient;
        private IToaster Toaster;
        public MaterialService(HttpClient _httpClient,IToaster _Toaster)
        {
            httpClient = _httpClient;
            Toaster = _Toaster;
        }
        public async Task<bool> Create(MaterialCreateDTO material)
        {
            var response = await httpClient.PostAsJsonAsync("api/material/create", material);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<Material>>();
            if(result.Success != false)
            {
                Toaster.Success("Материал успешно добавлен к уроку!");
                return true;
            }
            else
            {
                Toaster.Error(result.Message ?? "Что-то пошло не так");
                return false; 
            }
        }

        public async Task<bool> Delete(int Id)
        {

            var response = await httpClient.DeleteAsync($"api/material/delete/{Id}");
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            if(result.Success == true)
            {
                Toaster.Success("Материал успешно удален!");
                return true;
            }
            else
            {
                Toaster.Error(result.Message??"Материал не найден!");
                return false;
            }

        }

        public async Task<MaterialDTO> Get(int Id)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<MaterialDTO>>($"api/material/get/{Id}");
            return result.Data;
        }

        public async Task<List<Material>> GetMaterialsByLessonId(int LessonId)
        {
            var response = await httpClient.GetFromJsonAsync<ServiceResponse<List<Material>>>($"api/material/GetAllMaterials/{LessonId}");
            return response.Data;
          
        }

        public async Task<bool> Update(int Id,MaterialDTO material)
        {
            var response = await httpClient.PutAsJsonAsync($"api/material/update/{Id}",material);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<Material>>();
            if (result.Success == true)
            {
                Toaster.Success("Материал успешно обновлен!");
                return true;
            }
            else
            {
                Toaster.Error(result.Message ?? "Что-то пошло не так!");
                return false;
            }
        }
    }
}
