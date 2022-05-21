using System.ComponentModel.DataAnnotations;

namespace Metanet.Shared.DTO;

public class SubscriptionUpdateDTO
{
    [Key]
    public int ID { get; set; }
    [Required]
    
    public string UserId { get; set; }
    
    
    [Required] 
    public bool Status { get; set; } = false;
    
    public int CourseID { get; set; }
    
    
    public int? TransactionId { get; set; }
}