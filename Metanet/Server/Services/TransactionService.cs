using System.Security.Claims;
using Metanet.Server.Database;
using Metanet.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

namespace Metanet.Server.Services;

public class TransactionService : ITransactionService
{
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public TransactionService(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
    {
        _context = context;
        _signInManager = signInManager;
    }
    public async Task<List<Transaction>> GetAllById(string id)
    {
        var response = await _context.Transactions
            .OrderByDescending(d => d.CreatedAt)
            .Where(u => u.UserId == id)
            .Include(c => c.Course)
            .ToListAsync();
        return response;
    }
}