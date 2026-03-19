using Microsoft.AspNetCore.Mvc;

namespace IntelliCasePro.Web.Controllers;

public class CommunityController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
