using AutoMapper;
using Metanet.Server.Database;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;

namespace Metanet.Server
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<CourseCreateDTO, Course>().ReverseMap();
            CreateMap<CourseDTO, Course>().ReverseMap();
            CreateMap<BlogCreateDTO, Blog>().ReverseMap();
            CreateMap<BlogDTO, Blog>().ReverseMap();
            CreateMap<LessonCreateDTO, Lesson>().ReverseMap();
            CreateMap<LessonDTO, Lesson>().ReverseMap();
            CreateMap<MaterialDTO, Material>().ReverseMap();
            CreateMap<MaterialCreateDTO, Material>().ReverseMap();
            CreateMap<TeamCreateDTO, Team>().ReverseMap();
            CreateMap<TeamDTO, Team>().ReverseMap();
            CreateMap<List<CourseDTO>, IEnumerable<Course>>().ReverseMap();
            CreateMap<List<UserDTO>, IEnumerable<ApplicationUser>>().ReverseMap();
            CreateMap<PaginationDTO<UserDTO>, PaginationDTO<ApplicationUser>>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
           
            CreateMap<RoleDTO, Role>().ReverseMap();
            CreateMap<UserRolesDTO, UserRole>().ReverseMap();
            CreateMap<UserUpdateDTO, ApplicationUser>().ReverseMap();
            CreateMap<TransactionCreateDTO, Transaction>().ReverseMap();
            CreateMap<SubscriptionCreateDTO, Subscription>().ReverseMap();
            CreateMap<SubscriptionUpdateDTO, Subscription>().ReverseMap();
            CreateMap(typeof(PaginationDTO<>), typeof(PageResult<>)).ReverseMap();
            
           
            
        }
    }
}
