using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Connixt.Api.Services;

public class SoapClient : ISoapClient
{
    private readonly HttpClient _http;
    private readonly ILogger<SoapClient> _log;
    private readonly string _baseUrl;

    public SoapClient(HttpClient http, IConfiguration cfg, ILogger<SoapClient> log)
    {
        _http = http;
        _log = log;
        _baseUrl = "https://dev.connixtapps.com/interviewws/cixmxfws.asmx";
            //cfg["Connixt:SoapBaseUrl"] ?? throw new ArgumentException("Connixt:SoapBaseUrl missing");
    }

    public async Task<TResponse> InvokeAsync<TRequest, TResponse>(string action, TRequest request, SoapVersion version = SoapVersion.Soap11)
    {
        var innerXml = SerializeToXml(request);
        var envelope = WrapInSoapEnvelope(innerXml, version);
        var responseXml = await PostSoapAsync(action, envelope, version);

        var doc = new XmlDocument();
        doc.LoadXml(responseXml);

        // locate Body element (support SOAP 1.1 and 1.2)
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
        nsmgr.AddNamespace("s11", "http://schemas.xmlsoap.org/soap/envelope/");
        nsmgr.AddNamespace("s12", "http://www.w3.org/2003/05/soap-envelope");

        XmlNode? body = doc.SelectSingleNode("//s11:Body", nsmgr) ?? doc.SelectSingleNode("//s12:Body", nsmgr);
        if (body == null) throw new InvalidOperationException("SOAP Body not found in response");

        // find first child element of body (operation response)
        XmlElement? responseElement = null;
        foreach (XmlNode child in body.ChildNodes)
        {
            if (child.NodeType == XmlNodeType.Element)
            {
                responseElement = (XmlElement)child;
                break;
            }
        }
        if (responseElement == null) throw new InvalidOperationException("No response element found inside SOAP Body");

        // Deserialize responseElement OuterXml into TResponse
        var serializer = new XmlSerializer(typeof(TResponse));
        using var sr = new StringReader(responseElement.OuterXml);
        var result = (TResponse)serializer.Deserialize(sr)!;
        return result;
    }

    private string SerializeToXml<T>(T obj)
    {
        var serializer = new XmlSerializer(typeof(T));
        var ns = new XmlSerializerNamespaces();
        ns.Add("", "CIXMXFWS.ORG");

        var settings = new XmlWriterSettings { OmitXmlDeclaration = true, Encoding = Encoding.UTF8 };
        var sb = new StringBuilder();
        using var xw = XmlWriter.Create(sb, settings);
        serializer.Serialize(xw, obj, ns);
        return sb.ToString();
    }

    private string WrapInSoapEnvelope(string innerXml, SoapVersion version)
    {
        if (version == SoapVersion.Soap12)
        {
            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
  <soap12:Body>
    {innerXml}
  </soap12:Body>
</soap12:Envelope>";
        }
        else
        {
            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    {innerXml}
  </soap:Body>
</soap:Envelope>";
        }
    }

    private async Task<string> PostSoapAsync(string action, string envelopeXml, SoapVersion version)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl);
        if (version == SoapVersion.Soap12)
        {
            request.Content = new StringContent(envelopeXml, Encoding.UTF8, "application/soap+xml");
            // for SOAP 1.2, action can be in content-type or omitted; many ASMX endpoints still accept SOAPAction header sometimes
            request.Headers.Add("SOAPAction", $"\"CIXMXFWS.ORG/{action}\"");
        }
        else
        {
            request.Content = new StringContent(envelopeXml, Encoding.UTF8, "text/xml");
            request.Headers.Add("SOAPAction", $"\"CIXMXFWS.ORG/{action}\"");
        }

        var resp = await _http.SendAsync(request);
        resp.EnsureSuccessStatusCode();
        var respStr = await resp.Content.ReadAsStringAsync();
        _log.LogDebug("Received SOAP response ({Action}): {Len}", action, respStr.Length);
        return respStr;
    }
}
