using Metanet.Client.Services.IServices;
using Metanet.Shared.DTO;
using Metanet.Shared.ResponsesDTO;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Text.Json;
using Metanet.Shared.Models;
using Sotsera.Blazor.Toaster;

namespace Metanet.Client.Services
{
    public class UserService : IUserService
    {

        private HttpClient http;
        IToaster Toaster;
        public UserService(HttpClient _http,IToaster _Toaster)
        {
            http = _http;
            Toaster = _Toaster;
        }

		public async Task<UserDTO> Create(CreateUserDTO userDTO)
		{
            var response = await http.PostAsJsonAsync("api/User/Create",userDTO);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<UserDTO>>();
            if (result.Success == true) { Toaster.Success(result.Message ?? "Успешно создан пользователь"); }
            else { Toaster.Error(result.Message ?? "Что-то пошло не так"); }
            return result.Data;
        }

		public async Task<bool> Delete(string Id)
		{
            var response = await http.DeleteAsync($"api/User/Delete/{Id}");
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            if(result.Success == true) { Toaster.Success(result.Message ?? "Успешно удалено"); }
			else { Toaster.Error(result.Message ?? "Что-то пошло не так"); }
            return result.Success;
        }

		public async Task<PaginationDTO<UserDTO>> GetAllUsers(int page, int showPage = 10,string? search = "")
        {
             var result = await http.GetFromJsonAsync<ServiceResponse<PaginationDTO<UserDTO>>>($"api/User/GetAllUsers?page={page}&show={showPage}&search={search}");
             return result.Data;
        }

        public async Task<UserUpdateDTO> GetForUpdate(string Id)
        {
            var result = await http.GetFromJsonAsync<ServiceResponse<UserUpdateDTO>>($"api/User/GetForUpdate/{Id}");
            if (!result.Success)
            {
                Toaster.Error(result.Message ?? "Пользователь не найден");
            }
           
            return result.Data;
        }

        public async Task<bool> Update(string Id, UserUpdateDTO userUpdate)
        {
            var response = await http.PutAsJsonAsync($"api/User/Update/{Id}", userUpdate);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<UserUpdateDTO>>();
            if(result.Success == true) { Toaster.Success(result.Message ?? "Успешно обновлено"); }
            else { Toaster.Error(result.Message ?? "Что-то пошло не так"); }
            return result.Success;
        }


        public async Task<IEnumerable<UserDTO>> Search(string Search)
        {
            var result = await http.GetFromJsonAsync<ServiceResponse<IEnumerable<UserDTO>>>($"api/User/Search/{Search}");
            if (result.Success == true) { Toaster.Success(result.Message ?? "Успешно обновлено"); }
            else { Toaster.Error(result.Message ?? "Что-то пошло не так"); }
            return result.Data;
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var result = await http.GetFromJsonAsync<ApplicationUser>($"api/user/getuserbyid/{userId}");
            return result;
        }
        
        public async Task<ApplicationUser> GetAllUserInfoById(string userId)
        {
            var result = await http.GetFromJsonAsync<ApplicationUser>($"api/user/GetAllUserInfoById/{userId}");
            return result;
        }
    }
}
