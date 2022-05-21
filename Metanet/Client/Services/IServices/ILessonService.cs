using Metanet.Shared.DTO;
using Metanet.Shared.Models;

namespace Metanet.Client.Services.IServices
{
    public interface ILessonService
    {
        public Task<List<Lesson>> GetAllLesson();

        public Task<Lesson> GetLesson(int Id);

        public Task<LessonDTO> GetLessonByAlias(string Alias);

        public Task<Lesson> Create(LessonCreateDTO lesson);

        public Task<Lesson> Update (int Id, LessonDTO lesson);

        public Task<bool> Delete(int Id);



    }
}
