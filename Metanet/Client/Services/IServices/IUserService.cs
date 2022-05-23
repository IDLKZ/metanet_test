using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;

namespace Metanet.Client.Services.IServices
{
    public interface IUserService
   {
        public Task<PaginationDTO<UserDTO>> GetAllUsers(int page, int showPage = 10,string? search = "");
        public Task<UserDTO> Create(CreateUserDTO userDTO);
        public Task<bool> Delete(string Id);

        public Task<UserUpdateDTO> GetForUpdate(string Id);
        public Task<bool> Update(string Id,UserUpdateDTO userUpdate);
        public Task<IEnumerable<UserDTO>> Search(string Search);
        public Task<ApplicationUser> GetUserById(string userId);
   }
}
