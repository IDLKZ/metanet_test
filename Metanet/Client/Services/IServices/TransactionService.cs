using System.Net.Http.Json;
using Metanet.Shared.Models;

namespace Metanet.Client.Services.IServices;

public class TransactionService : ITransactionService
{
    private readonly HttpClient _httpClient;

    public TransactionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<Transaction>> GetAllAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Transaction>>("api/transaction");
        return response;
    }
}