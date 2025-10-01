using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Connixt.Shared.Models
{
    public class ReportRow
    {
        public string ZHOSTID { get; set; } = string.Empty;
        public string ZDIDDATANUM { get; set; } = string.Empty;
        public string ZDIDDATAST { get; set; } = string.Empty;
        public string ZDIDDESC { get; set; } = string.Empty;

        // Fields you were missing (added)
        public string ZDIDCONFIGNUM { get; set; }
        public string ZREPSTATUS { get; set; }
        public string ZETA { get; set; }
    }

}
