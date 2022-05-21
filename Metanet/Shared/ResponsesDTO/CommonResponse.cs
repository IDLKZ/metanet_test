using Metanet.Shared.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.ResponsesDTO
{
    public class CommonResponse
    {
        public bool Success { get; set; }
        
        public IEnumerable<string>? ErrorMessages { get; set; }

        public string Message { get; set; }

        public int StatusCode { get; set; } = 201;

        public string? Token { get; set; }

        public UserDTO? User { get; set; }

       
   
    }

   


}
