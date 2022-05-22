using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
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


        //GET ALL TRANSACTION
        [HttpGet("{DateFrom}/{DateTo}")]
        public async Task<ActionResult<ServiceResponse<ResultFromXml.Result>>> GetAllTransactions(string DateFrom, string DateTo)
        {
            try
            {
                string endPoint = "https://ecom.jysanbank.kz/ecom/api";
                var client = new HttpClient();
                var data = new[]
                {
                    new KeyValuePair<string, string>("MERCHANT", PaymentHelper.MID),
                    new KeyValuePair<string, string>("TERMINAL", PaymentHelper.TID),
                    new KeyValuePair<string, string>("DATE_FROM", DateFrom),
                    new KeyValuePair<string, string>("DATE_TO", DateTo),
                    new KeyValuePair<string, string>("P_SIGN",
                        PaymentHelper.GetHashedString(PaymentHelper.GetStringForTransaction(DateTo, DateFrom))),
                    new KeyValuePair<string, string>("LANGUAGE", "ru"),
                };
                ResultFromXml.Result scp = null;
                var result = await client.PostAsync(endPoint, new FormUrlEncodedContent(data));
                XmlSerializer serializer = new XmlSerializer(typeof(ResultFromXml.Result));
                using (Stream stream = result.Content.ReadAsStreamAsync().Result)
                {
                    scp = (ResultFromXml.Result) serializer.Deserialize(stream);
                }

                if (scp.Code == 0)
                {
                    return Ok(new ServiceResponse<ResultFromXml.Result>
                    {
                        Success = true,
                        StatusCode = 200,
                        Data = scp
                    });
                }
                else
                {
                    return Ok(new ServiceResponse<ResultFromXml.Result>
                    {
                        Success = false,
                        Message = "Данных нет",
                        StatusCode = 400
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ServiceResponse<ResultFromXml.Result>
                {
                        Success = false,
                        Message = ex.Message,
                        StatusCode = 400
                });
            }
            
        }
        //Get SINGLE TRANSACTION
         
        
        
        //Get Transaction by search

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<PaginationDTO<Transaction>>>> GetAllLocalTransactions([FromQuery] int page, int show = 5, string? search = "")
        {
            var query = _dbContext.Transactions.Where(u=> EF.Functions.Like(u.ORDER, "%" + search + "%") || EF.Functions.Like(u.MPI_ORDER, "%" + search + "%") || EF.Functions.Like(u.RRN, "%" + search + "%") || EF.Functions.Like(u.AMOUNT, "%" + search + "%") || EF.Functions.Like(u.CURRENCY, "%" + search + "%")).Include(u => u.User).Include(c=>c.Course);

            var transactions = new PageResult<Transaction>(query, page, show);
            var content = mapper.Map <PaginationDTO<Transaction>> (transactions);

            var result =  new ServiceResponse<PaginationDTO<Transaction>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = content
            };
            
            
            return Ok(result);
        }
        
        
        
        [HttpGet("{ORDER}")]
        public async Task<ActionResult<ServiceResponse<ResultSingleXml>>> GetTransaction(string ORDER)
        {
            string endPoint = "https://ecom.jysanbank.kz/ecom/api";
            var client = new HttpClient();
            var data = new[] {new KeyValuePair<string, string>("ORDER", ORDER), new KeyValuePair<string, string>("MERCHANT", PaymentHelper.MID), new KeyValuePair<string, string>("GETSTATUS", "1"), new KeyValuePair<string, string>("P_SIGN", PaymentHelper.GetHashedString(PaymentHelper.GetStringForSingleTransaction(ORDER))), new KeyValuePair<string, string>("LANGUAGE", "ru"),};
            ResultSingleXml.Result scp = new ResultSingleXml.Result();
            var result = await client.PostAsync(endPoint, new FormUrlEncodedContent(data));
            XmlSerializer serializer = new XmlSerializer(typeof(ResultSingleXml.Result));
            using (Stream stream = result.Content.ReadAsStreamAsync().Result)
            {
                scp = (ResultSingleXml.Result) serializer.Deserialize(stream);
            }
            if (scp.Code == 0)
            {
                return Ok(new ServiceResponse<ResultSingleXml.Result>()
                {
                    Success = true,
                    StatusCode = 200,
                    Data = scp
                });
            }
            return Ok(new ServiceResponse<ResultSingleXml.Result>()
            {
                Success = false,
                StatusCode = 404,
                Message = "Не найдено",
            });


            
        }

    }
    
    
    
}
