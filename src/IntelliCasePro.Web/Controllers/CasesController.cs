using System.Security.Claims;
using IntelliCasePro.Web.Data;
using IntelliCasePro.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Controllers;

public class CasesController : Controller
{
    private readonly AppDbContext _db;

    public CasesController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string search = "", string status = "")
    {
        var query = _db.Cases
            .Include(x => x.Client)
            .Include(x => x.LeadInvestigator)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.CaseNumber.Contains(search) ||
                x.Title.Contains(search) ||
                x.SubjectName.Contains(search) ||
                x.Client!.Name.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<CaseStatus>(status, out var parsed))
        {
            query = query.Where(x => x.Status == parsed);
        }

        var cases = await query
            .OrderByDescending(x => x.LastActivityOn)
            .ToListAsync();

        var vm = new CaseListViewModel
        {
            Search = search,
            Status = status,
            Cases = cases,
            StatusOptions = Enum.GetValues<CaseStatus>()
                .Select(x => new SelectListItem(x.ToString(), x.ToString()))
                .ToList()
        };

        return View(vm);
    }

    public async Task<IActionResult> Details(int id, string tab = "overview")
    {
        var vm = await BuildDetailsViewModelAsync(id, tab);
        if (vm is null)
        {
            return NotFound();
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTask([Bind(Prefix = "AddTask")] AddCaseTaskModel model)
    {
        if (model.CaseFileId <= 0 || string.IsNullOrWhiteSpace(model.Title))
        {
            TempData["Flash"] = "Task title is required.";
            return RedirectToAction(nameof(Details), new { id = model.CaseFileId, tab = "tasks" });
        }

        _db.Tasks.Add(new CaseTask
        {
            CaseFileId = model.CaseFileId,
            Title = model.Title.Trim(),
            AssignedTo = string.IsNullOrWhiteSpace(model.AssignedTo)
                ? (User.FindFirstValue(ClaimTypes.Name) ?? "Unassigned")
                : model.AssignedTo.Trim(),
            DueDate = model.DueDate,
            IsCompleted = false,
            Notes = (model.Notes ?? string.Empty).Trim()
        });

        await TouchCaseAsync(model.CaseFileId);
        await _db.SaveChangesAsync();
        TempData["Flash"] = "Task added to case file.";
        return RedirectToAction(nameof(Details), new { id = model.CaseFileId, tab = "tasks" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleTask(int taskId, int caseId, string tab = "tasks")
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId && x.CaseFileId == caseId);
        if (task is null)
        {
            return NotFound();
        }

        task.IsCompleted = !task.IsCompleted;
        await TouchCaseAsync(caseId);
        await _db.SaveChangesAsync();
        TempData["Flash"] = task.IsCompleted ? "Task marked complete." : "Task reopened.";
        return RedirectToAction(nameof(Details), new { id = caseId, tab });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddNote([Bind(Prefix = "AddNote")] AddCaseNoteModel model)
    {
        if (model.CaseFileId <= 0 || string.IsNullOrWhiteSpace(model.Text))
        {
            TempData["Flash"] = "Note text is required.";
            return RedirectToAction(nameof(Details), new { id = model.CaseFileId, tab = "notes" });
        }

        _db.Notes.Add(new CaseNote
        {
            CaseFileId = model.CaseFileId,
            CreatedOn = DateTime.Now,
            Author = User.FindFirstValue(ClaimTypes.Name) ?? model.Author,
            Text = model.Text.Trim(),
            IsInternal = model.IsInternal
        });

        await TouchCaseAsync(model.CaseFileId);
        await _db.SaveChangesAsync();
        TempData["Flash"] = model.IsInternal ? "Internal note added." : "Client-facing note added.";
        return RedirectToAction(nameof(Details), new { id = model.CaseFileId, tab = "notes" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTime([Bind(Prefix = "AddTimeEntry")] AddTimeEntryModel model)
    {
        var investigatorName = User.FindFirstValue(ClaimTypes.Name) ?? model.InvestigatorName;

        _db.TimeEntries.Add(new TimeEntry
        {
            CaseFileId = model.CaseFileId,
            Date = model.Date,
            InvestigatorName = investigatorName,
            ActivityType = model.ActivityType,
            Hours = model.Hours,
            Rate = model.Rate,
            Notes = model.Notes ?? string.Empty,
            IsBilled = false
        });

        await TouchCaseAsync(model.CaseFileId);
        await _db.SaveChangesAsync();
        TempData["Flash"] = "Time entry logged.";
        return RedirectToAction(nameof(Details), new { id = model.CaseFileId, tab = "billing" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddExpense([Bind(Prefix = "AddExpenseEntry")] AddExpenseEntryModel model)
    {
        _db.ExpenseEntries.Add(new ExpenseEntry
        {
            CaseFileId = model.CaseFileId,
            Date = model.Date,
            Category = model.Category,
            Description = model.Description ?? string.Empty,
            Quantity = model.Quantity,
            UnitCost = model.UnitCost,
            IsBillable = model.IsBillable,
            ReceiptNumber = model.ReceiptNumber ?? string.Empty
        });

        await TouchCaseAsync(model.CaseFileId);
        await _db.SaveChangesAsync();
        TempData["Flash"] = "Expense captured.";
        return RedirectToAction(nameof(Details), new { id = model.CaseFileId, tab = "billing" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddEvidence([Bind(Prefix = "AddEvidence")] AddEvidenceModel model)
    {
        var addedBy = User.FindFirstValue(ClaimTypes.Name) ?? model.AddedBy;

        _db.EvidenceItems.Add(new EvidenceItem
        {
            CaseFileId = model.CaseFileId,
            ReferenceNumber = string.IsNullOrWhiteSpace(model.ReferenceNumber) ? $"EV-{DateTime.Now:MMddHHmm}" : model.ReferenceNumber,
            Title = model.Title,
            Type = model.Type,
            Description = model.Description ?? string.Empty,
            AddedOn = DateTime.Now,
            AddedBy = addedBy,
            FileName = model.FileName ?? string.Empty,
            Tags = model.Tags ?? string.Empty,
            StorageLocation = string.IsNullOrWhiteSpace(model.StorageLocation) ? "Digital Locker" : model.StorageLocation,
            IsSensitive = model.IsSensitive,
            HashValue = string.IsNullOrWhiteSpace(model.HashValue) ? $"SHA256-{Guid.NewGuid():N}"[..20] : model.HashValue
        });

        await TouchCaseAsync(model.CaseFileId);
        await _db.SaveChangesAsync();
        TempData["Flash"] = "Evidence item logged.";
        return RedirectToAction(nameof(Details), new { id = model.CaseFileId, tab = "evidence" });
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateDropDownsAsync();
        return View(new CaseFormModel
        {
            CaseNumber = await GenerateCaseNumberAsync(),
            OpenedOn = DateTime.Today,
            DueDate = DateTime.Today.AddDays(7)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CaseFormModel model)
    {
        model.CaseNumber = string.IsNullOrWhiteSpace(model.CaseNumber)
            ? await GenerateCaseNumberAsync()
            : model.CaseNumber.Trim();

        if (await _db.Cases.AnyAsync(x => x.CaseNumber == model.CaseNumber))
        {
            ModelState.AddModelError(nameof(model.CaseNumber), "That case number already exists. Refresh the page and try again.");
        }

        InvestigatorUser? secondaryInvestigator = null;
        if (model.SecondaryInvestigatorId.HasValue && model.SecondaryInvestigatorId.Value > 0)
        {
            secondaryInvestigator = await _db.Investigators.FirstOrDefaultAsync(x => x.Id == model.SecondaryInvestigatorId.Value);
            if (secondaryInvestigator is null)
            {
                ModelState.AddModelError(nameof(model.SecondaryInvestigatorId), "Choose a valid secondary investigator.");
            }
        }

        var leadInvestigator = await _db.Investigators.FirstOrDefaultAsync(x => x.Id == model.LeadInvestigatorId);
        if (leadInvestigator is null)
        {
            ModelState.AddModelError(nameof(model.LeadInvestigatorId), "Choose a valid lead investigator.");
        }

        Client? client = null;
        if (model.CreateNewClient)
        {
            client = new Client
            {
                Name = (model.NewClientName ?? string.Empty).Trim(),
                CompanyName = (model.NewClientCompanyName ?? string.Empty).Trim(),
                Email = (model.NewClientEmail ?? string.Empty).Trim(),
                Phone = (model.NewClientPhone ?? string.Empty).Trim(),
                Address = (model.NewClientAddress ?? string.Empty).Trim(),
                Notes = (model.NewClientNotes ?? string.Empty).Trim()
            };

            if (string.IsNullOrWhiteSpace(client.Name))
            {
                ModelState.AddModelError(nameof(model.NewClientName), "Enter the client or contact name.");
            }
        }
        else
        {
            client = await _db.Clients.FirstOrDefaultAsync(x => x.Id == model.ClientId);
            if (client is null)
            {
                ModelState.AddModelError(nameof(model.ClientId), "Choose a valid client.");
            }
        }

        if (!ModelState.IsValid)
        {
            await PopulateDropDownsAsync();
            return View(model);
        }

        if (model.CreateNewClient && client is not null)
        {
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();
            model.ClientId = client.Id;
        }

        var entity = new CaseFile
        {
            CaseNumber = model.CaseNumber,
            Title = model.Title.Trim(),
            Summary = model.Summary.Trim(),
            Status = model.Status,
            Priority = model.Priority,
            OpenedOn = model.OpenedOn,
            DueDate = model.DueDate,
            LastActivityOn = DateTime.Now,
            SubjectName = model.SubjectName.Trim(),
            Jurisdiction = (model.Jurisdiction ?? string.Empty).Trim(),
            RetainerAmount = model.RetainerAmount,
            Budget = model.Budget,
            IsBillable = model.IsBillable,
            ClientId = model.ClientId,
            LeadInvestigatorId = model.LeadInvestigatorId
        };

        _db.Cases.Add(entity);
        await _db.SaveChangesAsync();

        var leadName = leadInvestigator?.FullName ?? User.FindFirstValue(ClaimTypes.Name) ?? "Lead Investigator";
        var secondaryName = secondaryInvestigator?.FullName;

        var starterTasks = BuildStarterTasks(entity.Id, model, leadName, secondaryName);
        if (starterTasks.Count > 0)
        {
            _db.Tasks.AddRange(starterTasks);
        }

        _db.Notes.Add(new CaseNote
        {
            CaseFileId = entity.Id,
            CreatedOn = DateTime.Now,
            Author = User.FindFirstValue(ClaimTypes.Name) ?? leadName,
            IsInternal = true,
            Text = BuildIntakeSummary(model, client?.Name ?? "Client")
        });

        if (model.CreateInitialEvidence)
        {
            _db.EvidenceItems.Add(new EvidenceItem
            {
                CaseFileId = entity.Id,
                ReferenceNumber = $"EV-{DateTime.Now:MMddHHmm}",
                Title = model.InitialEvidenceTitle.Trim(),
                Type = model.InitialEvidenceType,
                Description = (model.InitialEvidenceDescription ?? string.Empty).Trim(),
                AddedOn = DateTime.Now,
                AddedBy = User.FindFirstValue(ClaimTypes.Name) ?? leadName,
                FileName = (model.InitialEvidenceFileName ?? string.Empty).Trim(),
                Tags = (model.InitialEvidenceTags ?? string.Empty).Trim(),
                StorageLocation = string.IsNullOrWhiteSpace(model.InitialEvidenceStorageLocation)
                    ? "Digital Locker"
                    : model.InitialEvidenceStorageLocation.Trim(),
                IsSensitive = model.InitialEvidenceSensitive,
                HashValue = $"SHA256-{Guid.NewGuid():N}"[..20]
            });
        }

        if (model.DueDate.HasValue)
        {
            _db.CalendarEvents.Add(new CalendarEvent
            {
                CaseFileId = entity.Id,
                Title = $"{entity.CaseNumber} intake follow-up",
                StartsAt = model.DueDate.Value.Date.AddHours(9),
                EndsAt = model.DueDate.Value.Date.AddHours(10),
                Location = "Office / Review Queue",
                AssignedTo = leadName,
                Category = "Intake"
            });
        }

        await _db.SaveChangesAsync();

        TempData["Flash"] = $"Case {entity.CaseNumber} created and ready for intake follow-up.";
        return RedirectToAction(nameof(Details), new { id = entity.Id, tab = "overview" });
    }

    private async Task<CaseDetailsViewModel?> BuildDetailsViewModelAsync(int id, string tab)
    {
        var item = await _db.Cases
            .Include(x => x.Client)
            .Include(x => x.LeadInvestigator)
            .Include(x => x.Tasks)
            .Include(x => x.EvidenceItems)
            .Include(x => x.TimeEntries)
            .Include(x => x.ExpenseEntries)
            .Include(x => x.Notes)
            .Include(x => x.Invoices)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (item is null)
        {
            return null;
        }

        item.Tasks = item.Tasks.OrderBy(x => x.IsCompleted).ThenBy(x => x.DueDate).ToList();
        item.EvidenceItems = item.EvidenceItems.OrderByDescending(x => x.AddedOn).ToList();
        item.TimeEntries = item.TimeEntries.OrderByDescending(x => x.Date).ToList();
        item.ExpenseEntries = item.ExpenseEntries.OrderByDescending(x => x.Date).ToList();
        item.Notes = item.Notes.OrderByDescending(x => x.CreatedOn).ToList();
        item.Invoices = item.Invoices.OrderByDescending(x => x.IssuedOn).ToList();

        var relatedEvents = await _db.CalendarEvents
            .Where(x => x.CaseFileId == item.Id)
            .OrderBy(x => x.StartsAt)
            .ToListAsync();

        var activeTab = NormalizeTab(tab);
        var timeValue = item.TimeEntries.Sum(x => x.Amount);
        var expenseValue = item.ExpenseEntries.Sum(x => x.Total);
        var invoiceValue = item.Invoices.Sum(x => x.Total);
        var budgetUsed = timeValue + expenseValue;
        var openTasks = item.Tasks.Count(x => !x.IsCompleted);
        var completedTasks = item.Tasks.Count(x => x.IsCompleted);
        var nextTaskDue = item.Tasks.Where(x => !x.IsCompleted && x.DueDate.HasValue).OrderBy(x => x.DueDate).FirstOrDefault();

        var activities = new List<CaseActivityItemViewModel>
        {
            new()
            {
                Timestamp = item.OpenedOn,
                Title = "Case opened",
                Detail = $"{item.CaseNumber} was opened for {item.Client?.Name ?? "the client"}.",
                Kind = "Case"
            }
        };

        activities.AddRange(item.Notes.Select(x => new CaseActivityItemViewModel
        {
            Timestamp = x.CreatedOn,
            Title = x.IsInternal ? "Internal note added" : "Case note added",
            Detail = $"{x.Author}: {x.Text}",
            Kind = "Note"
        }));

        activities.AddRange(item.EvidenceItems.Select(x => new CaseActivityItemViewModel
        {
            Timestamp = x.AddedOn,
            Title = $"Evidence logged: {x.Title}",
            Detail = $"{x.Type} · {x.ReferenceNumber} · {x.StorageLocation}",
            Kind = "Evidence"
        }));

        activities.AddRange(item.TimeEntries.Select(x => new CaseActivityItemViewModel
        {
            Timestamp = x.Date,
            Title = $"Time entry: {x.ActivityType}",
            Detail = $"{x.InvestigatorName} logged {x.Hours:0.##}h at {x.Rate:C}.",
            Kind = "Time"
        }));

        activities.AddRange(item.ExpenseEntries.Select(x => new CaseActivityItemViewModel
        {
            Timestamp = x.Date,
            Title = $"Expense captured: {x.Category}",
            Detail = $"{x.Description} · {x.Total:C}",
            Kind = "Expense"
        }));

        activities.AddRange(item.Invoices.Select(x => new CaseActivityItemViewModel
        {
            Timestamp = x.IssuedOn,
            Title = $"Invoice issued: {x.InvoiceNumber}",
            Detail = $"{x.Status} · {x.Total:C}",
            Kind = "Invoice"
        }));

        var vm = new CaseDetailsViewModel
        {
            Case = item,
            ActiveTab = activeTab,
            RelatedEvents = relatedEvents,
            TimeValue = timeValue,
            ExpenseValue = expenseValue,
            InvoiceValue = invoiceValue,
            BudgetUsed = budgetUsed,
            OpenTaskCount = openTasks,
            CompletedTaskCount = completedTasks,
            SensitiveEvidenceCount = item.EvidenceItems.Count(x => x.IsSensitive),
            InternalNoteCount = item.Notes.Count(x => x.IsInternal),
            NextTaskDue = nextTaskDue?.DueDate,
            NextTaskTitle = nextTaskDue?.Title ?? "No pending task due dates",
            RecentActivity = activities
                .OrderByDescending(x => x.Timestamp)
                .Take(10)
                .ToList(),
            AddTask = new AddCaseTaskModel
            {
                CaseFileId = item.Id,
                AssignedTo = User.FindFirstValue(ClaimTypes.Name) ?? item.LeadInvestigator?.FullName ?? "",
                DueDate = item.DueDate
            },
            AddNote = new AddCaseNoteModel
            {
                CaseFileId = item.Id,
                Author = User.FindFirstValue(ClaimTypes.Name) ?? item.LeadInvestigator?.FullName ?? "",
                IsInternal = true
            },
            AddTimeEntry = new AddTimeEntryModel
            {
                CaseFileId = item.Id,
                InvestigatorName = User.FindFirstValue(ClaimTypes.Name) ?? item.LeadInvestigator?.FullName ?? "",
                Rate = 95m
            },
            AddExpenseEntry = new AddExpenseEntryModel
            {
                CaseFileId = item.Id,
                IsBillable = true
            },
            AddEvidence = new AddEvidenceModel
            {
                CaseFileId = item.Id,
                ReferenceNumber = $"EV-{DateTime.Now:MMddHHmm}",
                AddedBy = User.FindFirstValue(ClaimTypes.Name) ?? item.LeadInvestigator?.FullName ?? "",
                StorageLocation = "Digital Locker",
                IsSensitive = true
            }
        };

        return vm;
    }

    private static string NormalizeTab(string? tab)
    {
        return (tab ?? string.Empty).Trim().ToLowerInvariant() switch
        {
            "overview" => "overview",
            "tasks" => "tasks",
            "notes" => "notes",
            "evidence" => "evidence",
            "billing" => "billing",
            _ => "overview"
        };
    }

    private async Task TouchCaseAsync(int caseId)
    {
        var caseItem = await _db.Cases.FindAsync(caseId);
        if (caseItem is not null)
        {
            caseItem.LastActivityOn = DateTime.Now;
        }
    }

    private async Task PopulateDropDownsAsync()
    {
        ViewBag.Clients = (await _db.Clients.OrderBy(x => x.Name).ToListAsync())
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();

        ViewBag.Investigators = (await _db.Investigators.OrderBy(x => x.FullName).ToListAsync())
            .Select(x => new SelectListItem(x.FullName, x.Id.ToString()))
            .ToList();
    }

    private async Task<string> GenerateCaseNumberAsync()
    {
        var year = DateTime.Today.Year;
        var prefix = $"ICP-{year}-";
        var existing = await _db.Cases
            .Where(x => x.CaseNumber.StartsWith(prefix))
            .Select(x => x.CaseNumber)
            .ToListAsync();

        var maxSequence = 0;
        foreach (var caseNumber in existing)
        {
            var parts = caseNumber.Split('-', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3 && int.TryParse(parts[2], out var sequence))
            {
                maxSequence = Math.Max(maxSequence, sequence);
            }
        }

        return $"{prefix}{(maxSequence + 1):000}";
    }

    private static string BuildIntakeSummary(CaseFormModel model, string clientName)
    {
        var checklist = new List<string>
        {
            model.ConflictCheckComplete ? "Conflict check complete" : "Conflict check pending",
            model.AuthorizationSigned ? "Authorization signed" : "Authorization still required",
            model.RetainerReceived ? $"Retainer received ({model.RetainerAmount:C})" : "Retainer not yet received",
            model.EvidenceReceived ? "Initial evidence received" : "Initial evidence still outstanding"
        };

        var instructionText = string.IsNullOrWhiteSpace(model.IntakeInstructions)
            ? "No extra intake instructions were entered."
            : model.IntakeInstructions.Trim();

        return $"Intake created for {model.IntakeType} matter. Client: {clientName}. Checklist: {string.Join("; ", checklist)}. Instructions: {instructionText}";
    }

    private static List<CaseTask> BuildStarterTasks(int caseId, CaseFormModel model, string leadName, string? secondaryName)
    {
        var tasks = new List<CaseTask>();
        var dueDate = model.DueDate ?? model.OpenedOn.AddDays(3);

        if (!model.ConflictCheckComplete)
        {
            tasks.Add(new CaseTask
            {
                CaseFileId = caseId,
                Title = "Complete conflict check",
                AssignedTo = leadName,
                DueDate = model.OpenedOn.AddDays(1),
                IsCompleted = false,
                Notes = "Confirm there is no client, subject, or insurer conflict before deeper field work starts."
            });
        }

        if (!model.AuthorizationSigned)
        {
            tasks.Add(new CaseTask
            {
                CaseFileId = caseId,
                Title = "Obtain signed authorization / engagement",
                AssignedTo = leadName,
                DueDate = model.OpenedOn.AddDays(1),
                IsCompleted = false,
                Notes = "Keep signed paperwork in the case file before moving into external outreach or surveillance."
            });
        }

        if (!model.RetainerReceived && model.IsBillable)
        {
            tasks.Add(new CaseTask
            {
                CaseFileId = caseId,
                Title = "Follow up on retainer",
                AssignedTo = leadName,
                DueDate = model.OpenedOn.AddDays(2),
                IsCompleted = false,
                Notes = "Confirm retainer payment and billing terms for the new file."
            });
        }

        tasks.Add(new CaseTask
        {
            CaseFileId = caseId,
            Title = GetTemplateTaskTitle(model.IntakeType),
            AssignedTo = leadName,
            DueDate = dueDate,
            IsCompleted = false,
            Notes = GetTemplateTaskNotes(model.IntakeType)
        });

        if (!string.IsNullOrWhiteSpace(secondaryName) && !string.Equals(secondaryName, leadName, StringComparison.OrdinalIgnoreCase))
        {
            tasks.Add(new CaseTask
            {
                CaseFileId = caseId,
                Title = "Brief secondary investigator",
                AssignedTo = secondaryName,
                DueDate = model.OpenedOn.AddDays(1),
                IsCompleted = false,
                Notes = "Review scope, safety, reporting expectations, and evidence handling before field deployment."
            });
        }

        return tasks;
    }

    private static string GetTemplateTaskTitle(CaseIntakeType intakeType)
    {
        return intakeType switch
        {
            CaseIntakeType.Surveillance => "Plan surveillance coverage and target schedule",
            CaseIntakeType.Background => "Run background and records sweep",
            CaseIntakeType.Insurance => "Review claimant materials and incident chronology",
            CaseIntakeType.Domestic => "Document relationship timeline and corroborating sources",
            CaseIntakeType.Corporate => "Map stakeholders, access points, and reporting line",
            CaseIntakeType.LocateSkipTrace => "Launch locate and skip-trace workflow",
            CaseIntakeType.DueDiligence => "Start due diligence source review",
            CaseIntakeType.FraudReview => "Build fraud indicators matrix and evidence plan",
            _ => "Begin investigation planning"
        };
    }

    private static string GetTemplateTaskNotes(CaseIntakeType intakeType)
    {
        return intakeType switch
        {
            CaseIntakeType.Surveillance => "Confirm objectives, relevant vehicles, likely locations, timing windows, and evidence capture plan.",
            CaseIntakeType.Background => "Pull civil, corporate, social, and address data before moving into interviews or field verification.",
            CaseIntakeType.Insurance => "Review policy context, claim narrative, and known inconsistencies before scheduling field work.",
            CaseIntakeType.Domestic => "Capture household timeline, known associates, prior incidents, and communication boundaries.",
            CaseIntakeType.Corporate => "Review internal allegations, reporting chain, and document retention needs with discretion.",
            CaseIntakeType.LocateSkipTrace => "Start with databases, utilities, social graph, employers, and last-known address validation.",
            CaseIntakeType.DueDiligence => "Collect business, litigation, regulatory, and media material for the subject entity or person.",
            CaseIntakeType.FraudReview => "Organize suspicious transactions, witness statements, and evidence preservation steps.",
            _ => "Set the first concrete investigation actions and reporting rhythm."
        };
    }
}
