using Connixt.Shared.Models;
using Connixt.Shared.SoapDtos;
using System.Data;
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

    //public async Task<ReportListResponse> GetReportListAsync(string username, string password, int page = 0, int pageSize = 20)
    //{
    //    var req = new ZCIXMXFIMQDIDREPORTLIST
    //    {
    //        ZUSERNAME = username,
    //        ZPASSWORD = password,
    //        ZAPPID = "ANDROIDPHONEFM",
    //        ZTRANSID = "DIDTRACK",
    //        ZTRANSTYPE = "INSPECTION",
    //        PageNumber = page,
    //        PageSize = pageSize
    //    };

    //    var resp = await _soapClient.InvokeAsync<ZCIXMXFIMQDIDREPORTLIST, ZCIXMXFIMQDIDREPORTLISTResponse>(
    //        "ZCIXMXFIMQDIDREPORTLIST", req);

    //    // Map SOAP DTO into your shared DTO
    //    return new ReportListResponse
    //    {
    //        Total = resp.TotalRowCount,
    //        Rows = resp.Result.Diffgram.DocumentElement.Items
    //    };
    //}

    //    public async Task<ReportListResponse> GetReportListAsync(string username, string password, int page = 0, int pageSize = 20)
    //    {
    //        var req = new ZCIXMXFIMQDIDREPORTLIST
    //        {
    //            ZUSERNAME = username,
    //            ZPASSWORD = password,
    //            ZAPPID = "ANDROIDPHONEFM",
    //            ZTRANSID = "DIDTRACK",
    //            ZTRANSTYPE = "INSPECTION",
    //            PageNumber = page,
    //            PageSize = pageSize
    //        };

    //        //var resp = await _soapClient.InvokeAsync<ZCIXMXFIMQDIDREPORTLIST, ZCIXMXFIMQDIDREPORTLISTResponse>(
    //        //    "ZCIXMXFIMQDIDREPORTLIST", req);

    //        //var result = new ReportListResponse
    //        //{
    //        //    Total = resp?.TotalRowCount ?? 0,
    //        //    Rows = new List<ReportRow>()
    //        //};
    //        var resp = await _soapClient.InvokeAsync<
    //    ZCIXMXFIMQDIDREPORTLIST,
    //    ZCIXMXFIMQDIDREPORTLISTResponse>(
    //        "ZCIXMXFIMQDIDREPORTLIST", req);

    //        // resp here contains the raw XML as a string or XElement
    //        var xml = resp.ToString();  // or resp.InnerXml depending on type

    //        var ds = new DataSet();
    //        using (var reader = new StringReader(xml))
    //        {
    //            ds.ReadXml(reader);
    //        }

    //        // now you can access the table
    //        var table = ds.Tables["ZCIXMXFIMQDIDREPORTLIST"];
    //        foreach (DataRow row in table.Rows)
    //        {
    //            var didConfig = row["ZDIDCONFIGNUM"]?.ToString();
    //            var status = row["ZREPSTATUS"]?.ToString();
    //            // map to your model
    //        }

    //        // 1) Preferred: parse ZCIXMXFIMQDIDREPORTLISTTBL if present
    //        //if (resp?.ZCIXMXFIMQDIDREPORTLISTTBL != null)
    //        //{
    //        //    var innerXml = resp.ZCIXMXFIMQDIDREPORTLISTTBL.InnerXml;
    //        //    if (!string.IsNullOrWhiteSpace(innerXml))
    //        //    {
    //        //        // Try parse. Some services return <NewDataSet>... or <diffgram>...; handle both.
    //        //        XDocument xdoc = null;
    //        //        try
    //        //        {
    //        //            xdoc = XDocument.Parse(innerXml);
    //        //        }
    //        //        catch
    //        //        {
    //        //            // wrap with root and retry
    //        //            xdoc = XDocument.Parse($"<root>{innerXml}</root>");
    //        //        }

    //        //        // Try find explicit row nodes
    //        //        var rowNodes = xdoc.Descendants()
    //        //                           .Where(n => string.Equals(n.Name.LocalName, "ZCIXMXFIMQDIDREPORTLIST", StringComparison.OrdinalIgnoreCase)
    //        //                                    || n.Elements().Any(e => e.Name.LocalName == "ZHOSTID"))
    //        //                           .ToList();

    //        //        foreach (var node in rowNodes)
    //        //        {
    //        //            var rr = new ReportRow
    //        //            {
    //        //                ZHOSTID = node.Elements().FirstOrDefault(e => e.Name.LocalName == "ZHOSTID")?.Value ?? string.Empty,
    //        //                ZDIDDATANUM = node.Elements().FirstOrDefault(e => e.Name.LocalName == "ZDIDDATANUM")?.Value ?? string.Empty,
    //        //                ZDIDDATAST = node.Elements().FirstOrDefault(e => e.Name.LocalName == "ZDIDDATAST")?.Value ?? string.Empty,
    //        //                ZDIDDESC = node.Elements().FirstOrDefault(e => e.Name.LocalName == "ZDIDDESC")?.Value ?? string.Empty
    //        //            };
    //        //            result.Rows.Add(rr);
    //        //        }
    //        //    }
    //        //}

    //        //// 2) If nothing parsed yet, try Result as a possible inner-xml string
    //        //if (!result.Rows.Any() && !string.IsNullOrWhiteSpace(resp?.Result))
    //        //{
    //        //    var candidate = resp.Result.Trim();
    //        //    if (candidate.StartsWith("<"))
    //        //    {
    //        //        try
    //        //        {
    //        //            var xdoc = XDocument.Parse(candidate);
    //        //            var rowNodes = xdoc.Descendants()
    //        //                               .Where(n => n.Elements().Any(e => e.Name.LocalName == "ZHOSTID"))
    //        //                               .ToList();

    //        //            foreach (var node in rowNodes)
    //        //            {
    //        //                var rr = new ReportRow
    //        //                {
    //        //                    ZHOSTID = node.Elements().FirstOrDefault(e => e.Name.LocalName == "ZHOSTID")?.Value ?? string.Empty,
    //        //                    ZDIDDATANUM = node.Elements().FirstOrDefault(e => e.Name.LocalName == "ZDIDDATANUM")?.Value ?? string.Empty,
    //        //                    ZDIDDATAST = node.Elements().FirstOrDefault(e => e.Name.LocalName == "ZDIDDATAST")?.Value ?? string.Empty,
    //        //                    ZDIDDESC = node.Elements().FirstOrDefault(e => e.Name.LocalName == "ZDIDDESC")?.Value ?? string.Empty
    //        //                };
    //        //                result.Rows.Add(rr);
    //        //            }
    //        //        }
    //        //        catch
    //        //        {
    //        //            // ignore parse errors; leave rows empty and return Total if any
    //        //        }
    //        //    }
    //        //}

    //        // 3) Return (could be empty rows but not null)
    //        return result;
    //    }
    //}

    public async Task<ReportListResponse> GetReportListAsync(string username, string password, int page = 0, int pageSize = 20)
    {
        var req = new ZCIXMXFIMQDIDREPORTLIST
        {
            ZUSERNAME = username,
            ZPASSWORD = password,
            ZAPPID = "ANDROIDPHONEFM",
            ZTRANSID = "DIDTRACK",
            ZTRANSTYPE = "INSPECTION",
            PageNumber = page,
            PageSize = pageSize
        };

        // Call SOAP
        var resp = await _soapClient.InvokeAsync<
            ZCIXMXFIMQDIDREPORTLIST,
            ZCIXMXFIMQDIDREPORTLISTResponse>(
            "ZCIXMXFIMQDIDREPORTLIST", req);

        // Prepare return object
        //var result = new ReportListResponse
        //{
        //    Total = resp?.TotalRowCount ?? 0,
        //    Rows = new List<ReportRow>()
        //};


        var result = new ReportListResponse
        {
            Total = resp?.TotalRowCount ?? 0,
            Rows = new List<ReportRow>()
        };

        // Prefer parsing ZCIXMXFIMQDIDREPORTLISTTBL if available
        if (!string.IsNullOrWhiteSpace(resp?.ZCIXMXFIMQDIDREPORTLISTTBL?.OuterXml))
        {
            var xml = resp.ZCIXMXFIMQDIDREPORTLISTTBL.OuterXml;
            XDocument xdoc;
            try { xdoc = XDocument.Parse(xml); }
            catch { xdoc = XDocument.Parse($"<root>{xml}</root>"); }

            var rowNodes = xdoc.Descendants()
                .Where(n => n.Name.LocalName == "ZCIXMXFIMQDIDREPORTLIST")
                .ToList();

            foreach (var node in rowNodes)
            {
                var rr = new ReportRow
                {
                    ZHOSTID = node.Element("ZHOSTID")?.Value ?? string.Empty,
                    ZDIDDATANUM = node.Element("ZDIDDATANUM")?.Value ?? string.Empty,
                    ZDIDDATAST = node.Element("ZDIDDATAST")?.Value ?? string.Empty,
                    ZDIDDESC = node.Element("ZDIDDESC")?.Value ?? string.Empty
                };
                result.Rows.Add(rr);
            }
        }

        // Fallback: try parsing Result property if no rows found
        if (!result.Rows.Any() && !string.IsNullOrWhiteSpace(resp?.Result))
        {
            var candidate = resp.Result.Trim();
            if (candidate.StartsWith("<"))
            {
                XDocument xdoc;
                try { xdoc = XDocument.Parse(candidate); }
                catch { xdoc = XDocument.Parse($"<root>{candidate}</root>"); }

                var rowNodes = xdoc.Descendants()
                    .Where(n => n.Elements().Any(e => e.Name.LocalName == "ZHOSTID"))
                    .ToList();

                foreach (var node in rowNodes)
                {
                    var rr = new ReportRow
                    {
                        ZHOSTID = node.Element("ZHOSTID")?.Value ?? string.Empty,
                        ZDIDDATANUM = node.Element("ZDIDDATANUM")?.Value ?? string.Empty,
                        ZDIDDATAST = node.Element("ZDIDDATAST")?.Value ?? string.Empty,
                        ZDIDDESC = node.Element("ZDIDDESC")?.Value ?? string.Empty
                    };
                    result.Rows.Add(rr);
                }
            }
        }

        return result;



        //// Try get XML string from response
        //var xml = resp?.Result?.ToString();
        //if (string.IsNullOrWhiteSpace(xml))
        //    return result; // return empty list if no data
        //                   // Debug / log
        //System.Diagnostics.Debug.WriteLine("SOAP XML >>> " + xml);
        //try
        //{
        //    // Read into dataset
        //    var ds = new DataSet();
        //    using (var reader = new StringReader(xml))
        //    {
        //        ds.ReadXml(reader);
        //    }

        //    var table = ds.Tables["ZCIXMXFIMQDIDREPORTLIST"];
        //    if (table != null)
        //    {
        //        foreach (DataRow row in table.Rows)
        //        {
        //            var rr = new ReportRow
        //            {
        //                ZHOSTID = row["ZHOSTID"]?.ToString(),
        //                ZDIDDATANUM = row["ZDIDDATANUM"]?.ToString(),
        //                ZDIDDATAST = row["ZDIDDATAST"]?.ToString(),
        //                ZDIDDESC = row["ZDIDDESC"]?.ToString(),
        //                ZDIDCONFIGNUM = row["ZDIDCONFIGNUM"]?.ToString(),
        //                ZREPSTATUS = row["ZREPSTATUS"]?.ToString(),
        //                ZETA = row["ZETA"]?.ToString()
        //            };

        //            result.Rows.Add(rr);
        //        }s
        //    }
        //}
        //catch (Exception ex)
        //{
        //    // TODO: log exception (_logger.LogError if available)
        //    // but don’t throw, just return empty
        //}

        //return result;
    }

}
