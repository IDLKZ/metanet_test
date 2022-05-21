using AutoMapper;
using Metanet.Server.Database;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.EntityFrameworkCore;

namespace Metanet.Server.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly ApplicationDbContext applicationDb;
        IMapper _mapper;
       

        public MaterialService(ApplicationDbContext _db, IMapper mp)
        {
            applicationDb = _db;
            _mapper = mp;
        }
        public async Task<ServiceResponse<Material>> CreateMaterial(MaterialCreateDTO model)
        {
            if(model == null)
            {
                return new ServiceResponse<Material>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Проверьте верность запроса",
                };
            }
            else
            {
                Material material = _mapper.Map<Material>(model);
                await applicationDb.Materials.AddAsync(material);
                var result = await applicationDb.SaveChangesAsync();
                if(result > 0)
                {
                    return new ServiceResponse<Material>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        Data = material
                    };
                }
                else
                {
                    return new ServiceResponse<Material>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Проверьте верность запроса",
                    };
                }
            }
        }

        public async Task<ServiceResponse<bool>> DeleteMaterial(int Id)
        {
            if (Id == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Проверьте верность запроса",
                };
            }
            else
            {
                Material material = await applicationDb.Materials.FindAsync(Id);
               
                if (material != null)
                {
                     applicationDb.Materials.Remove(material);
                     applicationDb.SaveChanges();
                    return new ServiceResponse<bool>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        Data = true
                    };
                }
                else
                {
                    return new ServiceResponse<bool>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Проверьте верность запроса",
                    };
                }
            }
        }

        public async Task<ServiceResponse<List<Material>>> GetAllMaterials(int LessonId)
        {
            List<Material> materials = await applicationDb.Materials.Where(m => m.LessonId == LessonId).ToListAsync();
            return new ServiceResponse<List<Material>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = materials
            };
        }

        public async Task<ServiceResponse<MaterialDTO>> GetMaterial(int Id)
        {
            Material material = await applicationDb.Materials.FindAsync(Id);
            if(material != null)
            {
                return new ServiceResponse<MaterialDTO>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = _mapper.Map<MaterialDTO>(material)
                };
            }
            return new ServiceResponse<MaterialDTO>
            {
                Success = false,
                StatusCode = StatusCodes.Status404NotFound,
            };
        }

        public async Task<ServiceResponse<Material>> UpdateMaterial(int Id, MaterialDTO model)
        {
            Material material = await applicationDb.Materials.FindAsync(Id);
            if (material != null)
            {
                if(material.Id == model.Id)
                {
                    Material updated = _mapper.Map<Material>(model);
                    applicationDb.Materials.Update(updated);
                    var result = applicationDb.SaveChanges();
                    if(result > 0)
                    {
                        return new ServiceResponse<Material>
                        {
                            Success = true,
                            StatusCode = StatusCodes.Status200OK,
                            Data = updated
                        };
                    }
                    else
                    {
                        return new ServiceResponse<Material>
                        {
                            Success = false,
                            StatusCode = StatusCodes.Status500InternalServerError,
                        };
                    }
                }
                return new ServiceResponse<Material>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }
            return new ServiceResponse<Material>
            {
                Success = false,
                StatusCode = StatusCodes.Status404NotFound,
            };

        }
    }
}
