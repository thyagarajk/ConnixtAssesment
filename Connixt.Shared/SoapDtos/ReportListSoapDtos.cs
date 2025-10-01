using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Connixt.Shared.Models;
[XmlRoot("ZCIXMXFIMQDIDREPORTLISTResponse", Namespace = "CIXMXFWS.ORG")]
public class ZCIXMXFIMQDIDREPORTLISTResponse
{
    // The SOAP response has this element (sometimes string, sometimes XML string)
    [XmlElement("ZCIXMXFIMQDIDREPORTLISTResult")]
    public string Result { get; set; } = string.Empty;

    [XmlElement("TotalRowCount")]
    public int TotalRowCount { get; set; }

    // IMPORTANT: Add this property so you can read the XML table block
    [XmlElement("ZCIXMXFIMQDIDREPORTLISTTBL")]
    public XmlElement ZCIXMXFIMQDIDREPORTLISTTBL { get; set; }
}