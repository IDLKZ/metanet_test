using AutoMapper;
using Metanet.Server.Database;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.EntityFrameworkCore;

namespace Metanet.Server.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext applicationDb;
        IMapper _mapper;


        public TeamService(ApplicationDbContext _db, IMapper mp)
        {
            applicationDb = _db;
            _mapper = mp;
        }
        public async Task<ServiceResponse<Team>> Create(TeamCreateDTO teamCreateDTO)
        {
           if(teamCreateDTO == null)
            {
                return new ServiceResponse<Team>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Неверный запрос"
                };
            }
            else
            {
                Team team = _mapper.Map<Team>(teamCreateDTO);
                await applicationDb.Teams.AddAsync(team);
                var result = await applicationDb.SaveChangesAsync();
                if(result > 0)
                {
                    return new ServiceResponse<Team>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        Data = team
                    };
                }
                else 
                {
                    return new ServiceResponse<Team>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Проверьте верность запроса"
                    };
                }
            }
        }

        public async Task<ServiceResponse<bool>> Delete(int Id)
        {
            var existed = await applicationDb.Teams.FindAsync(Id);
            if(existed != null)
            {
                applicationDb.Teams.Remove(existed);
                var result = await applicationDb.SaveChangesAsync();
                return new ServiceResponse<bool>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                };
            }
            else
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Данный пользователь не найден"
                };
            }
        }

        public async Task<ServiceResponse<TeamDTO>> Get(int Id)
        {
            var existed = await applicationDb.Teams.FindAsync(Id);
            if (existed != null)
            {

                return new ServiceResponse<TeamDTO>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = _mapper.Map<TeamDTO>(existed)
                };
            }
            else
            {
                return new ServiceResponse<TeamDTO>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Данный пользователь не найден"
                };
            }
        }

        public async Task<ServiceResponse<List<Team>>> GetAllTeams()
        {
            var teams = await applicationDb.Teams.ToListAsync();
            return new ServiceResponse<List<Team>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = teams
            };
        }

        public async Task<ServiceResponse<Team>> Update(int Id, TeamDTO teamDTO)
        {
            if(Id == teamDTO.Id && teamDTO != null)
            {
                Team team = _mapper.Map<Team>(teamDTO);
                 applicationDb.Teams.Update(team);
                var result = await applicationDb.SaveChangesAsync();
                if (result > 0)
                {
                    return new ServiceResponse<Team>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        Data = team
                    };
                }
                else
                {
                    return new ServiceResponse<Team>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Проверьте верность запроса"
                    };
                }

            }

            return new ServiceResponse<Team>
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Проверьте верность запроса"
            };
        }
    }
}
