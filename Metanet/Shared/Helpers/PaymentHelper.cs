using System.Security.Cryptography;
using System.Text;
using Metanet.Shared.DTO;
using Metanet.Shared.Models;

namespace Metanet.Shared.Helpers;

public class PaymentHelper
{
    private static string SecredShared = "01234567890123456789012";
    public static string MID = "TEST_ECOM";
    public static string TID = "WEB10004";
    private  static  string PublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAtYRnFmeM8vHQutPbOoZfQRDhVl6JbDOz7buqjRgX/RxYAyAkhxykYqsHF6YmSWQQOrFc45L+PwKQih4nPe2Co4jEE/GeltcvnGn7rqz9CF2NHu4a2Ri+5jFxmWPCavZbNO0SnIAy/zFqcAG66ihsfLZBcWruog1sZBbSV6aVzKo9Ipo+hyPbW/RTJ6OOdLMsPqgs6nnk9KukzYyXGIv9mDj13biYIuQjgBtYD0XeAdSCAdAaLniYMcZNQfKOYYjjCq02lwVi8uv/uipPoTYRiIeMYc9eRFDsVVHApdssyBmijfpzGdApPuIjk9tDUuhRGUGXVt1bmmSvj9ZamK4E8wIDAQAB";

    public static string GenerateEncryptString(string TextToEncrypt)
    {
        try
        {
            RSACryptoServiceProvider _rsa = new RSACryptoServiceProvider();
            var  publicKey = $"-----BEGIN PUBLIC KEY-----\n{PublicKey}\n-----END PUBLIC KEY-----\n";
            _rsa.ImportFromPem(publicKey);
            var data = Encoding.UTF8.GetBytes(TextToEncrypt);
            var encryptedData =  Convert.ToBase64String(_rsa.Encrypt(data, false));
            return encryptedData;
        }
        catch (Exception e)
        {
            
            return e.Message.ToString();
        }
    }

    public static string GetHashedString(string TextToHashed)
    {
        SHA512 sha512 = new System.Security.Cryptography.SHA512Managed();

        byte[] sha512Bytes = System.Text.Encoding.Default.GetBytes(TextToHashed);

        byte[] cryString = sha512.ComputeHash(sha512Bytes);

        string hashedPwd = string.Empty;

        for (int i = 0; i < cryString.Length; i++)
        {
            hashedPwd += cryString[i].ToString("X2");
        }

        return hashedPwd.ToLower();
    }

    public static int GetOrderNumber(List<int> ORDERS)
    {
        
        Random rnd = new Random();
        int min = 100000;
        int max = Int32.MaxValue;
        int order =  rnd.Next(min, max);
        if (ORDERS.Contains(order))
        {
            return  PaymentHelper.GetOrderNumber(ORDERS);
        }

        return order;

    }

    public static PaymentDTO GetEncryptedCardInfo(PaymentDTO paymentDto)
    {
         paymentDto.crd_cvc = PaymentHelper.GenerateEncryptString(paymentDto.crd_cvc);
         paymentDto.crd_exp = PaymentHelper.GenerateEncryptString(paymentDto.crd_exp);
         paymentDto.crd_pan = PaymentHelper.GenerateEncryptString(paymentDto.crd_pan); 
         paymentDto.PAYMENT_TO = PaymentHelper.GenerateEncryptString(paymentDto.PAYMENT_TO);
         return paymentDto;
    }

    public static string GetStringForPayment(PaymentDTO paymentDto)
    {
        string textToSign = SecredShared+paymentDto.ORDER.ToString()+";"+paymentDto.AMOUNT.ToString()+";"+paymentDto.CURRENCY+";"
                            +paymentDto.MERCHANT+";"+paymentDto.TERMINAL+";"
                            +paymentDto.NONCE+";"+paymentDto.CLIENT_ID+";"+paymentDto.DESC+";"+ paymentDto.DESC_ORDER +";"
                            +paymentDto.EMAIL+";"+paymentDto.BACKREF+";"+paymentDto.Ucaf_Flag+";"+paymentDto.Ucaf_Authentication_Data+";"
                            +paymentDto.RECUR_FREQ+";"+paymentDto.RECUR_EXP+";"+paymentDto.INT_REF + ";"+paymentDto.RECUR_REF+";"
                            +paymentDto.PAYMENT_TO+";"
                            +paymentDto.crd_pan+";"+paymentDto.crd_exp+";"+paymentDto.crd_cvc+";";
        return textToSign;
    }

    public static string GetStringAfterPayment(TransactionUpdateDTO transaction)
    {
        string text = PaymentHelper.SecredShared + transaction.ORDER.ToString() + ";" + transaction.MPI_ORDER +";"+ transaction.RRN + ";" +
                      transaction.RES_CODE + ";" + transaction.AMOUNT + ";" + transaction.CURRENCY + ";" + transaction.RES_DESC + ";";
        return text;
    }

    public static string GetStringForTransaction(string DateTo, string DateFrom)
    {
        string text = PaymentHelper.SecredShared + DateFrom + ";" + DateTo + ";" + PaymentHelper.TID + ";" +
                      PaymentHelper.MID;
        return text;
    }
    
    public static string GetStringForSingleTransaction(string ORDER)
    {
        string text = PaymentHelper.SecredShared + ORDER + ";" + PaymentHelper.MID;
        return text;
    }

    public static PaymentDTO GetPaymentDto(Course course, List<int> Orders,ApplicationUser user)
    {
        if (course != null)
        {
            PaymentDTO paymentDto = new PaymentDTO();
            long order = PaymentHelper.GetOrderNumber(Orders);
            paymentDto.NAME = user.UserName;
            paymentDto.EMAIL = user.Email;
            paymentDto.ORDER = PaymentHelper.GetOrderNumber(Orders);
            paymentDto.AMOUNT = (int)course.Price;
            paymentDto.DESC = $"Приобретение курса {course.Title}";
            //paymentDto = GetEncryptedCardInfo(paymentDto);
            paymentDto.P_SIGN = GetHashedString(GetStringForPayment(paymentDto));
            return paymentDto;
        }

        return null;

    }



    
    
    
    
    

}