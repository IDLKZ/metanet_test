using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metanet.Shared.Models;

public class Subscription
{
    [Key]
    public int ID { get; set; }
    
    [Required]
    
    public string UserId { get; set; }
    
    public virtual ApplicationUser User { get; set; }

    [Required] public bool Status { get; set; } = false;
    
    public int CourseID { get; set; }
    public virtual  Course Course { get; set; }
    
    
    public int? TransactionId { get; set; }
    public virtual Transaction Transaction { get; set; }
}