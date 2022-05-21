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
}