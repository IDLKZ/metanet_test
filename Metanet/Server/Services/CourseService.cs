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
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext applicationDb;
        IMapper _mapper;
        IActionContextAccessor actionContext;
        
        public CourseService(ApplicationDbContext _db, IMapper mp,IActionContextAccessor ac)
        {
            applicationDb = _db;
            _mapper = mp;
            actionContext = ac;
        }

        public async Task<ServiceResponse<Course>> CreateCourse(CourseCreateDTO model)
        {
            var existed = applicationDb.Courses.ToList();
            if (existed.Count > 0)
            {
                return new ServiceResponse<Course>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Допустимое значение курсов - 1",
                };
            }

            if (model == null || !actionContext.ActionContext.ModelState.IsValid)
            {
                if (!actionContext.ActionContext.ModelState.IsValid)
                {
                    return new ServiceResponse<Course>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Проверьте правильность полей",
                        ErrorMessages = actionContext.ActionContext.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                    };
                }
                return new ServiceResponse<Course>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Проверьте правильность полей",
                };

            }
            Course course = _mapper.Map<Course>(model);
            course.Alias = model.Title.GenerateSlug();
            applicationDb.Courses.AddAsync(course);
            var result = await applicationDb.SaveChangesAsync();
            if (result > 0)
            {
                
                    return new ServiceResponse<Course>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status201Created,
                        Message = "Успешно создан курс",
                    };
            }
            else
            {
                return new ServiceResponse<Course>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Упс что-то пошло не так"
                };
            }
        }

        public async Task<ServiceResponse<Course>> UpdateCourse(int Id, CourseDTO model)
        {
            if (Id == model.Id)
            {
                if (!actionContext.ActionContext.ModelState.IsValid)
                {
                    return new ServiceResponse<Course>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Проверьте правильность полей",
                        ErrorMessages = actionContext.ActionContext.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                    };
                }
                else
                {
                    Course course = _mapper.Map<Course>(model);
                    course.Alias = model.Title.GenerateSlug();
                    applicationDb.Courses.Update(course);
                    var result = await applicationDb.SaveChangesAsync();
                    if (result > 0)
                    {
                        return new ServiceResponse<Course>
                        {
                            Success = true,
                            StatusCode = StatusCodes.Status201Created,
                            Message = $"Успешно обновлен курс {course.Title}"
                        };
                    }
                    else
                    {
                        return new ServiceResponse<Course>
                        {
                            Success = false,
                            StatusCode = StatusCodes.Status500InternalServerError,
                            Message = "Упс что-то пошло не так"
                        };
                    }
                }
            }
            else
            {
                return new ServiceResponse<Course>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Не найден курс"

                };

            }
        }


        public async Task<ServiceResponse<CourseDTO>> GetCourseByAlias(string Alias)
        {
            if (Alias == null)
            {
                return new ServiceResponse<CourseDTO>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Проверьте правильность полей",

                };
            }
            else
            {
                Course course = applicationDb.Courses.FirstOrDefault(option => option.Alias == Alias);
                if (course != null)
                {
                    CourseDTO courseDTO = _mapper.Map<CourseDTO>(course);
                    return new ServiceResponse<CourseDTO>
                    {
                        Success = true,
                        StatusCode = StatusCodes.Status200OK,
                        Data = courseDTO
                    };
                }
                else
                {
                    return new ServiceResponse<CourseDTO>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Курс не найден"
                    };
                }
            }
        }

        public async Task<ServiceResponse<List<Course>>> GetAllCourses()
        {
            var courses =  applicationDb.Courses.Include(c => c.Blog).ToList();
            return new ServiceResponse<List<Course>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = courses,
            };
        }
    }
}
