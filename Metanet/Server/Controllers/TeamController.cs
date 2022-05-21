using Metanet.Server.Services;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Metanet.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class TeamController : ControllerBase
    {

        private readonly ITeamService teamService;
        public TeamController(ITeamService _teamService)
        {
            teamService = _teamService;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Team>>>> GetAllTeams()
        {
            var response = await teamService.GetAllTeams();
           return  Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ServiceResponse<TeamDTO>>> Get(int Id)
        {
            var response = await teamService.Get(Id);
           return  Ok(response);
        }


        [HttpDelete("{Id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> Delete(int Id)
        {
            var response = await teamService.Delete(Id);
           return  Ok(response);
        }


        [HttpPut("{Id}")]
        public async Task<ActionResult<ServiceResponse<Team>>> Update(int Id,[FromBody] TeamDTO teamDTO)
        {
            var response = await teamService.Update(Id,teamDTO);
           return  Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Team>>> Create([FromBody] TeamCreateDTO teamDTO)
        {
            var response = await teamService.Create(teamDTO);
           return  Ok(response);
        }


    }
}
