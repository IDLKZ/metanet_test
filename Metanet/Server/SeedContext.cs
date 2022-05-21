using Metanet.Server.Database;
using Metanet.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace Metanet.Server;

public class SeedContext
{
    static async Task SeedAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!context.Roles.Any())
            {

                List<Role> roles = new List<Role>() {
                    new Role {Id = "fb898f96-83c5-4803-ae34-113307fa0be8", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "a4e368a8-be53-4103-8f19-ca5dd85de7c1"},
                    new Role {Id = "a5c34f10-1f37-481d-81a4-f5fb2c9a9c17", Name = "Customer", NormalizedName = "CUSTOMER", ConcurrencyStamp = "ccea0692-5aad-4a3f-8c06-3386593d614d"}
                };
                await  context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<SeedContext>();
            logger.LogError(ex, ex.Message);
        }
        
    }
}