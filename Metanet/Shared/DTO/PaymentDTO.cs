using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Metanet.Shared.Helpers;

namespace Metanet.Shared.DTO;

public class PaymentDTO
{
    [Required]
    public int ORDER { get; set; }
    [Required]
    [Range(100,System.Int64.MaxValue)]
    public int AMOUNT { get; set; }
    [Required]
    public string CURRENCY { get; set; } = "KZT";
    [Required]
    public string MERCHANT { get; } = PaymentHelper.MID;
    [Required]
    public string TERMINAL { get; } = PaymentHelper.TID;
    public string NONCE { get; set; } = String.Empty;
    public string LANGUAGE { get; } = "ru";
    public string CLIENT_ID { get; set; } = String.Empty;
    [Required]
    [MaxLength(50)]
    public string DESC { get; set; } = "Приобретение курса Metanet";
    public string DESC_ORDER { get; } = String.Empty;
    public string EMAIL { get; set; } = String.Empty;
    public string BACKREF { get; set; } = "https://metanet.education/payment-info/?redirect=true";
    public string Ucaf_Flag { get; set; } = String.Empty;
    public string Ucaf_Authentication_Data { get; set; } = String.Empty;
    public string crd_pan { get; set; } = String.Empty;
    public string crd_exp { get; set; } = String.Empty;
    public string crd_cvc { get; set; } = String.Empty;
    public int WTYPE { get; set; } = 3;
    public string NAME { get; set; } = String.Empty;
    public string RECUR_FREQ { get; set; } = String.Empty;
    public string RECUR_EXP { get; set; } = String.Empty;
    public string INT_REF { get; set; } = String.Empty;
    public string RECUR_REF { get; set; } = String.Empty;
    public string P_SIGN { get; set; } = String.Empty;
    public string MK_TOKEN { get; set; } = String.Empty;
    public string MERCH_TOKEN_ID { get; set; } = String.Empty;
    public string PAYMENT_TO { get; set; } = String.Empty;
    
    
    
    
    
    
    
    
    





}