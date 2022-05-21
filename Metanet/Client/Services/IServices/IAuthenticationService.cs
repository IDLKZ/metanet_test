using Metanet.Shared.Auth;
using Metanet.Shared.DTO;
using Metanet.Shared.ResponsesDTO;

namespace Metanet.Client.Services.IServices
{
    public interface IAuthenticationService
    {

        Task<CommonResponse> SignUp(RegisterDTO registerDTO);

        Task<CommonResponse> SignIn(LoginDTO loginDTO);

        Task Logout();


        Task<ServiceResponse<bool>> SendResetToken(ForgetDTO forgetDTO);
        Task<ServiceResponse<bool>> ResetPassword(ResetDTO resetDTO);

    }
}
