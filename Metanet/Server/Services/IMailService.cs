using Metanet.Shared.DTO;
using Metanet.Shared.ResponsesDTO;

namespace Metanet.Server.Services
{
    public interface IMailService
    {
        Task<ServiceResponse<bool>> SendEmailAsync(MailRequest mailRequest);
    }
}
