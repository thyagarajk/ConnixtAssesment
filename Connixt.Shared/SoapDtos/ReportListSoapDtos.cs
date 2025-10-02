using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Connixt.Shared.Models;
[XmlRoot("ZCIXMXFIMQDIDREPORTLISTResponse", Namespace = "CIXMXFWS.ORG")]
public class ZCIXMXFIMQDIDREPORTLISTResponse
{
    [XmlElement("ZCIXMXFIMQDIDREPORTLISTResult")]
    public string Result { get; set; } = string.Empty;

    [XmlElement("TotalRowCount")]
    public int TotalRowCount { get; set; }

    [XmlAnyElement("ZCIXMXFIMQDIDREPORTLISTTBL")]
    public XmlElement[] ZCIXMXFIMQDIDREPORTLISTTBL { get; set; }

}