using System.Collections.Generic;
using System.Xml.Serialization;
using Connixt.Shared.Models;

[XmlRoot("ZCIXMXFIMQDIDREPORTLISTResponse", Namespace = "CIXMXFWS.ORG")]
public class ZCIXMXFIMQDIDREPORTLISTResponse
{
    [XmlElement("ZCIXMXFIMQDIDREPORTLISTResult")]
    public ZCIXMXFIMQDIDREPORTLISTResult Result { get; set; } = new();

    [XmlElement("TotalRowCount")]
    public int TotalRowCount { get; set; }
}

public class ZCIXMXFIMQDIDREPORTLISTResult
{
    [XmlElement("diffgram", Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1")]
    public Diffgram Diffgram { get; set; } = new();
}

public class Diffgram
{
    [XmlElement("DocumentElement", Namespace = "")]
    public DocumentElement DocumentElement { get; set; } = new();
}

public class DocumentElement
{
    [XmlElement("ZCIXMXFIMQDIDREPORTLIST", Namespace = "")]
    public List<ReportRow> Items { get; set; } = new();
}
