using IntelliCasePro.Web.Data;
using IntelliCasePro.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Controllers;

public class CalendarController : Controller
{
    private readonly AppDbContext _db;

    public CalendarController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var eventsList = await _db.CalendarEvents
            .Include(x => x.CaseFile)
            .OrderBy(x => x.StartsAt)
            .ToListAsync();

        return View(new CalendarPageViewModel
        {
            Events = eventsList
        });
    }
}
