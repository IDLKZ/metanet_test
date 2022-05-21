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
    public class LessonService : ILessonService
    {

        private readonly ApplicationDbContext applicationDb;
        IMapper _mapper;
        IActionContextAccessor actionContext;

        public LessonService(ApplicationDbContext _db, IMapper mp, IActionContextAccessor ac)
        {
            applicationDb = _db;
            _mapper = mp;
            actionContext = ac;
        }

        public async Task<ServiceResponse<Lesson>> CreateLesson(LessonCreateDTO model)
        {
            if(model == null)
            {
                return new ServiceResponse<Lesson>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Пожалуйста, проверьте правильность заполнения полей"
                };
            }
            else
            {
                var existed = applicationDb.Lessons.FirstOrDefault(opt => opt.Title == model.Title);
                var blog = applicationDb.Blogs.Find(model.BlogId);
                if(existed != null)
                {
                    return new ServiceResponse<Lesson>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Такой урок уже существует"
                    };
                }
                if(blog == null)
                {
                    return new ServiceResponse<Lesson>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Такого блога не существует"
                    };
                }

                Lesson lesson = _mapper.Map<Lesson>(model);
                lesson.Alias = model.Title.GenerateSlug();
                await applicationDb.Lessons.AddAsync(lesson);
                var result = await applicationDb.SaveChangesAsync();
                if(result > 0)
                {
                    return new ServiceResponse<Lesson>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        Message = "Урок успешно создан",
                        Data =  lesson
                    };
                }
                else
                {
                    return new ServiceResponse<Lesson>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "Упс... Что-то пошло не так",
                    };
                }
                
            }
        }

        public async Task<ServiceResponse<bool>> DeleteLesson(int Id)
        {
            var existed = await applicationDb.Lessons.FindAsync(Id);
            if (existed != null)
            {
                applicationDb.Lessons.Remove(existed);
                applicationDb.SaveChanges();
                return new ServiceResponse<bool>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Урок успешно удален"
                    
                };
            }
            else
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Такого урока не существует"
                };
            }
        }

        public async Task<ServiceResponse<List<Lesson>>> GetAllLessons()
        {
            var existed = await applicationDb.Lessons.Include(l=>l.Blog).Include(l=>l.NextLesson).Include(l => l.PrevLesson).Include(l=>l.Materials).ToListAsync();
            return new ServiceResponse<List<Lesson>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = existed
            };
        }

        public async Task<ServiceResponse<Lesson>> GetLesson(int Id)
        {
            var existed = await applicationDb.Lessons.FindAsync(Id);
            if(existed != null)
            {
                return new ServiceResponse<Lesson>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = existed
                };
            }
            else
            {
                return new ServiceResponse<Lesson>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Такого урока не существует"
                };
            }
       

        }

        public async Task<ServiceResponse<LessonDTO>> GetLessonByAlias(string Alias)
        {
            var existed = await applicationDb.Lessons.FirstOrDefaultAsync(opt => opt.Alias == Alias);
            if(existed != null)
            {
                return new ServiceResponse<LessonDTO>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Data = _mapper.Map<LessonDTO>(existed)
                };
            }
            else
            {
                return new ServiceResponse<LessonDTO>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Урок не найден"
                };
            }
        }

        public async Task<ServiceResponse<Lesson>> UpdateLesson(int Id, LessonDTO model)
        {
            if(Id != model.Id)
            {
                    return new ServiceResponse<Lesson>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Проверьте правильность запроса"
                    };
            }
                else
                {
                    var existedLesson = applicationDb.Lessons.FirstOrDefault(opt => opt.Title == model.Title);
                    var blog = applicationDb.Blogs.Find(model.BlogId);
                    if (existedLesson != null && existedLesson.Id != model.Id)
                    {
                        return new ServiceResponse<Lesson>
                        {
                            Success = false,
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = "Такой урок уже существует"
                        };
                    }
                    if (blog == null)
                    {
                        return new ServiceResponse<Lesson>
                        {
                            Success = false,
                            StatusCode = StatusCodes.Status404NotFound,
                            Message = "Такого блога не существует"
                        };
                    }

                    Lesson lesson = _mapper.Map<Lesson>(model);
                    lesson.Alias = model.Title.GenerateSlug();
                    applicationDb.Lessons.Update(lesson);
                    var result = await applicationDb.SaveChangesAsync();
                    if (result > 0)
                    {
                        return new ServiceResponse<Lesson>
                        {
                            Success = true,
                            StatusCode = StatusCodes.Status201Created,
                            Message = "Урок успешно обновлен",
                            Data = lesson
                        };
                    }
                    else
                    {
                        return new ServiceResponse<Lesson>
                        {
                            Success = false,
                            StatusCode = StatusCodes.Status500InternalServerError,
                            Message = "Упс... Что-то пошло не так",
                        };
                    }
                }
            
        }
            
            
        
    }
}
