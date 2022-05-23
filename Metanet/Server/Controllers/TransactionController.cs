using System.Security.Claims;
using Metanet.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Metanet.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _transactionService.GetAllById(userId);
        return Ok(response);
    }
}