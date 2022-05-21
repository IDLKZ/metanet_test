using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;

namespace Metanet.Server.Services
{
    public interface ILessonService
    {
        public Task<ServiceResponse<Lesson>> CreateLesson(LessonCreateDTO model);

        public Task<ServiceResponse<Lesson>> UpdateLesson(int Id,LessonDTO model);

        public Task<ServiceResponse<Lesson>> GetLesson(int Id);

        public Task<ServiceResponse<LessonDTO>> GetLessonByAlias(string Alias);

        public Task<ServiceResponse<List<Lesson>>> GetAllLessons();

        public Task<ServiceResponse<bool>> DeleteLesson(int Id);
    }
}
