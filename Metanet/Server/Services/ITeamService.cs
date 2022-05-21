using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;

namespace Metanet.Server.Services
{
    public interface ITeamService
    {
        public Task<ServiceResponse<List<Team>>> GetAllTeams();
        public Task<ServiceResponse<TeamDTO>> Get(int Id);

        public Task<ServiceResponse<Team>> Create(TeamCreateDTO teamCreateDTO);

        public Task<ServiceResponse<Team>> Update(int Id,TeamDTO teamDTO);

        public Task<ServiceResponse<bool>> Delete(int Id);




    }
}
