using IntelliCasePro.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntelliCasePro.Web.Controllers;

public class DashboardController : Controller
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var vm = await _dashboardService.BuildAsync();
        return View(vm);
    }
}
