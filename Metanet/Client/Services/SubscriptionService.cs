using System.Net.Http.Json;
using Metanet.Client.Services.IServices;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Sotsera.Blazor.Toaster;

namespace Metanet.Client.Services;

public class SubscriptionService : ISubscriptionService
{
    private HttpClient _client;
    private IToaster Toaster;
    public SubscriptionService(HttpClient client,IToaster toaster)
    {
        _client = client;
        Toaster = toaster;
    }
    public async Task<bool> Create(SubscriptionCreateDTO subscriptionCreateDto)
    {
        var response = await _client.PostAsJsonAsync("api/Subscription/Create", subscriptionCreateDto);
        var result = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        if (result.Success)
        {
            Toaster.Success("Успешно создано");
        }
        else
        {
            Toaster.Warning(result.Message ?? "Упс... Что-то пошло не так");

        }

        return result.Data;

    }

    public async Task<bool> Update(int Id, SubscriptionUpdateDTO subscriptionUpdate)
    {
        var response = await _client.PutAsJsonAsync($"api/Subscription/Update/{Id}", subscriptionUpdate);
        var result = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        if (result.Success)
        {
            Toaster.Success("Успешно обновлено");
        }
        else
        {
            Toaster.Warning(result.Message ?? "Упс... Что-то пошло не так");

        }

        return result.Data;
    }

    public async Task<PaginationDTO<Subscription>> GetAllSubscription(int page, int show = 5, string? search = "")
    {
        var result = await _client.GetFromJsonAsync<ServiceResponse<PaginationDTO<Subscription>>>($"api/Subscription/GetAllSubscription?page={page}&show={show}&search={search}");
        if (!result.Success)
        {
            Toaster.Warning(result.Message ?? "Упс... Что-то пошло не так");
        }
        return result.Data;
    }

    public async Task<SubscriptionUpdateDTO> GetById(int Id)
    {
        var result = await _client.GetFromJsonAsync<ServiceResponse<SubscriptionUpdateDTO>>($"api/Subscription/GetById/{Id}");
        if (!result.Success)
        {
            Toaster.Warning(result.Message ?? "Упс... Что-то пошло не так");
        }
        return result.Data;
    }
}