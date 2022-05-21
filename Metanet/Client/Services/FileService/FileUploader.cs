using Metanet.Client.Helpers;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Sotsera.Blazor.Toaster;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Metanet.Client.Services.FileService
{
    public class FileUploader : IFileUploader
    {
        private readonly IJSRuntime jsRuntime;
        private readonly HttpClient Http;
        private IToaster Toaster;
        public FileUploader(HttpClient _httpClient,IJSRuntime runtime, IToaster _Toaster)
        {
            Http = _httpClient;
            jsRuntime = runtime;
            Toaster = _Toaster;
        }


        public async Task<string> UploadOneFile(InputFileChangeEventArgs e,string[] ResolvedFormat, int MaxSizeInMb = 20, string oldUrl = "")
        {
            var selectedFiles = e.GetMultipleFiles();
            var filename = e.File.Name;
            FileInfo fi = new FileInfo(e.File.Name);
            
            if((ResolvedFormat.Contains(fi.Extension.ToLower()) || ResolvedFormat.Count() == 0))
            {
                var fileContent = new StreamContent(e.File.OpenReadStream(maxAllowedSize: MaxSizeInMb * 1048576));
                using var content = new MultipartFormDataContent();
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(e.File.ContentType);
                content.Add(content: fileContent, name: "file", fileName: filename);
                var response = await Http.PostAsync("/api/FileUploader/FileUpload", content);
                var result = await response.Content.ReadAsStringAsync();
                var resultResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(result);
                if (response.IsSuccessStatusCode)
                {
                    return resultResponse.Data;
                }
                else
                {
                    Toaster.Warning("При загрузке файлов что-то пошло не так, попробуйте позже");
                    return oldUrl;
                }
            }
            else
            {
                if(!ResolvedFormat.Contains(fi.Extension.ToLower()) || ResolvedFormat.Count() != 0)
                {
                    Toaster.Warning($"Файл должен иметь форматы:{string.Join(",", ResolvedFormat)}");
                }
                else if(fi.Length / 1048576 > MaxSizeInMb)
                {
                    Toaster.Warning($"Превышен размер допустимого файла, ваш файл - {fi.Length/ 1048576} Mb , допустимо {MaxSizeInMb} Mb");
                }
                else
                {
                    Toaster.Warning($"Файл должен иметь форматы:{string.Join(",", ResolvedFormat)}, а также размер - {MaxSizeInMb} Mb");
                }
                
                return oldUrl;
            }
            
        }


        public async Task<string> DeleteFile(string ImageUrl)
        {
            if (!string.IsNullOrEmpty(ImageUrl))
            {
                var result = await Http.GetFromJsonAsync<ServiceResponse<bool>>("/api/FileUploader/DeleteFile?path="+ImageUrl);
                if (result.Success == true)
                {
                    return "";
                }

                return ImageUrl;
            }
            else
            {
                return ImageUrl;
            }
        }

    }
}
