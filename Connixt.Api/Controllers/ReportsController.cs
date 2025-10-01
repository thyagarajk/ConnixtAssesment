using Connixt.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Connixt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ISoapService _soap;
    public ReportsController(ISoapService soap) => _soap = soap;

    // ReportsController (API)
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string username, [FromQuery] string password, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            return BadRequest(new { error = "username & password required" });

        var r = await _soap.GetReportListAsync(username, password, page, pageSize);
        return Ok(new { total = r.Total, rows = r.Rows });
    }
}
