using IntelliCasePro.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Controllers.Api;

[ApiController]
[Route("api/expenses")]
public class ExpensesApiController : ControllerBase
{
    private readonly AppDbContext _db;

    public ExpensesApiController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> Summary()
    {
        var result = await _db.Cases
            .Select(x => new
            {
                x.Id,
                x.CaseNumber,
                x.Title,
                TimeTotal = x.TimeEntries.Sum(t => t.Hours * t.Rate),
                ExpenseTotal = x.ExpenseEntries.Sum(e => e.Quantity * e.UnitCost)
            })
            .ToListAsync();

        return Ok(result.Select(x => new
        {
            x.Id,
            x.CaseNumber,
            x.Title,
            x.TimeTotal,
            x.ExpenseTotal,
            GrandTotal = x.TimeTotal + x.ExpenseTotal
        }));
    }
}
