using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using IntelliCasePro.Web.Data;
using IntelliCasePro.Web.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Controllers;

[AllowAnonymous]
public class AuthController : Controller
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToLocal(returnUrl);
        }

        return View(new LoginViewModel
        {
            ReturnUrl = returnUrl,
            Email = "jane@intellicasepro.local"
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var normalizedEmail = model.Email.Trim().ToLowerInvariant();

        var investigator = await _db.Investigators
            .FirstOrDefaultAsync(x => x.Email.ToLower() == normalizedEmail && x.IsActive);

        if (investigator is null || !PasswordHasher.VerifyPassword(model.Password, investigator.PasswordHash, investigator.PasswordSalt))
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        investigator.LastLoginAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, investigator.Id.ToString()),
            new(ClaimTypes.Name, investigator.FullName),
            new(ClaimTypes.Email, investigator.Email),
            new(ClaimTypes.Role, investigator.IsAdmin ? "Admin" : "Investigator"),
            new("job_title", investigator.Title)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                AllowRefresh = true,
                ExpiresUtc = model.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(14)
                    : DateTimeOffset.UtcNow.AddHours(8)
            });

        TempData["Flash"] = $"Welcome back, {investigator.FullName}.";
        return RedirectToLocal(model.ReturnUrl);
    }


    [HttpGet]
    public IActionResult Denied()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Flash"] = "Signed out.";
        return RedirectToAction(nameof(Login));
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Dashboard");
    }
}

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}
