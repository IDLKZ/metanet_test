using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metanet.Shared.Models;

public class Subscription
{
    [Key]
    public int ID { get; set; }
    
    [Required]
    
    public string UserId { get; set; }
    
    public ApplicationUser User { get; set; }

    [Required] public bool Status { get; set; } = false;
    
    public int CourseID { get; set; }
    public  Course Course { get; set; }
    
    
    public int? TransactionId { get; set; }
    public Transaction Transaction { get; set; }
}