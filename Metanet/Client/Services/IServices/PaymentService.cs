using System.Text;
using Metanet.Shared.DTO;
using Metanet.Shared.Helpers;
using System.Net.Http.Json;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Sotsera.Blazor.Toaster;


namespace Metanet.Client.Services.IServices;

public class PaymentService : IPaymentService
{
    private readonly HttpClient _http;
    protected IToaster _toaster;
    public PaymentService(HttpClient http,IToaster toaster)
    {
        _http = http;
        _toaster = toaster;
    }
    public async Task<ServiceResponse<PaymentDTO>> GetPaymentDto(string UserId,string CourseAlias)
    {
        return await _http.GetFromJsonAsync<ServiceResponse<PaymentDTO>>($"api/payment/GetPaymentInfo/{UserId}/{CourseAlias}");
    }

    public async Task<bool> CreateTransaction(TransactionCreateDTO transactionCreateDto)
    {
        var response =  await _http.PostAsJsonAsync<TransactionCreateDTO>($"api/payment/CreateTransaction",transactionCreateDto);
        var resutl = await response.Content.ReadFromJsonAsync<bool>();
        return resutl;
    }
    public async Task<bool> UpdateTransaction(TransactionUpdateDTO transactionUpdateDto)
    {
        var response =  await _http.PostAsJsonAsync<TransactionUpdateDTO>($"api/payment/UpdateTransaction",transactionUpdateDto);
        var resutl = await response.Content.ReadFromJsonAsync<bool>();
        if (resutl == false)
        {
            _toaster.Error("Что-то пошло не так! Обратитесь в службу поддержки");
        }
        return resutl;
    }

    public async Task<ServiceResponse<bool>>  HasSubscription(string UserId,int CourseId)
    {
        var result =  await _http.GetFromJsonAsync<ServiceResponse<bool>>($"api/payment/HasSubscription/{UserId}/{CourseId}");
        Console.WriteLine(result.Data);
        return result;
    }
    
    public async Task<ServiceResponse<ResultFromXml.Result>> GetAllTransactions(string Date_From, string Date_To)
    {
        var result =  await _http.GetFromJsonAsync<ServiceResponse<ResultFromXml.Result>>($"api/Payment/GetAllTransactions/{Date_From}/{Date_To}");
        return result;
    }

    public async Task<PaginationDTO<Transaction>> GetAllLocalTransactions(int page, int show = 5, string? search = "")
    {
        var result =  await _http.GetFromJsonAsync<ServiceResponse<PaginationDTO<Transaction>>>($"api/Payment/GetAllLocalTransactions?page={page}&show={show}&search={search}");
        if (!result.Success)
        {
            _toaster.Warning(result.Message ?? "Что-то пошло не так");
        }
        return result.Data;
    }


    public async Task<ResultSingleXml.Result> GetSingleTransaction(string ORDER)
    {
        var result =  await _http.GetFromJsonAsync<ServiceResponse<ResultSingleXml.Result>>($"api/Payment/GetTransaction/{ORDER}");
        if (!result.Success)
        {
            _toaster.Warning(result.Message ??"Что-то пошло не так");
        }

        return result.Data;

    }
}