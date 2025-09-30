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

        // In this simple flow we use test password from env or constant (avoid storing real pwd)
        var password = Environment.GetEnvironmentVariable("CONNIX_TEST_PASSWORD") ?? "#ABCDE12345$";
        var list = await _api.GetReportsAsync(username, password, page, pageSize);
        return View(list);
    }
}
