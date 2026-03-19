using System.Security.Claims;
using IntelliCasePro.Web.Data;
using IntelliCasePro.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Controllers;

public class EvidenceController : Controller
{
    private readonly AppDbContext _db;

    public EvidenceController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(int? caseId)
    {
        var cases = await _db.Cases.OrderByDescending(x => x.LastActivityOn).ToListAsync();
        var selectedCase = caseId ?? cases.FirstOrDefault()?.Id ?? 0;

        var vm = new EvidencePageViewModel
        {
            SelectedCaseId = selectedCase,
            CaseOptions = cases.Select(x => new SelectListItem($"{x.CaseNumber} - {x.Title}", x.Id.ToString())).ToList(),
            EvidenceItems = await _db.EvidenceItems
                .Include(x => x.CaseFile)
                .Where(x => selectedCase == 0 || x.CaseFileId == selectedCase)
                .OrderByDescending(x => x.AddedOn)
                .ToListAsync(),
            AddEvidence = new AddEvidenceModel
            {
                CaseFileId = selectedCase,
                ReferenceNumber = $"EV-{DateTime.Now:MMddHHmm}"
            }
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddEvidenceModel model)
    {
        var addedBy = User.FindFirstValue(ClaimTypes.Name) ?? model.AddedBy;

        _db.EvidenceItems.Add(new EvidenceItem
        {
            CaseFileId = model.CaseFileId,
            ReferenceNumber = model.ReferenceNumber,
            Title = model.Title,
            Type = model.Type,
            Description = model.Description,
            AddedOn = DateTime.Now,
            AddedBy = addedBy,
            FileName = model.FileName,
            Tags = model.Tags,
            StorageLocation = model.StorageLocation,
            IsSensitive = model.IsSensitive,
            HashValue = string.IsNullOrWhiteSpace(model.HashValue) ? $"SHA256-{Guid.NewGuid():N}"[..20] : model.HashValue
        });

        var caseItem = await _db.Cases.FindAsync(model.CaseFileId);
        if (caseItem is not null)
        {
            caseItem.LastActivityOn = DateTime.Now;
        }

        await _db.SaveChangesAsync();
        TempData["Flash"] = "Evidence item added.";
        return RedirectToAction(nameof(Index), new { caseId = model.CaseFileId });
    }
}
