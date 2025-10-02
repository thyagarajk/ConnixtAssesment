using Connixt.Shared.Models;
using Connixt.Shared.SoapDtos;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace Connixt.Api.Services;

public class SoapService : ISoapService
{
    private readonly ISoapClient _soapClient;
    private readonly ILogger<SoapService> _log;

    public SoapService(ISoapClient soapClient, ILogger<SoapService> log)
    {
        _soapClient = soapClient;
        _log = log;
    }

    public async Task<LogonResult> LogonAsync(string username, string password)
    {
        var req = new ZCIXMXFIMQLOGON
        {
            ZUSERNAME = username,
            ZPASSWORD = password,
            ZAPPID = "ANDROIDPHONEFM",
            ZTRANSID = "LOGON",
            ZLASTLOGINDT = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")
        };

        var resp = await _soapClient.InvokeAsync<ZCIXMXFIMQLOGON, ZCIXMXFIMQLOGONResponse>(
    "ZCIXMXFIMQLOGON", req);

        //var resp = await _soapClient.InvokeAsync<ZCIXMXFIMQLOGON, ZCIXMXFIMQLOGONResponse>("ZCIXMXFIMQLOGON");

        var success = string.Equals(resp.ZLOGONSTATUS, "S", StringComparison.OrdinalIgnoreCase);
        return new LogonResult
        {
            Success = success,
            Message = resp.Result ?? "",
            FullName = resp.ZUSERFLNAME ?? username
        };
    }

    public async Task<ReportListResponse> GetReportListAsync(string username, string password, int page = 0, int pageSize = 20)
    {
        var req = new ZCIXMXFIMQDIDREPORTLIST
        {
            ZUSERNAME = username,
            ZPASSWORD = password,
            ZAPPID = "ANDROIDPHONEFM",
            ZPARTNERID="",
            ZTRANSID = "DIDTRACK",
            ZTRANSTYPE = "INSPECTION",
            PageNumber = page,
            PageSize = pageSize,
            SearchFilter = "",
            sort = "",
            ZSORTBY = "",
            ZSORTDIR = "",
            F1 = "",
            F2 = "",
            F3 = "",
            F4 = "",
            F5 = "",
            F6 = "",
            F7 = "",
            F8 = "",
            F9 = "",
            F10 = ""
        };

        // Call SOAP
        var resp = await _soapClient.InvokeAsync<
            ZCIXMXFIMQDIDREPORTLIST,
            ZCIXMXFIMQDIDREPORTLISTResponse>(
            "ZCIXMXFIMQDIDREPORTLIST", req);

        var result = new ReportListResponse
        {
            Total = resp?.TotalRowCount ?? 0,
            Rows = new List<ReportRow>()
        };

        var tblXml = resp.ZCIXMXFIMQDIDREPORTLISTTBL?.FirstOrDefault()?.OuterXml;
        if (!string.IsNullOrEmpty(tblXml))
        {
            var xdoc = XDocument.Parse(tblXml);

            XNamespace diffgr = "urn:schemas-microsoft-com:xml-diffgram-v1";

            var rows = xdoc.Descendants(diffgr + "diffgram")
                           .Descendants("ZCIXMXFIMQDIDREPORTLIST");

            foreach (var node in rows)
            {
                var rr = new ReportRow
                {
                    ZHOSTID = node.Element("ZHOSTID")?.Value,
                    ZDIDDATANUM = node.Element("ZDIDDATANUM")?.Value,
                    ZDIDDATAST = node.Element("ZDIDDATAST")?.Value,
                    ZDIDDESC = node.Element("ZDIDDESC")?.Value,
                    ZREPSTATUS = node.Element("ZREPSTATUS")?.Value,
                    ZETA = node.Element("ZETA")?.Value
                };
                result.Rows.Add(rr);
            }
        }



        return result;

    }

}
