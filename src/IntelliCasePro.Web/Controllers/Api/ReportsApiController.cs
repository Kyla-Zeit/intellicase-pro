using IntelliCasePro.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntelliCasePro.Web.Controllers.Api;

[ApiController]
[Route("api/reports")]
public class ReportsApiController : ControllerBase
{
    private readonly ReportsService _reportsService;

    public ReportsApiController(ReportsService reportsService)
    {
        _reportsService = reportsService;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> Overview()
    {
        var vm = await _reportsService.BuildAsync();

        return Ok(new
        {
            vm.TotalRevenue,
            vm.CasesClosedThisYear,
            vm.AverageCaseValue,
            vm.ClosureRate,
            CaseDistribution = vm.CaseDistribution.Select(x => new { x.Label, x.Value, x.Percentage }),
            RevenueSeries = vm.RevenueSeries
        });
    }
}
