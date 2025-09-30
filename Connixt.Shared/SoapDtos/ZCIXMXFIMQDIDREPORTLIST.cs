using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Connixt.Shared.SoapDtos
{
    [XmlRoot("ZCIXMXFIMQDIDREPORTLIST", Namespace = "CIXMXFWS.ORG")]
    public class ZCIXMXFIMQDIDREPORTLIST
    {
        public string? ZUSERNAME { get; set; }
        public string? ZPASSWORD { get; set; }
        public string? ZAPPID { get; set; }
        public string? ZPARTNERID { get; set; }
        public string? ZTRANSID { get; set; }
        public string? ZTRANSTYPE { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchFilter { get; set; }
        public string? sort { get; set; }
        public string? ZSORTBY { get; set; }
        public string? ZSORTDIR { get; set; }
    }
}
