using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Metanet.Shared.DTO
{
    public class PaginationDTO<T>
    {

        public int ItemCount { get; set; }

        public int PageCount { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public IEnumerable<T> Data { get; set; }

        public int FirstPage { get; set; }

        public int LastPage { get; set; }

        public int NextPage { get; set; }
    
        public int CurrentPage { get; set; }

        public int PreviousPage { get; set; }

        
    }



    
}
