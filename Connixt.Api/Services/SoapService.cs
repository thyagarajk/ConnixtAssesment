using Connixt.Shared.Models;
using Connixt.Shared.SoapDtos;
using Microsoft.Extensions.Logging;

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
            ZTRANSID = "DIDTRACK",
            ZTRANSTYPE = "INSPECTION",
            PageNumber = page,
            PageSize = pageSize
        };

        var resp = await _soapClient.InvokeAsync<ZCIXMXFIMQDIDREPORTLIST, ZCIXMXFIMQDIDREPORTLISTResponse>(
            "ZCIXMXFIMQDIDREPORTLIST", req);

        // Map SOAP DTO into your shared DTO
        return new ReportListResponse
        {
            Total = resp.TotalRowCount,
            Rows = resp.Result.Diffgram.DocumentElement.Items
        };
    }

}
