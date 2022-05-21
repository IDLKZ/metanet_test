using Microsoft.AspNetCore.Components.Forms;

namespace Metanet.Client.Services.FileService
{
    public interface IFileUploader
    {
        public Task<string> UploadOneFile(InputFileChangeEventArgs e, string[] ResolvedFormat,int MaxSizeInMb = 20,string oldUrl ="");
        public Task<string> DeleteFile(string ImageUrl);
    }
}
