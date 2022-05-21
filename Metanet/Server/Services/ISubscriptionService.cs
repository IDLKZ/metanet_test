using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Mvc;

namespace Metanet.Server.Services;

public interface ISubscriptionService
{
    public Task<ServiceResponse<bool>> Create(SubscriptionCreateDTO subscriptionCreateDto);
    public Task<ServiceResponse<bool>> Update(int Id,SubscriptionUpdateDTO subscriptionUpdateDto);
    public Task<ServiceResponse<Subscription>> GetById(int Id);
    public Task<ServiceResponse<PaginationDTO<Subscription>>> GetAllSubscriptions(int page, int show = 5, string? search = "");
}