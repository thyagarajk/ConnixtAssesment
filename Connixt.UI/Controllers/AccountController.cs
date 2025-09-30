using System.Security.Claims;
using Connixt.Shared.Models;
using Connixt.UI.Models;
using Connixt.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Connixt.UI.Controllers;

public class AccountController : Controller
{
    private readonly IApiClient _api;

    public AccountController(IApiClient api) => _api = api;

    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var (success, fullName, message) = await _api.LoginAsync(vm.Username!, vm.Password!);
        if (!success)
        {
            vm.ErrorMessage = message ?? "Login failed";
            return View(vm);
        }

        // sign in user via cookie auth
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, fullName ?? vm.Username!), new Claim("username", vm.Username!) };
        var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(id);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = false, ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20) });
        // also add username to session
        HttpContext.Session.SetString("username", vm.Username!);

        return RedirectToAction("Index", "Report");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
