using Metanet.Shared.DTO;
using Metanet.Shared.Models;

namespace Metanet.Client.Services.IServices;

public interface ISubscriptionService
{

    public Task<bool> Create(SubscriptionCreateDTO subscriptionCreateDto);
    public Task<bool> Update(int Id,SubscriptionUpdateDTO subscriptionUpdate);
    public Task<PaginationDTO<Subscription>> GetAllSubscription(int page, int show = 5, string? search = "");
    public Task<Tuple<Subscription,SubscriptionUpdateDTO>> GetById(int Id);

    public Task<Subscription> GetSubscriptionByUserId(string userId);





}