using Metanet.Server.Services;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Mvc;
namespace Metanet.Server.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SubscriptionController : ControllerBase
{
    
    protected ISubscriptionService subscriptionService;
    
    public SubscriptionController(ISubscriptionService _subscriptionService)
    {
        subscriptionService = _subscriptionService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<bool>>> Create([FromBody] SubscriptionCreateDTO subscriptionCreateDto)
    {
        var result = await subscriptionService.Create(subscriptionCreateDto);
        return Ok(result);
    }
    
    [HttpPut("{Id}")]
    public async Task<ActionResult<ServiceResponse<bool>>> Update(int Id,[FromBody] SubscriptionUpdateDTO subscriptionUpdateDto)
    {
        var result = await subscriptionService.Update(Id,subscriptionUpdateDto);
        return Ok(result);
    }
    
    [HttpGet("{Id}")]
    public async Task<ActionResult<ServiceResponse<Subscription>>> GetById(int Id)
    {
        var result = await subscriptionService.GetById(Id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<PaginationDTO<Subscription>>>> GetAllSubscription(
        [FromQuery] int page, int show = 5, string? search = "")
    {
        var result = await subscriptionService.GetAllSubscriptions(page, show, search);
        return Ok(result);

    }
}