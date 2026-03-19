using IntelliCasePro.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IntelliCasePro.Web.Controllers;

public class HomeController : Controller
{
    [AllowAnonymous]
    [Route("Home/Error")]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
