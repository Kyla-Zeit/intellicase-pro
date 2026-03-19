using IntelliCasePro.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Controllers;

public class InvoicesController : Controller
{
    private readonly AppDbContext _db;

    public InvoicesController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var invoices = await _db.Invoices
            .Include(x => x.CaseFile)
            .ThenInclude(x => x!.Client)
            .OrderByDescending(x => x.IssuedOn)
            .ToListAsync();

        return View(invoices);
    }
}
