using System.Linq;
using AutoMapper;
using Metanet.Server.Database;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.EntityFrameworkCore;
using EF = Microsoft.EntityFrameworkCore.EF;
using Microsoft.EntityFrameworkCore;

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
            if (subscription != null && subscriptionUpdateDto.ID == subscription.ID)
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

    public async Task<ServiceResponse<SubscriptionUpdateDTO>> GetById(int Id)
    {
        Subscription subscription = await dbContext.Subscriptions.Include(s=>s.User).Include(c=>c.Course).Include(t=>t.Transaction).FirstOrDefaultAsync(s=>s.ID == Id);
        if (subscription != null)
        {
            return new ServiceResponse<SubscriptionUpdateDTO>()
            {
                Success = true,
                Data = mapper.Map<SubscriptionUpdateDTO>(subscription),
            };
                 
        }
        return new ServiceResponse<SubscriptionUpdateDTO>()
        {
            Success = false,
            StatusCode = 400,
            Message = "Неверный запрос",
            
        };
    }

    public async Task<ServiceResponse<PaginationDTO<Subscription>>> GetAllSubscriptions(int page, int show = 5, string? search = "")
    {
        var query = dbContext.Subscriptions.Include(s=>s.User).Include(s=>s.Course).Include(t=>t.Transaction);

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

    public async Task<ServiceResponse<Subscription>> GetByUserId(string userId)
    {
        var content = await dbContext.Subscriptions
            .Where(u => u.UserId == userId)
            .Include(c => c.Course)
            .FirstOrDefaultAsync();
        var result = new ServiceResponse<Subscription>
        {
            Success = true,
            StatusCode = StatusCodes.Status200OK,
            Data = content
        };
        return result;
    }
}