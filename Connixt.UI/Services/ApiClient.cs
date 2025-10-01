using System.Net.Http.Json;
using System.Text.Json;
using Connixt.Shared.Models;

namespace Connixt.UI.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    public ApiClient(HttpClient http) => _http = http;

    //public async Task<(bool success, string? fullName, string? message)> LoginAsync(string username, string password)
    //{
    //    var payload = new { Username = username, Password = password };
    //    var resp = await _http.PostAsJsonAsync("/api/auth/login", payload);
    //    if (!resp.IsSuccessStatusCode)
    //    {
    //        var txt = await resp.Content.ReadAsStringAsync();
    //        return (false, null, txt);
    //    }
    //    var body = await resp.Content.ReadFromJsonAsync<LogonResponse>();

    //    // read fullName
    //    // Read once and awaited properly:
    //    var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, string>>();
    //    json.TryGetValue("fullName", out var fullname);
    //    json.TryGetValue("message", out var message);

    //    return (true, fullname, message);
    //}

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

        // return values from deserialized object, no second read
        return (true, body?.FullName, body?.Message);
    }


    //public async Task<ReportListResponse> GetReportsAsync(string username, string password, int page = 0, int pageSize = 20)
    //{
    //    var url = $"/api/reports?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}&page={page}&pageSize={pageSize}";
    //    var resp = await _http.GetFromJsonAsync<JsonElement>(url);
    //    // parse JSON -> ReportListResponse
    //    var result = new ReportListResponse();
    //    if (resp.TryGetProperty("total", out var totalProp) && totalProp.TryGetInt32(out var total))
    //        result.Total = total;
    //    if (resp.TryGetProperty("rows", out var rowsProp) && rowsProp.ValueKind == JsonValueKind.Array)
    //    {
    //        foreach (var item in rowsProp.EnumerateArray())
    //        {
    //            var rr = new ReportRow
    //            {
    //                ZHOSTID = item.GetProperty("ZHOSTID").GetString(),
    //                ZDIDDATANUM = item.GetProperty("ZDIDDATANUM").GetString(),
    //                ZDIDDATAST = item.GetProperty("ZDIDDATAST").GetString(),
    //                ZDIDDESC = item.GetProperty("ZDIDDESC").GetString()
    //            };
    //            result.Rows.Add(rr);
    //        }
    //    }
    //    return result;
    //}

    // using System.Text.Json;
    // using System.Net.Http.Json;

    public async Task<ReportListResponse> GetReportsAsync(string username, string password, int page = 0, int pageSize = 20)
    {
        var url = $"/api/reports?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}&page={page}&pageSize={pageSize}";

        try
        {
            // Use case-insensitive property matching so the server casing (Total, total, Rows, rows) won't break us.
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resp = await _http.GetFromJsonAsync<ReportListResponse>(url, options);
            return resp ?? new ReportListResponse { Total = 0, Rows = new List<ReportRow>() };
        }
        catch (Exception ex)
        {
           // _logger?.LogError(ex, "Failed to GET {Url}", url);
            return new ReportListResponse { Total = 0, Rows = new List<ReportRow>() };
        }
    }

}
