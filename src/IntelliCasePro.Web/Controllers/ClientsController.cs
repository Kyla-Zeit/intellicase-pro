using IntelliCasePro.Web.Data;
using IntelliCasePro.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Controllers;

public class ClientsController : Controller
{
    private readonly AppDbContext _db;

    public ClientsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var clients = await _db.Clients
            .Include(x => x.Cases)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return View(clients);
    }

    public async Task<IActionResult> Details(int id)
    {
        var client = await _db.Clients
            .Include(x => x.Cases)
            .ThenInclude(x => x.LeadInvestigator)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (client is null)
        {
            return NotFound();
        }

        return View(client);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new ClientFormModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClientFormModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _db.Clients.Add(new Client
        {
            Name = model.Name,
            CompanyName = model.CompanyName,
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address,
            Notes = model.Notes
        });

        await _db.SaveChangesAsync();
        TempData["Flash"] = "Client added.";
        return RedirectToAction(nameof(Index));
    }
}
