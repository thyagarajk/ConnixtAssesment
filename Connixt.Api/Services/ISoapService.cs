using Connixt.Shared.Models;

namespace Connixt.Api.Services;

public interface ISoapService
{
    Task<LogonResult> LogonAsync(string username, string password);
    // Task<ReportListResult> GetReportsAsync(string username, string password, int page = 0, int pageSize = 20);
    Task<ReportListResponse> GetReportListAsync(string username, string password, int page = 0, int pageSize = 20);

}

public class LogonResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public string FullName { get; set; } = "";
}

public class ReportListResult
{
    public int Total { get; set; }
    public List<Connixt.Shared.Models.ReportRow> Items { get; set; } = new();
}
