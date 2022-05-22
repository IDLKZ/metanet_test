using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;

namespace Metanet.Client.Services.IServices;

public interface IPaymentService
{
    public Task<ServiceResponse<PaymentDTO>> GetPaymentDto(string UserId, string CourseAlias);
    public Task<bool> CreateTransaction(TransactionCreateDTO transactionCreateDto);
    public Task<bool> UpdateTransaction(TransactionUpdateDTO transactionUpdateDto);
    public Task<ServiceResponse<bool>> HasSubscription(string UserId,int CourseId);
    
    public Task<ServiceResponse<ResultFromXml.Result>> GetAllTransactions(string Date_From, string Date_To);
    public Task<PaginationDTO<Transaction>> GetAllLocalTransactions(int page, int show = 5, string? search = "");
    public Task<ResultSingleXml.Result> GetSingleTransaction(string ORDER);

}