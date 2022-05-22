using Metanet.Shared.Models;
using Microsoft.OpenApi.Any;

namespace Metanet.Server.Services;

public interface ITransactionService
{
    public Task<List<Transaction>> GetAllById(string id);
}