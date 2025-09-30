using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connixt.Shared.Models
{
    public class LogonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public string FullName { get; set; } = "";
    }
}
