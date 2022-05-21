using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;

namespace Metanet.Server.Services
{
    public interface ICourseService
    {
        public Task<ServiceResponse<Course>> CreateCourse(CourseCreateDTO model);

        public Task<ServiceResponse<Course>> UpdateCourse(int Id,CourseDTO model);
        public Task<ServiceResponse<CourseDTO>> GetCourseByAlias(string Alias);

        public Task<ServiceResponse<List<Course>>> GetAllCourses();





    }
}
