using Microsoft.AspNetCore.Authorization;
using IntelliCasePro.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntelliCasePro.Web.Controllers;

[Authorize(Roles = "Admin")]
public class SettingsController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new SettingsViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Save(SettingsViewModel model)
    {
        TempData["Flash"] = "Settings saved in demo mode.";
        return RedirectToAction(nameof(Index));
    }
}
