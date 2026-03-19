using IntelliCasePro.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntelliCasePro.Web.Controllers;

public class ReportsController : Controller
{
    private readonly ReportsService _reportsService;

    public ReportsController(ReportsService reportsService)
    {
        _reportsService = reportsService;
    }

    public async Task<IActionResult> Index()
    {
        var vm = await _reportsService.BuildAsync();
        return View(vm);
    }
}
