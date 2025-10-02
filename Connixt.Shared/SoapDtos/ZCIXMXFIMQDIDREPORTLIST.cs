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
        [XmlElement("ZUSERNAME")] public string? ZUSERNAME { get; set; }
        [XmlElement("ZPASSWORD")] public string? ZPASSWORD { get; set; }
        [XmlElement("ZAPPID")] public string? ZAPPID { get; set; }
        [XmlElement("ZPARTNERID")] public string? ZPARTNERID { get; set; }
        [XmlElement("ZTRANSID")] public string? ZTRANSID { get; set; }
        [XmlElement("ZTRANSTYPE")] public string? ZTRANSTYPE { get; set; }
        [XmlElement("PageNumber")] public int? PageNumber { get; set; }
        [XmlElement("PageSize")] public int? PageSize { get; set; }
        [XmlElement("SearchFilter")] public string? SearchFilter { get; set; }
        [XmlElement("sort")] public string? sort { get; set; }
        [XmlElement("ZSORTBY")] public string? ZSORTBY { get; set; }
        [XmlElement("ZSORTDIR")] public string? ZSORTDIR { get; set; }
        [XmlElement("F1")] public string? F1 { get; set; }
        [XmlElement("F2")] public string? F2 { get; set; }
        [XmlElement("F3")] public string? F3 { get; set; }
        [XmlElement("F4")] public string? F4 { get; set; }
        [XmlElement("F5")] public string? F5 { get; set; }
        [XmlElement("F6")] public string? F6 { get; set; }
        [XmlElement("F7")] public string? F7 { get; set; }
        [XmlElement("F8")] public string? F8 { get; set; }
        [XmlElement("F9")] public string? F9 { get; set; }
        [XmlElement("F10")] public string? F10 { get; set; }
    }
}
