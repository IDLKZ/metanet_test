using AutoMapper;
using Metanet.Server.Database;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SlugGenerator;

namespace Metanet.Server.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext applicationDb;
        IMapper _mapper;
        IActionContextAccessor actionContext;

        public BlogService(ApplicationDbContext _db, IMapper mp, IActionContextAccessor ac)
        {
            applicationDb = _db;
            _mapper = mp;
            actionContext = ac;
        }


        public async Task<ServiceResponse<Blog>> CreateBlog(BlogCreateDTO model)
        {
           
            if (model == null)
            {
                return new ServiceResponse<Blog>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Заполните все поля"
                };
            }
            else
            {
                var existed = applicationDb.Blogs.FirstOrDefault(b => b.Title == model.Title);
                var course = applicationDb.Courses.Find(model.CourseId);
                if(existed != null)
                {
                    return new ServiceResponse<Blog>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Такой блог уже существует"
                    };
                }
                if(course == null)
                {
                    return new ServiceResponse<Blog>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Выберите корректный курс"
                    };
                }
                Blog blog = _mapper.Map<Blog>(model);
                blog.Alias = model.Title.GenerateSlug();
                applicationDb.Blogs.AddAsync(blog);
                var result = await applicationDb.SaveChangesAsync();
                return result > 0
                    ? new ServiceResponse<Blog>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        Message = "Успешно создан курс",
                        Data = blog
                    }
                    : new ServiceResponse<Blog>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "Упс, что-то пошло не так",

                    };
            }
        }


        public async Task<ServiceResponse<Blog>> UpdateBlog(int Id,BlogDTO model)
        {

            if (model == null)
            {
                return new ServiceResponse<Blog>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Заполните все поля"
                };
            }
            else
            {
                var existed = applicationDb.Blogs.FirstOrDefault(b => b.Title == model.Title);
                var course = applicationDb.Courses.Find(model.CourseId);
                if (existed != null && existed.Id  != model.Id)
                {
                    return new ServiceResponse<Blog>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Такой блог уже существует"
                    };
                }
                if (course == null)
                {
                    return new ServiceResponse<Blog>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Выберите корректный курс"
                    };
                }
                if(Id != model.Id)
                {
                    return new ServiceResponse<Blog>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Выберите корректный блог"
                    };
                }
                Blog blog = _mapper.Map<Blog>(model);
                blog.Alias = model.Title.GenerateSlug();
                applicationDb.Blogs.Update(blog);
                var result = await applicationDb.SaveChangesAsync();
                return result > 0
                    ? new ServiceResponse<Blog>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        Message = "Успешно обновлен курс",
                        Data = blog
                    }
                    : new ServiceResponse<Blog>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "Упс, что-то пошло не так",

                    };
            }
        }


        public async Task<ServiceResponse<Blog>> GetBlog(int Id)
        {

           
                var existed = await applicationDb.Blogs.FindAsync(Id);
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
                        Message = "Такого блога не существует",
                   
                    };
            }
               
            
        }


        public async Task<ServiceResponse<bool>> DeleteBlog(int Id)
        {
            var existed = await applicationDb.Blogs.FindAsync(Id);
            if (existed != null)
            {
                applicationDb.Blogs.Remove(existed);
                applicationDb.SaveChanges();

                return new ServiceResponse<bool>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Успешно удален блог"
                };
            }
            else
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Такого блога не существует",

                };
            }
        }

        public async Task<ServiceResponse<List<Blog>>> GetAllBlogs()
        {
            List<Blog> blogs = applicationDb.Blogs.Include(Course => Course.Course).Include(p=>p.Lessons).ToList();
            return new ServiceResponse<List<Blog>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = blogs
            };
        }

        public async Task<ServiceResponse<BlogDTO>> GetBlogByAlias(string Alias)
        {
            var existed =  applicationDb.Blogs.Include(Course => Course.Course).FirstOrDefault(option => option.Alias == Alias);
            if (existed != null)
            {
                return new ServiceResponse<BlogDTO>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = _mapper.Map<BlogDTO>(existed)
                };
            }
            else
            {
                return new ServiceResponse<BlogDTO>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Такого блога не существует",

                };
            }
        }

        public async Task<ServiceResponse<List<Lesson>>> GetBlogLessons(int BlogId)
        {
            var existed = await applicationDb.Lessons.Where(l=>l.BlogId == BlogId).Include(l => l.Blog).Include(l => l.NextLesson).Include(l => l.PrevLesson).Include(l => l.Materials).ToListAsync();
            return new ServiceResponse<List<Lesson>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = existed
            };
        }
    }
}
