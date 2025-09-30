using Connixt.Api.Services;
using Connixt.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Connixt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISoapService _soap;

    public AuthController(ISoapService soap) => _soap = soap;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LogonRequest req)
    {
        if (req == null || string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Password))
            return BadRequest(new { error = "username & password required" });

        var res = await _soap.LogonAsync(req.Username, req.Password);
        if (!res.Success) return Unauthorized(new { message = res.Message });
        return Ok(new { fullName = res.FullName, message = res.Message });
    }
}
