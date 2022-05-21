using Metanet.Shared.DTO;
using Metanet.Shared.Models;

namespace Metanet.Client.Services.IServices
{
    public interface ITeamService
    {
        public Task<List<Team>> GetAllTeams();
        public Task<TeamDTO> Get(int Id);

        public Task<bool> Create(TeamCreateDTO teamCreateDTO);

        public Task<bool> Update(int Id, TeamDTO teamDTO);

        public Task<bool> Delete(int Id);
    }
}
