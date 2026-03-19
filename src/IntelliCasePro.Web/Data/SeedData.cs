using IntelliCasePro.Web.Models;
using IntelliCasePro.Web.Security;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        if (!await db.Investigators.AnyAsync())
        {
            var seededInvestigators = new List<InvestigatorUser>
            {
                CreateInvestigator("Jane Doe", "Senior Investigator", "jane@intellicasepro.local", "555-102-2001", true),
                CreateInvestigator("Marcus Hale", "Field Investigator", "marcus@intellicasepro.local", "555-102-2002", false),
                CreateInvestigator("Priya Shah", "Analyst", "priya@intellicasepro.local", "555-102-2003", false)
            };

            db.Investigators.AddRange(seededInvestigators);
            await db.SaveChangesAsync();
        }
        else
        {
            await EnsureInvestigatorAccountsAsync(db);
        }

        if (await db.Cases.AnyAsync())
        {
            return;
        }

        var investigators = await db.Investigators.OrderBy(x => x.Id).ToListAsync();

        var clients = new List<Client>
        {
            new() { Name = "Smith Legal Group", CompanyName = "Smith Legal Group", Email = "intake@smithlegal.ca", Phone = "905-555-1100", Address = "18 King Street W, Toronto, ON", Notes = "Corporate and family law referrals." },
            new() { Name = "Northshore Insurance", CompanyName = "Northshore Insurance", Email = "claims@northshore.ca", Phone = "416-555-8022", Address = "74 Adelaide Street E, Toronto, ON", Notes = "Fraud review and witness statements." },
            new() { Name = "Beacon Recovery Services", CompanyName = "Beacon Recovery Services", Email = "ops@beaconrecovery.ca", Phone = "289-555-4411", Address = "90 Lakeshore Road, Mississauga, ON", Notes = "Skip tracing and surveillance support." }
        };

        db.Clients.AddRange(clients);
        await db.SaveChangesAsync();

        var cases = new List<CaseFile>
        {
            new()
            {
                CaseNumber = "ICP-2026-001",
                Title = "Corporate Asset Misappropriation",
                Summary = "Discreet surveillance and records analysis regarding suspected asset diversion.",
                Status = CaseStatus.Active,
                Priority = PriorityLevel.High,
                OpenedOn = DateTime.Today.AddDays(-41),
                DueDate = DateTime.Today.AddDays(12),
                LastActivityOn = DateTime.Today.AddHours(-2),
                ClientId = clients[0].Id,
                LeadInvestigatorId = investigators[0].Id,
                SubjectName = "R. Thompson",
                Jurisdiction = "Ontario",
                RetainerAmount = 2500m,
                Budget = 12000m,
                IsBillable = true
            },
            new()
            {
                CaseNumber = "ICP-2026-002",
                Title = "Insurance Fraud Verification",
                Summary = "Scene review, social media review, and witness canvassing for injury claim inconsistencies.",
                Status = CaseStatus.Surveillance,
                Priority = PriorityLevel.Critical,
                OpenedOn = DateTime.Today.AddDays(-22),
                DueDate = DateTime.Today.AddDays(7),
                LastActivityOn = DateTime.Today.AddHours(-5),
                ClientId = clients[1].Id,
                LeadInvestigatorId = investigators[1].Id,
                SubjectName = "A. Velez",
                Jurisdiction = "Ontario",
                RetainerAmount = 1800m,
                Budget = 9000m,
                IsBillable = true
            },
            new()
            {
                CaseNumber = "ICP-2026-003",
                Title = "Skip Trace and Recovery Support",
                Summary = "Locate subject and confirm assets for recovery proceedings.",
                Status = CaseStatus.AwaitingClient,
                Priority = PriorityLevel.Medium,
                OpenedOn = DateTime.Today.AddDays(-15),
                DueDate = DateTime.Today.AddDays(21),
                LastActivityOn = DateTime.Today.AddDays(-1),
                ClientId = clients[2].Id,
                LeadInvestigatorId = investigators[2].Id,
                SubjectName = "D. Morgan",
                Jurisdiction = "Ontario",
                RetainerAmount = 1000m,
                Budget = 5400m,
                IsBillable = true
            },
            new()
            {
                CaseNumber = "ICP-2025-044",
                Title = "Neighbourhood Witness Canvass",
                Summary = "Completed witness canvass and chronology preparation.",
                Status = CaseStatus.Closed,
                Priority = PriorityLevel.Low,
                OpenedOn = DateTime.Today.AddDays(-83),
                DueDate = DateTime.Today.AddDays(-20),
                LastActivityOn = DateTime.Today.AddDays(-18),
                ClientId = clients[0].Id,
                LeadInvestigatorId = investigators[0].Id,
                SubjectName = "N/A",
                Jurisdiction = "Ontario",
                RetainerAmount = 1500m,
                Budget = 4200m,
                IsBillable = true
            }
        };

        db.Cases.AddRange(cases);
        await db.SaveChangesAsync();

        var tasks = new List<CaseTask>
        {
            new() { CaseFileId = cases[0].Id, Title = "Review vehicle registration records", AssignedTo = "Jane Doe", DueDate = DateTime.Today.AddDays(1), IsCompleted = false, Notes = "Coordinate with legal intake." },
            new() { CaseFileId = cases[0].Id, Title = "Prepare client update memo", AssignedTo = "Priya Shah", DueDate = DateTime.Today.AddDays(2), IsCompleted = false, Notes = "Include timeline and current evidence status." },
            new() { CaseFileId = cases[1].Id, Title = "Night surveillance block 2", AssignedTo = "Marcus Hale", DueDate = DateTime.Today, IsCompleted = false, Notes = "Use sedan, avoid repeat parking positions." },
            new() { CaseFileId = cases[1].Id, Title = "Cross-check employer activity", AssignedTo = "Priya Shah", DueDate = DateTime.Today.AddDays(-1), IsCompleted = false, Notes = "Potential overdue task for dashboard." },
            new() { CaseFileId = cases[2].Id, Title = "Draft recovery lead summary", AssignedTo = "Priya Shah", DueDate = DateTime.Today.AddDays(4), IsCompleted = false, Notes = "Flag likely commercial address hit." },
            new() { CaseFileId = cases[3].Id, Title = "Finalize closure package", AssignedTo = "Jane Doe", DueDate = DateTime.Today.AddDays(-19), IsCompleted = true, Notes = "Done and invoiced." }
        };

        db.Tasks.AddRange(tasks);
        await db.SaveChangesAsync();

        var evidence = new List<EvidenceItem>
        {
            new() { CaseFileId = cases[0].Id, ReferenceNumber = "EV-001", Title = "Parking garage footage", Type = EvidenceType.Video, Description = "Timestamped entrance footage showing suspected transfer vehicle.", AddedOn = DateTime.Today.AddDays(-5), AddedBy = "Jane Doe", FileName = "garage-footage.mp4", Tags = "video,surveillance,vehicle", StorageLocation = "Secure Vault A1", IsSensitive = true, HashValue = "SHA256-DEMO-001" },
            new() { CaseFileId = cases[0].Id, ReferenceNumber = "EV-002", Title = "Expense reimbursement spreadsheet", Type = EvidenceType.Document, Description = "Accounting irregularities highlighted for comparison.", AddedOn = DateTime.Today.AddDays(-3), AddedBy = "Priya Shah", FileName = "reimbursements.xlsx", Tags = "finance,documents", StorageLocation = "Digital Locker / Case 001", IsSensitive = true, HashValue = "SHA256-DEMO-002" },
            new() { CaseFileId = cases[1].Id, ReferenceNumber = "EV-003", Title = "Restaurant surveillance stills", Type = EvidenceType.Photo, Description = "Subject carrying boxes despite reported injury restrictions.", AddedOn = DateTime.Today.AddDays(-2), AddedBy = "Marcus Hale", FileName = "restaurant-stills.zip", Tags = "photo,fieldwork", StorageLocation = "Digital Locker / Case 002", IsSensitive = true, HashValue = "SHA256-DEMO-003" },
            new() { CaseFileId = cases[2].Id, ReferenceNumber = "EV-004", Title = "Open-source address history", Type = EvidenceType.DigitalForensic, Description = "Compiled address and business registry hits.", AddedOn = DateTime.Today.AddDays(-1), AddedBy = "Priya Shah", FileName = "osint-address-history.pdf", Tags = "osint,registry", StorageLocation = "Digital Locker / Case 003", IsSensitive = false, HashValue = "SHA256-DEMO-004" }
        };

        db.EvidenceItems.AddRange(evidence);
        await db.SaveChangesAsync();

        var custody = new List<ChainOfCustodyEntry>
        {
            new() { EvidenceItemId = evidence[0].Id, Timestamp = DateTime.Today.AddDays(-5).AddHours(2), Action = "Received", PerformedBy = "Jane Doe", Notes = "Imported from field upload." },
            new() { EvidenceItemId = evidence[1].Id, Timestamp = DateTime.Today.AddDays(-3).AddHours(3), Action = "Tagged", PerformedBy = "Priya Shah", Notes = "Financial anomaly review bucket." },
            new() { EvidenceItemId = evidence[2].Id, Timestamp = DateTime.Today.AddDays(-2).AddHours(5), Action = "Reviewed", PerformedBy = "Marcus Hale", Notes = "Matches surveillance summary." }
        };

        db.ChainOfCustodyEntries.AddRange(custody);
        await db.SaveChangesAsync();

        var timeEntries = new List<TimeEntry>
        {
            new() { CaseFileId = cases[0].Id, InvestigatorName = "Jane Doe", ActivityType = TimeActivityType.Research, Date = DateTime.Today.AddDays(-6), Hours = 2.5m, Rate = 115m, Notes = "Corporate records pull and chronology prep.", IsBilled = false },
            new() { CaseFileId = cases[0].Id, InvestigatorName = "Jane Doe", ActivityType = TimeActivityType.Surveillance, Date = DateTime.Today.AddDays(-2), Hours = 4.0m, Rate = 125m, Notes = "Garage observation and plate confirmation.", IsBilled = false },
            new() { CaseFileId = cases[1].Id, InvestigatorName = "Marcus Hale", ActivityType = TimeActivityType.Surveillance, Date = DateTime.Today.AddDays(-1), Hours = 5.5m, Rate = 110m, Notes = "Restaurant and parking lot surveillance.", IsBilled = false },
            new() { CaseFileId = cases[1].Id, InvestigatorName = "Priya Shah", ActivityType = TimeActivityType.Reporting, Date = DateTime.Today.AddDays(-1), Hours = 1.75m, Rate = 95m, Notes = "Drafted interim findings memo.", IsBilled = false },
            new() { CaseFileId = cases[2].Id, InvestigatorName = "Priya Shah", ActivityType = TimeActivityType.Research, Date = DateTime.Today.AddDays(-4), Hours = 3.25m, Rate = 95m, Notes = "Address history and corporate lookup.", IsBilled = true },
            new() { CaseFileId = cases[3].Id, InvestigatorName = "Jane Doe", ActivityType = TimeActivityType.Statement, Date = DateTime.Today.AddDays(-21), Hours = 2.0m, Rate = 115m, Notes = "Final witness confirmation call.", IsBilled = true }
        };

        db.TimeEntries.AddRange(timeEntries);
        await db.SaveChangesAsync();

        var expenses = new List<ExpenseEntry>
        {
            new() { CaseFileId = cases[0].Id, Date = DateTime.Today.AddDays(-2), Category = ExpenseCategory.Mileage, Description = "Downtown parking and mileage", Quantity = 1m, UnitCost = 48m, IsBillable = true, ReceiptNumber = "RCPT-1001" },
            new() { CaseFileId = cases[0].Id, Date = DateTime.Today.AddDays(-3), Category = ExpenseCategory.Research, Description = "Corporate registry access", Quantity = 1m, UnitCost = 32m, IsBillable = true, ReceiptNumber = "RCPT-1002" },
            new() { CaseFileId = cases[1].Id, Date = DateTime.Today.AddDays(-1), Category = ExpenseCategory.Meals, Description = "Late surveillance meal", Quantity = 1m, UnitCost = 18m, IsBillable = false, ReceiptNumber = "RCPT-1003" },
            new() { CaseFileId = cases[2].Id, Date = DateTime.Today.AddDays(-4), Category = ExpenseCategory.Research, Description = "Skip trace data pull", Quantity = 1m, UnitCost = 65m, IsBillable = true, ReceiptNumber = "RCPT-1004" },
            new() { CaseFileId = cases[3].Id, Date = DateTime.Today.AddDays(-22), Category = ExpenseCategory.Misc, Description = "Courier package", Quantity = 1m, UnitCost = 24m, IsBillable = true, ReceiptNumber = "RCPT-1005" }
        };

        db.ExpenseEntries.AddRange(expenses);
        await db.SaveChangesAsync();

        var notes = new List<CaseNote>
        {
            new() { CaseFileId = cases[0].Id, CreatedOn = DateTime.Today.AddHours(-4), Author = "Jane Doe", IsInternal = true, Text = "Subject vehicle observed entering north garage level. Cross-reference with reimbursement logs." },
            new() { CaseFileId = cases[1].Id, CreatedOn = DateTime.Today.AddHours(-6), Author = "Marcus Hale", IsInternal = true, Text = "Subject carried multiple boxes with no visible mobility issue. Image stills uploaded." },
            new() { CaseFileId = cases[2].Id, CreatedOn = DateTime.Today.AddDays(-1), Author = "Priya Shah", IsInternal = false, Text = "Client update prepared with likely commercial lead address and registry summary." }
        };

        db.Notes.AddRange(notes);
        await db.SaveChangesAsync();

        var invoices = new List<Invoice>
        {
            new() { CaseFileId = cases[0].Id, InvoiceNumber = "INV-2026-010", IssuedOn = DateTime.Today.AddDays(-3), DueOn = DateTime.Today.AddDays(11), HoursAmount = 787.50m, ExpenseAmount = 80m, TaxAmount = 112.78m, Status = InvoiceStatus.Sent },
            new() { CaseFileId = cases[1].Id, InvoiceNumber = "INV-2026-011", IssuedOn = DateTime.Today.AddDays(-1), DueOn = DateTime.Today.AddDays(13), HoursAmount = 771.25m, ExpenseAmount = 18m, TaxAmount = 102.60m, Status = InvoiceStatus.Sent },
            new() { CaseFileId = cases[3].Id, InvoiceNumber = "INV-2025-091", IssuedOn = DateTime.Today.AddDays(-20), DueOn = DateTime.Today.AddDays(-6), HoursAmount = 230m, ExpenseAmount = 24m, TaxAmount = 33.02m, Status = InvoiceStatus.Paid }
        };

        db.Invoices.AddRange(invoices);
        await db.SaveChangesAsync();

        var calendar = new List<CalendarEvent>
        {
            new() { CaseFileId = cases[0].Id, Title = "Client status briefing", StartsAt = DateTime.Today.AddHours(15), EndsAt = DateTime.Today.AddHours(16), Location = "Virtual", AssignedTo = "Priya Shah", Category = "Briefing" },
            new() { CaseFileId = cases[1].Id, Title = "Evening surveillance", StartsAt = DateTime.Today.AddHours(19), EndsAt = DateTime.Today.AddHours(23), Location = "Mississauga", AssignedTo = "Marcus Hale", Category = "Field" },
            new() { CaseFileId = cases[2].Id, Title = "Skip trace review", StartsAt = DateTime.Today.AddDays(1).AddHours(10), EndsAt = DateTime.Today.AddDays(1).AddHours(11), Location = "Office", AssignedTo = "Priya Shah", Category = "Analysis" },
            new() { CaseFileId = cases[0].Id, Title = "Evidence review huddle", StartsAt = DateTime.Today.AddDays(2).AddHours(9), EndsAt = DateTime.Today.AddDays(2).AddHours(10), Location = "Boardroom A", AssignedTo = "Jane Doe", Category = "Internal" }
        };

        db.CalendarEvents.AddRange(calendar);
        await db.SaveChangesAsync();
    }

    private static InvestigatorUser CreateInvestigator(string fullName, string title, string email, string phone, bool isAdmin)
    {
        var (hash, salt) = PasswordHasher.HashPassword("Demo#2026!");

        return new InvestigatorUser
        {
            FullName = fullName,
            Title = title,
            Email = email,
            Phone = phone,
            IsAdmin = isAdmin,
            IsActive = true,
            PasswordHash = hash,
            PasswordSalt = salt
        };
    }

    private static async Task EnsureInvestigatorAccountsAsync(AppDbContext db)
    {
        var investigators = await db.Investigators.ToListAsync();
        var changed = false;

        foreach (var investigator in investigators)
        {
            if (string.IsNullOrWhiteSpace(investigator.PasswordHash) || string.IsNullOrWhiteSpace(investigator.PasswordSalt))
            {
                var (hash, salt) = PasswordHasher.HashPassword("Demo#2026!");
                investigator.PasswordHash = hash;
                investigator.PasswordSalt = salt;
                changed = true;
            }

            if (!investigator.IsActive)
            {
                investigator.IsActive = true;
                changed = true;
            }
        }

        if (changed)
        {
            await db.SaveChangesAsync();
        }
    }
}
