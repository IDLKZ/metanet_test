using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Metanet.Server.Database;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using EF = Microsoft.EntityFrameworkCore.EF;

namespace Metanet.Server.Services;

public class SubscriptionService : ISubscriptionService
{
    private ApplicationDbContext dbContext;
    private IMapper mapper;
    public SubscriptionService(ApplicationDbContext _dbContext,IMapper _mapper)
    {
        dbContext = _dbContext;
        mapper = _mapper;
    }
    
    public async Task<ServiceResponse<bool>> Create(SubscriptionCreateDTO subscriptionCreateDto)
    {
        try
        {
            Subscription subscription = mapper.Map<Subscription>(subscriptionCreateDto);
            await dbContext.Subscriptions.AddAsync(subscription);
            await dbContext.SaveChangesAsync();
            return new ServiceResponse<bool>()
            {
                Success = true,
                Data = true,
            };
        }
        catch (Exception ex)
        {
            Subscription subscription = mapper.Map<Subscription>(subscriptionCreateDto);
            await dbContext.Subscriptions.AddAsync(subscription);
            await dbContext.SaveChangesAsync();
            return new ServiceResponse<bool>()
            {
                Success = false,
                Message = "Что-то пошло не так:" + ex.Message,  
                Data = false,
            };
        }
        
    }

    public async Task<ServiceResponse<bool>> Update(int Id,SubscriptionUpdateDTO subscriptionUpdateDto)
    {
        if (Id == subscriptionUpdateDto.ID)
        {
            Subscription subscription = await dbContext.Subscriptions.FindAsync(Id);
            if (subscription != null)
            {
                subscription = mapper.Map<Subscription>(subscriptionUpdateDto);
                 dbContext.Subscriptions.Update(subscription);
                 await dbContext.SaveChangesAsync();
                 return new ServiceResponse<bool>()
                 {
                     Success = true,
                     Data = true,
                 };
                 
            }
            return new ServiceResponse<bool>()
            {
                Success = false,
                StatusCode = 400,
                Message = "Неверный запрос",
                Data = false,
            };
            
        }
        else
        {
            return new ServiceResponse<bool>()
            {
                Success = false,
                StatusCode = 400,
                Message = "Неверный запрос",
                Data = false,
            };
        }
    }

    public async Task<ServiceResponse<Subscription>> GetById(int Id)
    {
        Subscription subscription = await dbContext.Subscriptions.Include(s=>s.User).Include(c=>c.Course).FirstOrDefaultAsync(s=>s.ID==Id);
        if (subscription != null)
        {
            return new ServiceResponse<Subscription>()
            {
                Success = true,
                Data = subscription,
            };
                 
        }
        return new ServiceResponse<Subscription>()
        {
            Success = false,
            StatusCode = 400,
            Message = "Неверный запрос",
            
        };
    }

    public async Task<ServiceResponse<PaginationDTO<Subscription>>> GetAllSubscriptions(int page, int show = 5, string? search = "")
    {
        var query = dbContext.Subscriptions.Include(u => u.User).Include(c=>c.Course);

        var subscription = new PageResult<Subscription>(query, page, show);
        var content = mapper.Map <PaginationDTO<Subscription>> (subscription);

        var result = new ServiceResponse<PaginationDTO<Subscription>>
        {
            Success = true,
            StatusCode = StatusCodes.Status200OK,
            Data = content
        };
        return result;
    }
}