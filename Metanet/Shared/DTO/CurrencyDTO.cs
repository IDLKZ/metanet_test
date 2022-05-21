using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.DTO
{
    public class Motd
    {
        public string msg { get; set; }
        public string url { get; set; }
    }

    public class Rates
    {
        public double EUR { get; set; }
        public double KZT { get; set; }
        public double RUB { get; set; }
        public double ETH { get; set; }
        public double XRP { get; set; }
        public double DOGE { get; set; }
        public double BSC { get; set; }
        public double AVA { get; set; }
        
    }

    public class CurrencyDTO
    {
       
        public bool success { get; set; }
        public string @base { get; set; }
        public string date { get; set; }
        public Rates rates { get; set; }
    }
}
