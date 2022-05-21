using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanet.Shared.DTO
{
    public class FileDTO
    {
        public string Name { get; set; }
        public double Size { get; set; }

        public string Extension { get; set; }

        public IBrowserFile File { get; set; }
    }
}
