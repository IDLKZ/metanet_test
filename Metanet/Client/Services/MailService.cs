using Metanet.Client.Services.IServices;
using Metanet.Shared.DTO;
using Metanet.Shared.ResponsesDTO;
using Newtonsoft.Json;
using Sotsera.Blazor.Toaster;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Metanet.Client.Services
{
    public class MailService : IMailService
    {
        private HttpClient httpClient;
        private IToaster Toaster;
        public MailService(HttpClient _client,IToaster _Toaster)
        {
            httpClient = _client;
            Toaster = _Toaster;
        }

        public async Task<ServiceResponse<bool>> SendEmail(MailRequest mailRequest)
        {
            
            try
            {
                if(mailRequest != null)
                {
                    var content = new MultipartFormDataContent();
                    content.Add(content: new StringContent(mailRequest.Subject.ToString()), "Subject");
                    content.Add(content: new StringContent(mailRequest.Body.ToString()), "Body");

                    if (mailRequest.Emails != null)
                    {
                        if(mailRequest.Emails.Count() > 0)
                        {
                            foreach (var email in mailRequest.Emails)
                            {
                                 content.Add(content: new StringContent(email.ToString()), "Emails");
                            }
                        }
                        
                    }

                    if (mailRequest.CurrentFile != null)
                    {
                        if (mailRequest.CurrentFile.Count() > 0)
                        {
                            foreach (var file in mailRequest.CurrentFile)
                            {

                                content.Add(new StreamContent(file.OpenReadStream(maxAllowedSize: 2 * 1048576))
                                {
                                    Headers =
                                {
                                    ContentLength = file.Size,
                                    ContentType = new MediaTypeHeaderValue(file.ContentType)
                                }
                                }, "Attachments", file.Name);


                            }
                        }
                           
                    }
                    var response = await httpClient.PostAsync("api/mail/SendMail", content);
                    var result = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

                    if (result.Success)
                    {
                        Toaster.Success(result.Message ?? "Успешно отправлено!");
                    }
                    else
                    {
                        Toaster.Error(result.Message ?? "Что-то пошло не так!");
                    }
                    return result;
                }
                else
                {
                    Toaster.Error("Проверьте правильность запроса!");
                    return new ServiceResponse<bool>
                    {
                        Success = false
                    };
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine($"oops something went wrong - {ex.ToString()}");
                Toaster.Error ("Что-то пошло не так!");
                return new ServiceResponse<bool>
                {
                    Success = false
                };
            }
            
            

        }
    }
}
