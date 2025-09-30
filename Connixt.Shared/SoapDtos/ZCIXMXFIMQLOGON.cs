using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Connixt.Shared.SoapDtos
{
    [XmlRoot("ZCIXMXFIMQLOGON", Namespace = "CIXMXFWS.ORG")]
    public class ZCIXMXFIMQLOGON
    {
        public string? ZUSERNAME { get; set; }
        public string? ZPASSWORD { get; set; }
        public string? ZAPPID { get; set; }
        public string? ZPARTNERID { get; set; }
        public string? ZTRANSID { get; set; }
        public string? ZEMAILVALIDATED { get; set; }
        public string? ZDEVICEID { get; set; }
        public string? ZLASTLOGINDT { get; set; }
        public string? ZDEVICEMODEL { get; set; }
        public string? ZDEVICEOSVERSION { get; set; }
        public string? ZDEVICEAPPVERSION { get; set; }
        public string? ZAPPIDENTIFIER { get; set; }
        public string? ZDEVICENAME { get; set; }
        public string? ZDEVICETYPE { get; set; }
        public string? ZAPPBUNDLEID { get; set; }
        public string? ZAPPVERSION { get; set; }
        public string? ZAPPSERVER { get; set; }
        public string? ZDEVICEOS { get; set; }
    }
}
