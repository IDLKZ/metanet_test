using Blazored.LocalStorage;
using Metanet.Client.Services.Auth;
using Metanet.Client.Services.IServices;
using Metanet.Shared;
using Metanet.Shared.Auth;
using Metanet.Shared.DTO;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Sotsera.Blazor.Toaster;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Metanet.Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private ILocalStorageService localStorage;
        private HttpClient httpClient;
        private readonly AuthenticationStateProvider authState;
        private IToaster Toaster;

        public AuthenticationService(ILocalStorageService local, HttpClient http, AuthenticationStateProvider authenticationState,IToaster _Toaster)
        {
            localStorage = local;
            httpClient = http;
            authState = authenticationState;
            Toaster = _Toaster;

        }

        

        public async Task<CommonResponse> SignIn(LoginDTO loginDTO)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/signin", loginDTO);
            var result = await response.Content.ReadFromJsonAsync<CommonResponse>();
           
            if (result.Success == true)
            {
                await localStorage.SetItemAsync(Constants.Token, result.Token);
                await localStorage.SetItemAsync(Constants.User, result.User);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
                ((AuthState)authState).NotifyUserLoggedIn(result.Token);
                return new CommonResponse { Success = true, Message = "Добро пожаловать", StatusCode = 200 };
            }
            else
            {
                return result;
            }

        }

        public async Task<CommonResponse> SignUp(RegisterDTO registerDTO)
        {

            var response = await httpClient.PostAsJsonAsync("api/auth/signup", registerDTO);
            var result = await response.Content.ReadFromJsonAsync<CommonResponse>();
            if (result.Success)
            {

                return new CommonResponse { Success = true, Message = "Успешно Зарегистрировались", StatusCode = 200 };
            }
            else
            {
                return result;
            }
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync(Constants.Token);
            await localStorage.RemoveItemAsync(Constants.User);
            httpClient.DefaultRequestHeaders.Authorization = null;
            ((AuthState)authState).NotifyUserLogout();
        }

        public async Task<ServiceResponse<bool>> SendResetToken(ForgetDTO forgetDTO)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/SendResetToken", forgetDTO);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            if (result.Success)
            {
                Toaster.Success(result.Message ?? "Письмо со сбросом успешно отправлено на почту");
            }
            else
            {
                Toaster.Error(result.Message ?? "Упс что-то пошло не так");
            }
            return result;

        }

        public async Task<ServiceResponse<bool>> ResetPassword(ResetDTO resetDTO)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/ResetPassword", resetDTO);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            if (result.Success)
            {
                Toaster.Success(result.Message ?? "Пароль успешно сброшен");
            }
            else
            {
                Toaster.Error(result.Message ?? "Ключ недействителен или проверьте правильность ссылки");
            }
            return result;
        }
    }
}
