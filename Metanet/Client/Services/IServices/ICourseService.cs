using Metanet.Shared.DTO;
using Metanet.Shared.Models;

namespace Metanet.Client.Services.IServices
{
    public interface ICourseService
    {

        public Task<List<Course>> GetAllCourses();

        public Task<bool> CreateCourse(CourseCreateDTO course);

        public Task<CourseDTO> GetCourseByAlias(string Alias);

        public Task<bool> UpdateCourse(int Id,CourseDTO course);

    }
}
