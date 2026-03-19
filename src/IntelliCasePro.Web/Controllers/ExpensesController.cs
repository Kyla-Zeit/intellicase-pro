using System.Security.Claims;
using IntelliCasePro.Web.Data;
using IntelliCasePro.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Controllers;

public class ExpensesController : Controller
{
    private readonly AppDbContext _db;

    public ExpensesController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(int? caseId)
    {
        var vm = await BuildPageViewModelAsync(caseId);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTime([Bind(Prefix = "AddTimeEntry")] AddTimeEntryModel model)
    {
        if (model.CaseFileId <= 0)
        {
            TempData["Flash"] = "Could not save time entry because no case was selected.";
            return RedirectToAction(nameof(Index));
        }

        var caseItem = await _db.Cases.FindAsync(model.CaseFileId);
        if (caseItem is null)
        {
            TempData["Flash"] = "Could not save time entry because the case could not be found.";
            return RedirectToAction(nameof(Index));
        }

        var investigatorName = User.FindFirstValue(ClaimTypes.Name);

        _db.TimeEntries.Add(new TimeEntry
        {
            CaseFileId = model.CaseFileId,
            Date = model.Date,
            InvestigatorName = string.IsNullOrWhiteSpace(investigatorName) ? model.InvestigatorName : investigatorName,
            ActivityType = model.ActivityType,
            Hours = model.Hours,
            Rate = model.Rate,
            Notes = model.Notes,
            IsBilled = false
        });

        caseItem.LastActivityOn = DateTime.Now;

        await _db.SaveChangesAsync();
        TempData["Flash"] = "Time entry added.";
        return RedirectToAction(nameof(Index), new { caseId = model.CaseFileId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddExpense([Bind(Prefix = "AddExpenseEntry")] AddExpenseEntryModel model)
    {
        if (model.CaseFileId <= 0)
        {
            TempData["Flash"] = "Could not save expense entry because no case was selected.";
            return RedirectToAction(nameof(Index));
        }

        var caseItem = await _db.Cases.FindAsync(model.CaseFileId);
        if (caseItem is null)
        {
            TempData["Flash"] = "Could not save expense entry because the case could not be found.";
            return RedirectToAction(nameof(Index));
        }

        _db.ExpenseEntries.Add(new ExpenseEntry
        {
            CaseFileId = model.CaseFileId,
            Date = model.Date,
            Category = model.Category,
            Description = model.Description,
            Quantity = model.Quantity,
            UnitCost = model.UnitCost,
            IsBillable = model.IsBillable,
            ReceiptNumber = model.ReceiptNumber
        });

        caseItem.LastActivityOn = DateTime.Now;

        await _db.SaveChangesAsync();
        TempData["Flash"] = "Expense entry added.";
        return RedirectToAction(nameof(Index), new { caseId = model.CaseFileId });
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTime(int id, int caseId)
    {
        var item = await _db.TimeEntries.FirstOrDefaultAsync(x => x.Id == id && x.CaseFileId == caseId);
        if (item is null)
        {
            TempData["Flash"] = "Time entry not found.";
            return RedirectToAction(nameof(Index), new { caseId });
        }

        _db.TimeEntries.Remove(item);

        var caseItem = await _db.Cases.FindAsync(caseId);
        if (caseItem is not null)
        {
            caseItem.LastActivityOn = DateTime.Now;
        }

        await _db.SaveChangesAsync();
        TempData["Flash"] = "Time entry deleted.";
        return RedirectToAction(nameof(Index), new { caseId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteExpense(int id, int caseId)
    {
        var item = await _db.ExpenseEntries.FirstOrDefaultAsync(x => x.Id == id && x.CaseFileId == caseId);
        if (item is null)
        {
            TempData["Flash"] = "Expense entry not found.";
            return RedirectToAction(nameof(Index), new { caseId });
        }

        _db.ExpenseEntries.Remove(item);

        var caseItem = await _db.Cases.FindAsync(caseId);
        if (caseItem is not null)
        {
            caseItem.LastActivityOn = DateTime.Now;
        }

        await _db.SaveChangesAsync();
        TempData["Flash"] = "Expense entry deleted.";
        return RedirectToAction(nameof(Index), new { caseId });
    }

    private async Task<ExpensePageViewModel> BuildPageViewModelAsync(int? caseId)
    {
        var cases = await _db.Cases
            .Include(x => x.Client)
            .OrderByDescending(x => x.LastActivityOn)
            .ToListAsync();

        var selectedCase = caseId.HasValue
            ? cases.FirstOrDefault(x => x.Id == caseId.Value)
            : cases.FirstOrDefault();

        var vm = new ExpensePageViewModel
        {
            SelectedCaseId = selectedCase?.Id ?? 0,
            SelectedCase = selectedCase,
            CaseOptions = cases.Select(x => new SelectListItem($"{x.CaseNumber} - {x.Title}", x.Id.ToString())).ToList()
        };

        if (selectedCase is not null)
        {
            vm.TimeEntries = await _db.TimeEntries
                .Where(x => x.CaseFileId == selectedCase.Id)
                .OrderByDescending(x => x.Date)
                .ToListAsync();

            vm.ExpenseEntries = await _db.ExpenseEntries
                .Where(x => x.CaseFileId == selectedCase.Id)
                .OrderByDescending(x => x.Date)
                .ToListAsync();

            vm.AddTimeEntry = new AddTimeEntryModel
            {
                CaseFileId = selectedCase.Id,
                InvestigatorName = User.FindFirstValue(ClaimTypes.Name) ?? "Jane Doe",
                Date = DateTime.Today
            };

            vm.AddExpenseEntry = new AddExpenseEntryModel
            {
                CaseFileId = selectedCase.Id,
                Date = DateTime.Today,
                IsBillable = true
            };
        }

        return vm;
    }
}