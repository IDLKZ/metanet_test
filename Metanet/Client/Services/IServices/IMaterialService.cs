using Metanet.Shared.DTO;
using Metanet.Shared.Models;

namespace Metanet.Client.Services.IServices
{
    public interface IMaterialService
    {

        public Task<bool> Create(MaterialCreateDTO material);
        public Task<bool> Update(int Id,MaterialDTO material);

        public Task<List<Material>> GetMaterialsByLessonId(int LessonId);
        public Task<MaterialDTO> Get(int Id);

        public Task<bool> Delete(int Id);


    }
}
