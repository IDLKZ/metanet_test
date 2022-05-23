using Metanet.Shared.DTO;
using Metanet.Shared.Models;

namespace Metanet.Client.Services.IServices
{
    public interface IHomeService
    {
        //Вся команда
        public Task<List<Team>> GetAllTeams();

        //Получить тот самый курс
        public Task<Course> Course();
        //Страница одного курса
        public Task<Course> GetCourseByAlias(string Alias);
        //Все блоки
        public Task<List<Blog>> GetAllBlogs();
        //Получить блок по курсу
        public Task<List<Blog>> GetBlogByCourseId(int CourseId);
        //Блок по ссылке
        public Task<Blog> GetBlogByAlias(string Alias);
        public Task<Blog> GetBlogById(int Id);
        //Все уроки по ссылке
        public Task<List<Lesson>> GetLessonByBlogId(int BlogId);
        //Урок по ссылке
        public Task<Lesson> GetLessonByAlias(string Alias);

        public Task<CurrencyDTO> GetCurrency();

        public Task<StatsDTO> GetStats();

        public Task<bool> CheckPay();
    }
}
