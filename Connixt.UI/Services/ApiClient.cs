using System.Net.Http.Json;
using System.Text.Json;
using Connixt.Shared.Models;

namespace Connixt.UI.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    public ApiClient(HttpClient http) => _http = http;
    public async Task<(bool success, string? fullName, string? message)> LoginAsync(string username, string password)
    {
        var payload = new { Username = username, Password = password };
        var resp = await _http.PostAsJsonAsync("/api/auth/login", payload);

        if (!resp.IsSuccessStatusCode)
        {
            var txt = await resp.Content.ReadAsStringAsync();
            return (false, null, txt);
        }

        var body = await resp.Content.ReadFromJsonAsync<LogonResponse>();

        return (true, body?.FullName, body?.Message);
    }


    public async Task<ReportListResponse> GetReportsAsync(string username, string password, int page = 0, int pageSize = 20)
    {
        var url = $"/api/reports?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}&page={page}&pageSize={pageSize}";

        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resp = await _http.GetFromJsonAsync<ReportListResponse>(url, options);
            return resp ?? new ReportListResponse { Total = 0, Rows = new List<ReportRow>() };
        }
        catch (Exception ex)
        {
                    return new ReportListResponse { Total = 0, Rows = new List<ReportRow>() };
        }
    }

}
