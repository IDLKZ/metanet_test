using Metanet.Server.Database;
using Metanet.Server.Services;
using Metanet.Shared;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
namespace Metanet.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        protected IHomeService homeService;
        static readonly HttpClient client = new HttpClient();
        UserManager<ApplicationUser> userManager;
        private string userId = "";
        public HomeController(IHomeService service,HttpClient http,UserManager<ApplicationUser> _userManager)
        {
            homeService = service;
            userManager = _userManager;

        }
        [HttpGet]
        public async Task<ActionResult<List<Team>>> GetAllTeams()
        {
            var response = await homeService.GetAllTeams();
            return  Ok(response);
        }
        [HttpGet]
        public async Task<ActionResult<Course>> GetCourse()
        {
            var response = await homeService.GetCourse();
           return  Ok(response);
        }
        [HttpGet("{Alias}")]
        public async Task<ActionResult<Course>> GetCourseByAlias(string Alias)
        {
            var response = await homeService.GetCourseByAlias(Alias);
           return  Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<Blog>>> GetAllBlogs()
        {
            var response = await homeService.GetAllBlogs();
            return  Ok(response);
        }

        [HttpGet("{CourseId:int}")]
        public async Task<ActionResult<List<Blog>>> GetBlogByCourseId(int CourseId)
        {
            var response = await homeService.GetBlogByCourseId(CourseId);
           return  Ok(response);
        }

        [HttpGet("{Alias}")]
        public async Task<ActionResult<Blog>> GetBlogByAlias(string Alias)
        {
            var response = await homeService.GetBlogByAlias(Alias);
           return  Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Blog>> GetBlogById(int Id)
        {
            var response = await homeService.GetBlogById(Id);
           return  Ok(response);
        }

        [HttpGet("{BlogId:int}")]
        public async Task<ActionResult<List<Lesson>>> GetLessonByBlogId(int BlogId)
        {
            var response = await homeService.GetLessonByBlogId(BlogId);
           return  Ok(response);
        }

        [HttpGet("{Alias}")]
        public async Task<ActionResult<Lesson>> GetLessonByAlias(string Alias)
        {
            var response = await homeService.GetLessonByAlias(Alias);
           return  Ok(response);
        }

        [HttpGet]
        public async  Task<ActionResult<CurrencyDTO>> GetCurrency()
        {
            CurrencyDTO currencyDTO = new CurrencyDTO();
            try
            {
                var currency_response = await client.GetAsync(Constants.CurrencyExchange);
                var crypto_response = await client.GetAsync(Constants.CryptoExchange);
                var currency = await currency_response.Content.ReadAsStringAsync();
                var crypto = await crypto_response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CurrencyDTO>(currency);
                var result2 = JsonConvert.DeserializeObject<CurrencyDTO>(crypto);

                if(result != null && result.success == true)
                {
                    if(result2.success == true)
                    {
                        result.rates.ETH = result2.rates.ETH;
                        result.rates.XRP = result2.rates.XRP;
                        result.rates.DOGE = result2.rates.DOGE;  
                        result.rates.BSC = result2.rates.BSC;
                        result.rates.AVA = result2.rates.AVA;
                     
                     }
                    return result;
                }
            }
            catch
            {

            }


            return currencyDTO;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<StatsDTO>>> GetAllStats()
        {

            var result = await homeService.GetStats();
            return Ok(result);

        }

        

        

}
}
