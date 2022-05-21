using Metanet.Server.Database;
using Metanet.Server.Helpers;
using Metanet.Shared;
using Metanet.Shared.Auth;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Metanet.Shared.Models;
using Metanet.Server.Services;
using System.Web;

namespace Metanet.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly APISettings _APISettings;
        private IMailService mailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(
            SignInManager<ApplicationUser> signIn,
            UserManager<ApplicationUser> user,
            RoleManager<Role> role,
            IOptions<APISettings> api,
            IMailService _mail,
            IHttpContextAccessor httpContextAccessor
            )

        {
            signInManager = signIn;
            userManager = user;
            roleManager = role;
            _APISettings = api.Value;
            mailService = _mail;
            _httpContextAccessor = httpContextAccessor;

        }
        //Register Start
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] RegisterDTO registerDTO)
        {
            if(registerDTO == null || !ModelState.IsValid)
            {
               
                if (!ModelState.IsValid)
                {
                    return Ok(new CommonResponse
                    {
                        Success = false,
                        Message = "Введены неверные данные",
                        StatusCode = 400,
                        ErrorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e=>e.ErrorMessage))
                });
                }
                return Ok(new CommonResponse
                {
                    Success = false,
                    Message = "Введены неверные данные",
                    StatusCode = 400

                });
            }
            var existedByEmail = await userManager.FindByEmailAsync(registerDTO.Email);
            var existedByPhone =  userManager.Users.FirstOrDefault(u=>u.PhoneNumber == registerDTO.PhoneNo);
            var userNameExisted = await userManager.FindByNameAsync(registerDTO.UserName);
            if (userNameExisted != null)
            {
                ModelState.AddModelError("Email", "Такая почта уже используется");
                return Ok(new CommonResponse
                {
                    Success = false,
                    Message = "Такой логин уже используется",
                    StatusCode = 400,
                    ErrorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            if (existedByEmail != null)
            {
                ModelState.AddModelError("Email","Такая почта уже используется");
                return Ok(new CommonResponse
                {
                    Success = false,
                    Message = "Такая почта уже используется",
                    StatusCode = 400,
                    ErrorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

           if(existedByPhone != null)
            {
                ModelState.AddModelError("Phone", "Такой телефон уже используется");
                return Ok(new CommonResponse
                {
                    Success = false,
                    Message = "Такая телефон уже используется",
                    StatusCode = 400,
                    ErrorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }


            var user = new ApplicationUser
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                Name = registerDTO.Name,
                PhoneNumber = registerDTO.PhoneNo,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                return Ok(new CommonResponse
                {
                    Success = false,
                    Message="Ошибка при регистрации",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessages = result.Errors.Select(e=>e.Description)
                });
            }
            var roleResult = await userManager.AddToRoleAsync(user,Constants.Customer);
            if (!roleResult.Succeeded)
            {
                return Ok(new CommonResponse
                {
                    Success = false,
                    Message = "Ошибка при регистрации",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessages = roleResult.Errors.Select(e => e.Description)
                });
            }
            return Ok(new CommonResponse{
                StatusCode = StatusCodes.Status201Created,
                Success = true,
                Message = "Успешно зарегистрировались"
            });


        }
        //Register End

        //Login Start
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] LoginDTO loginDTO)
        {
            var user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return Ok(
                        new CommonResponse
                        {
                            Success = false,
                            Message = "Пользователь не найден",
                            StatusCode = StatusCodes.Status404NotFound
                        }
                    );
            }

            else
            {
                var result = await signInManager.PasswordSignInAsync(user.UserName, loginDTO.Password, false, false);

                if (result.Succeeded)
                {
                    var credentials = GetSigningCredentials();
                    var claims = await GetClaims(user);

                    var tokenOptions = new JwtSecurityToken(
                        issuer: _APISettings.ValidIssuer,
                        audience: _APISettings.ValidAudience,
                        claims: claims,
                        expires: DateTime.Now.AddDays(30),
                        signingCredentials: credentials);

                    var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    return Ok(new CommonResponse
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        Token = token,
                        User = new UserDTO
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            ImageUrl = user.ImageUrl,
                            Description = user.Description
                        }
                    }
                    );
                }

                else
                {
                    return Unauthorized(new CommonResponse
                    {
                        Success = false,
                        Message = "Неверный Email или Пароль",
                        StatusCode = StatusCodes.Status401Unauthorized,
                    });
                }

            }

        }
        //Login End


        //Send Reset Token
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> SendResetToken([FromBody] ForgetDTO forget)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(forget.Email);
                if (user != null)
                {
                    var code = await userManager.GeneratePasswordResetTokenAsync(user);
                    code = HttpUtility.UrlEncode(code);
                    string url = $"{Request.Scheme}://{Request.Host.Value}/reset?token={code}&identify={user.Email}";
                    var mail = new Shared.DTO.MailRequest();
                    mail.Subject = "Восстановление пароля";
                    mail.Body = $"<a href={url}>{url}</a>";
                    mail.Emails = new List<string>{ user.Email.ToString()};
                   await mailService.SendEmailAsync(mail);
                       

                }
                return new ServiceResponse<bool>
                {
                    Message = "Успешно отправлено сообщение на почту со ссылкой на сброс пароля",
                    Success = true,
                    Data = true,
                    StatusCode = 200
                };
            }
            catch(Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Message = "Сервис временно не доступен попробуйте позже",
                    Success = false,
                    Data = false,
                    StatusCode = 500
                };
            }
            
        }


        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> ResetPassword([FromBody] Shared.DTO.ResetDTO reset)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(reset.UserId);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, reset.Token, reset.Password);
                    if (result.Succeeded)
                    {
                        return new ServiceResponse<bool>
                        {
                            Message = "Пароль успешно обновлен",
                            Success = true,
                            Data = true,
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        return new ServiceResponse<bool>
                        {
                            Message = "Срок действия письма истек",
                            Success = false,
                            Data = false,
                            StatusCode = 400
                        };
                    }
                }
                return new ServiceResponse<bool>
                {
                    Message = "Срок действия ключа истек",
                    Success = false,
                    Data = false,
                    StatusCode = 400
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Message = "Сервис временно не доступен попробуйте позже",
                    Success = false,
                    Data = false,
                    StatusCode = 500
                };
            }

        }


        //Some Helpers
        private SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_APISettings.SecretKey));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim("Id",user.Id),
                new Claim("PhoneNumber",user.PhoneNumber),
                new Claim("UserName",user.UserName),
                
            };

            var roles = await userManager.GetRolesAsync(await userManager.FindByEmailAsync(user.Email));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

    }
}
