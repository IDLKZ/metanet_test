using AutoMapper;
using Metanet.Server.Database;
using Metanet.Shared;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Metanet.Server.Services
{
    public class UserService : IUserService
    {

        private readonly ApplicationDbContext applicationDb;
        IMapper _mapper;
        private readonly UserManager<ApplicationUser> userManager;
       
		public UserService(ApplicationDbContext _db, IMapper mp, UserManager<ApplicationUser> _userManager)
		{
			applicationDb = _db;
			_mapper = mp;
			userManager = _userManager;

             


        }

		public async Task<ServiceResponse<PaginationDTO<UserDTO>>> GetAllUsers(int page, int showPage = 10,string? search = "")

        {

            
            var query = _mapper.ProjectTo<UserDTO>(applicationDb.Users.Where(u=> EF.Functions.Like(u.Name, "%" + search + "%") || EF.Functions.Like(u.Email, "%" + search + "%") || EF.Functions.Like(u.UserName, "%" + search + "%") || EF.Functions.Like(u.PhoneNumber, "%" + search + "%")).Include(u => u.UserRoles).ThenInclude(r => r.Role));
            var users = new PageResult<UserDTO>(query, page, showPage);
            users.Data = users.Data.OrderByDescending(u => u.CreatedAt);
            var content = _mapper.Map <PaginationDTO<UserDTO>> (users);
            
            return new ServiceResponse<PaginationDTO<UserDTO>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = content
            };
        }

		public async Task<ServiceResponse<UserDTO>> Create(CreateUserDTO createUser)
		{
            var existedEmail = await userManager.FindByEmailAsync(createUser.Email);
            var existedName = await userManager.FindByNameAsync(createUser.UserName);
            var existedPhone =  applicationDb.Users.FirstOrDefault(u => u.PhoneNumber == createUser.PhoneNumber);

            if (existedName != null)
            {
                return new ServiceResponse<UserDTO>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Такой логин уже используется"
                };
            }
            if (existedEmail != null)
			{
                return new ServiceResponse<UserDTO>
				{
                    Success = false,
                    StatusCode = 400,
                    Message = "Такая почта уже используется"
				};
			}
            if (existedPhone != null)
            {
                return new ServiceResponse<UserDTO>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Такой телефон уже используется"
                };
            }
			try
			{
                var user = new ApplicationUser
                {
                    Email = createUser.Email,
                    UserName = createUser.UserName,
                    Name = createUser.Name,
                    PhoneNumber = createUser.PhoneNumber,
                    ImageUrl = createUser.ImageUrl,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow

                };
                var result = await userManager.CreateAsync(user, createUser.Password);

				if (!result.Succeeded)
				{
                    return new ServiceResponse<UserDTO>
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = "Что-то пошло не так"
                    };
                }
                var roleResult = await userManager.AddToRoleAsync(user, createUser.Role);
                if (!roleResult.Succeeded)
                {
                    return new ServiceResponse<UserDTO>
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = "Что-то пошло не так"
                    };
                }
                var userExisted =  applicationDb.Users.FirstOrDefault(u=> u.Email==createUser.Email);
                return new ServiceResponse<UserDTO>
                {
                    Success = true,
                    StatusCode = 201,
                    Message = "Новый пользователь успешно создан",
                    Data = _mapper.Map<UserDTO>(userExisted)
                };
            }
            catch (Exception ex)
			{
                return new ServiceResponse<UserDTO>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = $"Что-то пошло не так {ex.Message}"
                };
            }
        }

		public async Task<ServiceResponse<bool>> Delete(string Id)
		{
            var user = await applicationDb.Users.FindAsync(Id);
            if(user != null)
			{
                applicationDb.Remove(user);
                applicationDb.SaveChanges();
                return new ServiceResponse<bool>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Успешно удалено"
                };
            }
            return new ServiceResponse<bool>
            {
                Success = false,
                StatusCode = 404,
                Message = "Пользователь не найден"
            };
        }

        public async Task<ServiceResponse<UserUpdateDTO>> GetUserForUpdate(string Id)
		{
            ApplicationUser? user = await applicationDb.Users.Include(u => u.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync(u=> u.Id == Id);
            if(user!= null)
			{
                var existed = _mapper.Map<UserUpdateDTO>(user);
				if (user.UserRoles.Count() > 0)
				{
                    existed.Role = user.UserRoles.First().Role.Name;
				}
                return new ServiceResponse<UserUpdateDTO>
                {
                    Success = true,
                    StatusCode = 200,
                    Data = existed
                };
			}
            return new ServiceResponse<UserUpdateDTO>
            {
                Success = false,
                StatusCode = 404,
                Message = "Пользователь не найден"
            };
        }

		public async Task<ServiceResponse<UserDTO>> Update(string Id,UserUpdateDTO UserUpdate)
		{
            ApplicationUser applicationUser = await userManager.FindByIdAsync(Id);
            if (UserUpdate != null && !string.IsNullOrEmpty(Id))
			{

                if (applicationUser.Id == UserUpdate.Id)
                {
                    var existedName = await userManager.FindByNameAsync(UserUpdate.UserName);
                    var existedPhone = applicationDb.Users.FirstOrDefault(u => u.PhoneNumber == UserUpdate.PhoneNumber);
                    string Result = "Успешно обновлены: данные пользователя, ";
                    if (existedName != null && existedName.Id != UserUpdate.Id)
                    {
                        return new ServiceResponse<UserDTO>
                        {
                            Success = false,
                            StatusCode = 400,
                            Message = "Такой логин уже используется"
                        };
                    }
                    if (existedPhone != null && existedPhone.Id != UserUpdate.Id)
                    {
                        return new ServiceResponse<UserDTO>
                        {
                            Success = false,
                            StatusCode = 400,
                            Message = "Такой телефон уже используется"
                        };
                    }
                    applicationUser.UserName = UserUpdate.UserName;
                    applicationUser.Name = UserUpdate.Name;
                    applicationUser.PhoneNumber = UserUpdate.PhoneNumber;
                    applicationUser.ImageUrl = UserUpdate.ImageUrl;
                    applicationUser.Description = UserUpdate.Description;
                    applicationUser.UpdatedAt = DateTime.UtcNow;

                    var result = await userManager.UpdateAsync(applicationUser);

                    if (!result.Succeeded)
                    {
                        return new ServiceResponse<UserDTO>
                        {
                            Success = false,
                            StatusCode = 400,
                            Message = $"Что-то пошло не так {result.Errors}"
                        };
                    }
                    if(!string.IsNullOrEmpty(UserUpdate.Password) && !string.IsNullOrEmpty(UserUpdate.ConfirmPassword))
                    {
                        string strRegex = @"(^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$)";
                        Regex re = new Regex(strRegex);
                        if (re.IsMatch(UserUpdate.Password) && UserUpdate.Password == UserUpdate.ConfirmPassword)
                           {
                                var token = await userManager.GeneratePasswordResetTokenAsync(applicationUser);
                                await userManager.ResetPasswordAsync(applicationUser, token, UserUpdate.Password);
                                Result += " пароль обновлен, ";

                            }
                          else
                            {
                               Result += " пароль не изменен т.к пароль должен содержать более 8 знаков мелкого и верхнего регистра(A-Z a-z),спец знака(@_-) и цифр(0-9), ";
                             }
                    }
                    

                    if (Constants.Roles.Contains(UserUpdate.Role))
                    {
                        var roles = await userManager.GetRolesAsync(applicationUser);
                        await userManager.RemoveFromRolesAsync(applicationUser, roles.ToArray());
                        var resultRole = await userManager.AddToRoleAsync(applicationUser, UserUpdate.Role);
                        Result += " роль обновлена";
                    }
                    
                    return new ServiceResponse<UserDTO>
                    {
                        Success = true,
                        StatusCode = 200,
                        Message = Result
                    };





                }
                return new ServiceResponse<UserDTO>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Пользователь не найден"
                };
            }

            return new ServiceResponse<UserDTO>
            {
				Success = false,
                StatusCode = 400,
                Message = "Неверный запрос"
            };

        }

        public async Task<ServiceResponse<IEnumerable<UserDTO>>> Search(string Search)
        {
          
               var users = await applicationDb.Users.Include(u => u.UserRoles).ThenInclude(r => r.Role).Where(f => EF.Functions.Like(f.Name, "%"+Search+"%") || EF.Functions.Like(f.Email, "%" + Search + "%") || EF.Functions.Like(f.UserName, "%" + Search + "%")).ToListAsync();
            var usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users);
            return new ServiceResponse<IEnumerable<UserDTO>>
            {
                Success = true,
                StatusCode = 200,
                Data = usersDTO
            };
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var user = await applicationDb.Users.FindAsync(userId);
            return user;
        }
    }
}
