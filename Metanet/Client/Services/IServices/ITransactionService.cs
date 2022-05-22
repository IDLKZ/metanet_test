using Metanet.Shared.Models;

namespace Metanet.Client.Services.IServices;

public interface ITransactionService
{
    public Task<List<Transaction>> GetAllAsync();
}