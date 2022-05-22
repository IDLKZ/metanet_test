using System.ComponentModel.DataAnnotations;
using Metanet.Shared.Models;

namespace Metanet.Shared.DTO;

public class SubscriptionUpdateDTO
{
    [Key]
    public int ID { get; set; }
    [Required]
    
    public string UserId { get; set; }
    
    
    [Required] 
    public bool Status { get; set; }
    
    public int CourseID { get; set; }
    
    
    public int? TransactionId { get; set; }
    
    
    public virtual ApplicationUser User { get; set; }
    public virtual  Course Course { get; set; }
    public virtual  Transaction Transaction { get; set; }
}