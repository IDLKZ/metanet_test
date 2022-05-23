using AutoMapper;
using Metanet.Server.Database;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;

namespace Metanet.Server.Services
{
    public interface IUserService
    {
        public Task<ServiceResponse<PaginationDTO<UserDTO>>> GetAllUsers(int page, int showPage = 10,string? search="");

        public Task<ServiceResponse<UserDTO>> Create(CreateUserDTO createUser);

        public Task<ServiceResponse<bool>> Delete(string Id);

        public Task<ServiceResponse<UserUpdateDTO>> GetUserForUpdate(string Id);

        public Task<ServiceResponse<UserDTO>> Update(string Id,UserUpdateDTO UserUpdate);


        public Task<ServiceResponse<IEnumerable<UserDTO>>> Search(string Search);

        public Task<ApplicationUser> GetUserById(string userId);
    }
}
