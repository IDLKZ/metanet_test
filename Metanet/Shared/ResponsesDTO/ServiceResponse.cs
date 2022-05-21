using Metanet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.ResponsesDTO
{
    public class ServiceResponse <T>
    {
        public bool Success { get; set; }

        public IEnumerable<string>? ErrorMessages { get; set; }

        public string Message { get; set; }

        public int StatusCode { get; set; }

        public T Data { get; set; }

        
    }
}
