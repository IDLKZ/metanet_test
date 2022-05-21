using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;

namespace Metanet.Server.Services
{
    public interface IHomeService

    {
        //Вся команда
        public Task<ServiceResponse<List<Team>>> GetAllTeams();

        //Получить тот самый курс
        public Task<ServiceResponse<Course>> GetCourse();
        //Страница одного курса
        public Task<ServiceResponse<Course>> GetCourseByAlias(string Alias);
        //Все блоки
        public Task<ServiceResponse<List<Blog>>> GetAllBlogs();
        //Получить блок по курсу
        public Task<ServiceResponse<List<Blog>>> GetBlogByCourseId(int CourseId);
        //Блок по ссылке
        public Task<ServiceResponse<Blog>> GetBlogByAlias(string Alias);
        public Task<ServiceResponse<Blog>> GetBlogById(int Id);
        //Все уроки по ссылке
        public Task<ServiceResponse<List<Lesson>>> GetLessonByBlogId(int BlogId);
        //Урок по ссылке
        public Task<ServiceResponse<Lesson>> GetLessonByAlias(string Alias);
        public Task<ServiceResponse<StatsDTO>> GetStats();




    }
}
