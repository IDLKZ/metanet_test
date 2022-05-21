using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;

namespace Metanet.Server.Services
{
    public interface IMaterialService
    {

        public Task<ServiceResponse<Material>> CreateMaterial(MaterialCreateDTO model);

        public Task<ServiceResponse<Material>> UpdateMaterial(int Id, MaterialDTO model);

        public Task<ServiceResponse<MaterialDTO>> GetMaterial(int Id);

        public Task<ServiceResponse<List<Material>>> GetAllMaterials(int LessonId);

        public Task<ServiceResponse<bool>> DeleteMaterial(int Id);





    }
}
