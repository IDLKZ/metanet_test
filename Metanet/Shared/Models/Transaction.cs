using System.ComponentModel.DataAnnotations;

namespace Metanet.Shared.Models;

public class Transaction
{
    [Key]
    public int ID { get; set; }
    
    [Required]
    public int ORDER { get; set; }
    
    public string? MPI_ORDER { get; set; }
    
    public string? RRN { get; set; }
    
    public int? RES_CODE { get; set; }
    
    [Required]
    [MaxLength(255)]
    [Range(100, 100000000)]
    public string AMOUNT { get; set; }
    
    [Required]
    public string CURRENCY { get; set; }
    
    
    public string? RES_DESC { get; set; }

    public int Status { get; set; } = 0;
    
    public string UserId { get; set; }
    
    public virtual ApplicationUser User { get; set; }
    
    public int CourseId { get; set; }
    
    public virtual  Course Course { get; set; }
    
    public string? P_SIGN { get; set; }
    
    
}