using Metanet.Client.Services.IServices;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Sotsera.Blazor.Toaster;
using System.Net.Http.Json;

namespace Metanet.Client.Services
{
    public class TeamService : ITeamService
    {
        private readonly HttpClient httpClient;
        private IToaster Toaster;

        public TeamService(HttpClient _httpClient,IToaster _Toaster)
        {
            httpClient = _httpClient;
            Toaster = _Toaster;
        }


        public async Task<bool> Create(TeamCreateDTO teamCreateDTO)
        {
            var response = await httpClient.PostAsJsonAsync("api/team/create", teamCreateDTO);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<Team>>();
            if (result.Success == true)
            {
                Toaster.Success("Успешно создано");
                return true;
            }
            Toaster.Error(result.Message ?? "Что-то пошло не так");
            return false;
        }

        public async Task<bool> Delete(int Id)
        {
            var response = await httpClient.DeleteAsync($"api/team/delete/{Id}");
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            if (result.Success == true)
            {
                Toaster.Success("Успешно удалено");
                return true;
            }
            Toaster.Error(result.Message ?? "Что-то пошло не так");
            return false;
        }

        public async Task<TeamDTO> Get(int Id)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<TeamDTO>>($"api/team/get/{Id}");
            return result.Data;
        }

        public async Task<List<Team>> GetAllTeams()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Team>>>("api/team/GetAllTeams");
            return result.Data;
        }

        public async Task<bool> Update(int Id, TeamDTO teamDTO)
        {
            var response = await httpClient.PutAsJsonAsync($"api/team/update/{Id}", teamDTO);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<Team>>();
            if (result.Success == true)
            {
                Toaster.Success("Успешно обновлено!");
                return true;
            }
            Toaster.Error(result.Message ?? "Что-то пошло не так");
            return false;
        }
    }
}
