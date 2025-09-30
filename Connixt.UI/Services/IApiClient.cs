using Connixt.Shared.Models;

namespace Connixt.UI.Services;

public interface IApiClient
{
    Task<(bool success, string? fullName, string? message)> LoginAsync(string username, string password);
    Task<ReportListResponse> GetReportsAsync(string username, string password, int page = 0, int pageSize = 20);
}
