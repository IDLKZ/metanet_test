using AutoMapper;
using Metanet.Server.Database;
using Metanet.Server.Services;
using Metanet.Shared.DTO;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;
using System.Security.Principal;
using Metanet.Shared.Models;

namespace Metanet.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        IUserService UserService;
        public UserController(IUserService userService)
        {
            UserService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<PaginationDTO<UserDTO>>>> GetAllUsers([FromQuery] int page, int show = 5, string? search = "")
        {
            var result = await UserService.GetAllUsers(page, show,search);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> Create([FromBody]CreateUserDTO userCreate)
        {
            var result = await UserService.Create(userCreate);
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> Delete(string Id)
        {
            var result = await UserService.Delete(Id);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ServiceResponse<UserUpdateDTO>>> GetForUpdate(string Id)
        {
            
            var result = await UserService.GetUserForUpdate(Id);
            return Ok(result);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> Update(string Id,[FromBody] UserUpdateDTO UserUpdate)
        {
            var result = await UserService.Update(Id,UserUpdate);
            return Ok(result);
        }

        [HttpGet("{Search}")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<UserDTO>>>> Search(string Search)
        {
            var result = await UserService.Search(Search);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<ApplicationUser>> GetUserById(string userId)
        {
            var result = await UserService.GetUserById(userId);
            return Ok(result);
        }

    }
}
