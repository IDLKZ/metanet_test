using Metanet.Shared.DTO;
using Metanet.Shared.ResponsesDTO;

namespace Metanet.Client.Services.IServices
{
    public interface IMailService
    {

        public Task<ServiceResponse<bool>> SendEmail(MailRequest mailRequest);
    }
}
