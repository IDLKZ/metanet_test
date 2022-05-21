using Metanet.Server.Database;
using Metanet.Server.Services;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;
using Metanet.Shared.ResponsesDTO;
using Microsoft.AspNetCore.Mvc;

namespace Metanet.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MailController : ControllerBase
    {
        ApplicationDbContext dbContext;

        private readonly IMailService mailService;
        public MailController(IMailService _mailService,ApplicationDbContext _context)
        {
            mailService = _mailService;
            dbContext = _context;
        }

        
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> SendMail([FromForm] MailRequest request)
        {
                var result = await mailService.SendEmailAsync(request);
                return Ok(result);
            
            

        }
    }
}
