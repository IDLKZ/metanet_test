using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Metanet.Server.Database;
using Metanet.Shared.DTO;
using Metanet.Shared.Helpers;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.EntityFrameworkCore;

namespace Metanet.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private IMapper mapper;
        
        public PaymentController(ApplicationDbContext dbContext,IMapper _mapper)
        {
            _dbContext = dbContext;
            mapper = _mapper;
        }
        
        
        [HttpGet("{UserId}/{Alias}")]
        public async Task<ActionResult<ServiceResponse<PaymentDTO>>> GetPaymentInfo(string UserId, string Alias)
        {
            List<int> ORDERS =  await _dbContext.Transactions.Select(t=>t.ORDER).ToListAsync();
            ApplicationUser user = await _dbContext.Users.FindAsync(UserId);
            Subscription subscription = await _dbContext.Subscriptions.Where(s => s.UserId == UserId && s.Status == true).FirstOrDefaultAsync();
            Course course =  _dbContext.Courses.FirstOrDefault(c => c.Alias == Alias);
            if (subscription == null && user != null)
            {
                return Ok(new ServiceResponse<PaymentDTO>()
                {
                    Success = true,
                    StatusCode = 200,
                    Data = PaymentHelper.GetPaymentDto(course,ORDERS,user)
                });
            }
             return Ok(new ServiceResponse<PaymentDTO>()
            {
                Success = false,
                StatusCode = 200,
                Data = null
            });
        }
            //Create Transaction
        [HttpPost]
        public async Task<ActionResult<bool>> CreateTransaction([FromBody] TransactionCreateDTO transactionCreateDto)
        {
            try
            {
                Transaction transaction = mapper.Map<Transaction>(transactionCreateDto);
                await _dbContext.Transactions.AddAsync(transaction);
                var result = await _dbContext.SaveChangesAsync();
                if (result == 1)
                {
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }
        
        //Update Transaction
        
        [HttpPost]
        public async Task<ActionResult<bool>> UpdateTransaction([FromBody] TransactionUpdateDTO transactionUpdateDTO)
        {
            try
            {
                if (PaymentHelper.GetHashedString(PaymentHelper.GetStringAfterPayment(transactionUpdateDTO)) ==
                    transactionUpdateDTO.P_SIGN)
                {
                    Transaction transaction = _dbContext.Transactions.Where(t => t.ORDER == transactionUpdateDTO.ORDER).FirstOrDefault();
                    if (transaction != null)
                    {
                        transaction.AMOUNT = transactionUpdateDTO.AMOUNT;
                        transaction.CURRENCY = transactionUpdateDTO.CURRENCY;
                        transaction.ORDER = transactionUpdateDTO.ORDER;
                        transaction.MPI_ORDER = transactionUpdateDTO.MPI_ORDER;
                        transaction.RRN = transactionUpdateDTO.RRN;
                        transaction.RES_DESC = transactionUpdateDTO.RES_DESC;
                        transaction.P_SIGN = transactionUpdateDTO.P_SIGN;
                        transaction.RES_CODE = transactionUpdateDTO.RES_CODE;
                        if (transaction.RES_CODE == 0)
                        {
                            transaction.Status = 1;
                            Subscription subscription = _dbContext.Subscriptions.Where(s => s.UserId == transaction.UserId && s.CourseID == transaction.CourseId).FirstOrDefault();
                            if (subscription != null)
                            {
                                //обновляем
                                subscription.Status = true;
                                subscription.TransactionId = transaction.ID;
                                _dbContext.Subscriptions.Update(subscription);
                                await _dbContext.SaveChangesAsync();
                            }
                            else
                            { 
                                //создаем
                                subscription = new Subscription() { UserId = transaction.UserId, CourseID = transaction.CourseId,Status = true,TransactionId = transaction.ID};
                                _dbContext.Subscriptions.Add(subscription);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            transaction.Status = -1;
                        }
                        _dbContext.Transactions.Update(transaction);
                       var result = await _dbContext.SaveChangesAsync();
                       return Ok(result == 1);
                    }
                    else
                    {
                        return Ok(false);
                    }
                }
                return Ok(false);
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }


        
        //if user has subscription
        [HttpGet("{UserId}/{CourseId}")]

        public async Task<ActionResult<ServiceResponse<bool>>> HasSubscription(string UserId,int CourseId)
        {
            ApplicationUser user = await  _dbContext.Users.Include(u=>u.UserRoles).ThenInclude(role => role.Role).FirstOrDefaultAsync(u=>u.Id == UserId);
            if (user != null)
            {
                if (user.UserRoles.FirstOrDefault().Role.Name == "Admin")
                {
                    return Ok(new ServiceResponse<bool>()
                    {
                        Success = true,
                        Data = true
                    });
                }
                Subscription subscription = _dbContext.Subscriptions.Where(s => s.UserId == UserId && s.CourseID == CourseId && s.Status == true).FirstOrDefault();
                if (subscription != null)
                {
                    return Ok(new ServiceResponse<bool>()
                    {
                        Success = true,
                        Data = true
                    });
                }
                else
                {
                    return Ok(new ServiceResponse<bool>()
                    {
                        Success = false,
                        Data = false
                    });
                }
            }
            else
            {
                return Ok(new ServiceResponse<bool>()
                {
                    Success = false,
                    Data = false
                });
            }
            
        }
        
        
    }
}
