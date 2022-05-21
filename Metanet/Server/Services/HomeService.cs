using AutoMapper;
using Metanet.Server.Database;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.EntityFrameworkCore;

namespace Metanet.Server.Services
{
    public class HomeService : IHomeService
    {
        ApplicationDbContext dbContext;
        IMapper mapper;
        public HomeService(ApplicationDbContext _dbContext,IMapper _mapper)
        {
            dbContext = _dbContext;
            mapper = _mapper;
        }
        public async Task<ServiceResponse<Course>> GetCourse()
        {
            var existed = dbContext.Courses.FirstOrDefault();
            if(existed != null)
            {
                return new ServiceResponse<Course>
                            {
                                Success = true,
                                StatusCode = StatusCodes.Status200OK,
                                Data = existed
                            };
            }
            
            else
            {
                return new ServiceResponse<Course>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Курсы еще не созданы"
                };
            }
        }

        public async Task<ServiceResponse<List<Blog>>> GetAllBlogs()
        {
            var blogs = await dbContext.Blogs.ToListAsync();
            return new ServiceResponse<List<Blog>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = blogs
            };
        }

        public async Task<ServiceResponse<List<Team>>> GetAllTeams()
        {
            var teams = await dbContext.Teams.ToListAsync();
            return new ServiceResponse<List<Team>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = teams
            };
        }

        public async Task<ServiceResponse<Blog>> GetBlogByAlias(string Alias)
        {
            var existed = await dbContext.Blogs.Include(b=>b.Course).Include(b=>b.Lessons).FirstOrDefaultAsync(b => b.Alias == Alias);
            if (existed != null)
            {
                return new ServiceResponse<Blog>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = existed
                };
            }

            else
            {
                return new ServiceResponse<Blog>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Блог еще не создан"
                };
            }
        }
        public async Task<ServiceResponse<Blog>> GetBlogById(int Id)
        {
            var existed = await dbContext.Blogs.Include(b => b.Course).Include(b => b.Lessons).FirstOrDefaultAsync(b => b.Id == Id);
            if (existed != null)
            {
                return new ServiceResponse<Blog>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = existed
                };
            }

            else
            {
                return new ServiceResponse<Blog>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Блог еще не создан"
                };
            }
        }
        public async Task<ServiceResponse<List<Blog>>> GetBlogByCourseId(int CourseId)
        {
            var blogs = await dbContext.Blogs.Where(b => b.CourseId == CourseId).Include(b=>b.Lessons).ToListAsync();
            return new ServiceResponse<List<Blog>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = blogs
            };
        }

        public async Task<ServiceResponse<Course>> GetCourseByAlias(string Alias)
        {
            var course = await dbContext.Courses.FirstOrDefaultAsync(c => c.Alias == Alias);
            if(course != null)
            {
                return new ServiceResponse<Course>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = course
                };
            }
            else
            {
                return new ServiceResponse<Course>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Курсы еще не созданы"
                };
            }
        }

        public async Task<ServiceResponse<Lesson>> GetLessonByAlias(string Alias)
        {
            var lesson = await dbContext.Lessons.Include(l => l.Blog).Include(l => l.Materials).Include(l=>l.PrevLesson).Include(l => l.NextLesson).FirstOrDefaultAsync(c => c.Alias == Alias);
            if (lesson != null)
            {
                return new ServiceResponse<Lesson>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = lesson
                };
            }

            else
            {
                return new ServiceResponse<Lesson>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Блог еще не создан"
                };
            }
        }

        public async Task<ServiceResponse<List<Lesson>>> GetLessonByBlogId(int BlogId)
        {
            var lessons = await dbContext.Lessons.Include(l => l.Materials).Where(l => l.BlogId == BlogId).OrderBy(l=>l.Number).ToListAsync();
            return new ServiceResponse<List<Lesson>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = lessons
            };
        }

        public async Task<ServiceResponse<StatsDTO>> GetStats()
        {
            StatsDTO stats = new StatsDTO
            {
                Users = dbContext.Users.Count(),
                Course = dbContext.Courses.Count(),
                Blogs = dbContext.Blogs.Count(),
                Lessons = dbContext.Lessons.Count(),
                Materials = dbContext.Materials.Count(),

            };
            return new ServiceResponse<StatsDTO>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = stats
            };
        }
    }
}
