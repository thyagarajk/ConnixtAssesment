using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Connixt.Shared.SoapDtos
{
    [XmlRoot("ZCIXMXFIMQLOGONResponse", Namespace = "CIXMXFWS.ORG")]
    public class ZCIXMXFIMQLOGONResponse
    {
        [XmlElement("ZCIXMXFIMQLOGONResult")]
        public string? Result { get; set; }

        [XmlElement("ZLOGONSTATUS")]
        public string? ZLOGONSTATUS { get; set; }

        [XmlElement("ZUSERTYPE")]
        public string? ZUSERTYPE { get; set; }

        [XmlElement("ZUSERFLNAME")]
        public string? ZUSERFLNAME { get; set; }

        [XmlElement("ZISADMIN")]
        public string? ZISADMIN { get; set; }

        [XmlElement("ZPWDSTATUS")]
        public string? ZPWDSTATUS { get; set; }

        [XmlElement("ZPWDMESSAGE")]
        public string? ZPWDMESSAGE { get; set; }

        [XmlElement("ZPWDTEMP")]
        public string? ZPWDTEMP { get; set; }
    }
}
