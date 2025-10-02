using Connixt.Shared.Models;
using Connixt.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Connixt.UI.Controllers;

[Authorize]
public class ReportController : Controller
{
    private readonly IApiClient _api;
    public ReportController(IApiClient api) => _api = api;

    public async Task<IActionResult> Index(int page = 0, int pageSize = 20) 
    {
        var username = HttpContext.Session.GetString("username");
        if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account");

        var password = Environment.GetEnvironmentVariable("CONNIX_TEST_PASSWORD") ?? "#ABCDE12345$";

        ReportListResponse list;
        try
        {
            list = await _api.GetReportsAsync(username, password, page, pageSize);
            list.Rows ??= new List<ReportRow>();
        }
        catch
        {
            ModelState.AddModelError("", "Unable to load reports at this time.");
            list = new ReportListResponse { Total = 0, Rows = new List<ReportRow>() };
        }

        return View(list);
    }


}
