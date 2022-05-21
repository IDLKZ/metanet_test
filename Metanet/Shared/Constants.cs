using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared
{
    public class Constants
    {
        public const  string Admin = "Admin";

        public static string Customer = "Customer";

       public static string  Token = "Token";

        public static string User = "User";

        public static string[] Roles = { "Admin", "Customer" };

        //CURRENCY API
        public static string CurrencyExchange = "https://api.exchangerate.host/latest?base=usd&symbols=EUR,RUB,KZT";
        public static string CryptoExchange = "https://api.exchangerate.host/latest?base=usd&source=crypto&symbols=ETH,XRP,DOGE,BSC,AVA";

    }
}
