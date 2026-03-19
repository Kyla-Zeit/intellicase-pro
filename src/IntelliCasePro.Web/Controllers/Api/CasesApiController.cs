using IntelliCasePro.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Controllers.Api;

[ApiController]
[Route("api/cases")]
public class CasesApiController : ControllerBase
{
    private readonly AppDbContext _db;

    public CasesApiController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await _db.Cases
            .Include(x => x.Client)
            .Include(x => x.LeadInvestigator)
            .OrderByDescending(x => x.LastActivityOn)
            .Select(x => new
            {
                x.Id,
                x.CaseNumber,
                x.Title,
                Status = x.Status.ToString(),
                Priority = x.Priority.ToString(),
                x.SubjectName,
                Client = x.Client!.Name,
                LeadInvestigator = x.LeadInvestigator!.FullName,
                x.OpenedOn,
                x.DueDate,
                x.LastActivityOn
            })
            .ToListAsync();

        return Ok(items);
    }
}
