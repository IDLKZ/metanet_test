using System.ComponentModel.DataAnnotations;
using Metanet.Shared.Models;

namespace Metanet.Shared.DTO;

public class SubscriptionCreateDTO
{
    [Required]
    
    public string UserId { get; set; }
    
    
    [Required] 
    public bool Status { get; set; } = false;
    
    public int CourseID { get; set; }
    
    
    public int? TransactionId { get; set; }
}