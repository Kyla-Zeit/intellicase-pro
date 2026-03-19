using System.Security.Claims;
using IntelliCasePro.Web.Data;
using IntelliCasePro.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Services;

public class DashboardService
{
    private readonly AppDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashboardService(AppDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<DashboardViewModel> BuildAsync()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var weekAhead = today.AddDays(7);
        var monthStart = new DateTime(today.Year, today.Month, 1);
        var last7Days = today.AddDays(-7);

        var openCaseQuery = _db.Cases
            .Where(x => x.Status != CaseStatus.Closed && x.Status != CaseStatus.Archived);

        var openCases = await openCaseQuery.CountAsync();
        var evidenceCount = await _db.EvidenceItems.CountAsync();
        var billableHours = (await _db.TimeEntries
            .Where(x => x.Date >= monthStart)
            .Select(x => x.Hours)
            .ToListAsync())
            .Sum();

        var monthlyRevenue = (await _db.Invoices
            .Where(x => x.IssuedOn >= monthStart)
            .Select(x => new { x.HoursAmount, x.ExpenseAmount, x.TaxAmount })
            .ToListAsync())
            .Sum(x => x.HoursAmount + x.ExpenseAmount + x.TaxAmount);

        var openTasks = await _db.Tasks.CountAsync(x => !x.IsCompleted);
        var overdueTasks = await _db.Tasks.CountAsync(x => !x.IsCompleted && x.DueDate.HasValue && x.DueDate.Value < today);
        var dueSoonCases = await openCaseQuery.CountAsync(x => x.DueDate.HasValue && x.DueDate.Value >= today && x.DueDate.Value <= weekAhead);
        var activeFieldEventsToday = await _db.CalendarEvents.CountAsync(x => x.StartsAt >= today && x.StartsAt < tomorrow);

        var unbilledTimeValue = (await _db.TimeEntries
            .Where(x => !x.IsBilled)
            .Select(x => new { x.Hours, x.Rate })
            .ToListAsync())
            .Sum(x => x.Hours * x.Rate);

        var outstandingReceivables = (await _db.Invoices
            .Where(x => x.Status == InvoiceStatus.Sent || x.Status == InvoiceStatus.Overdue)
            .Select(x => new { x.HoursAmount, x.ExpenseAmount, x.TaxAmount })
            .ToListAsync())
            .Sum(x => x.HoursAmount + x.ExpenseAmount + x.TaxAmount);

        var activeCases = await openCaseQuery
            .Include(x => x.Client)
            .Include(x => x.LeadInvestigator)
            .OrderByDescending(x => x.LastActivityOn)
            .Take(5)
            .ToListAsync();

        var priorityTasks = await _db.Tasks
            .Include(x => x.CaseFile)
            .Where(x => !x.IsCompleted)
            .OrderBy(x => x.DueDate ?? DateTime.MaxValue)
            .ThenBy(x => x.Title)
            .Take(6)
            .ToListAsync();

        var recentEvidence = await _db.EvidenceItems
            .Include(x => x.CaseFile)
            .OrderByDescending(x => x.AddedOn)
            .Take(5)
            .ToListAsync();

        var upcomingEvents = await _db.CalendarEvents
            .Include(x => x.CaseFile)
            .Where(x => x.StartsAt >= today)
            .OrderBy(x => x.StartsAt)
            .Take(5)
            .ToListAsync();

        var recentInvoices = await _db.Invoices
            .Include(x => x.CaseFile)
            .OrderByDescending(x => x.IssuedOn)
            .Take(5)
            .ToListAsync();

        var recentNotes = await _db.Notes
            .Include(x => x.CaseFile)
            .OrderByDescending(x => x.CreatedOn)
            .Take(4)
            .ToListAsync();

        var activeCaseStatuses = await openCaseQuery
            .GroupBy(x => x.Status)
            .Select(g => new { Label = g.Key.ToString(), Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToListAsync();

        var statusBreakdown = activeCaseStatuses
            .Select(x => new DashboardStatusSummary
            {
                Label = x.Label,
                Count = x.Count,
                Percentage = openCases == 0 ? 0m : Math.Round((decimal)x.Count / openCases * 100m, 1)
            })
            .ToList();

        var investigators = await _db.Investigators
            .OrderBy(x => x.FullName)
            .ToListAsync();

        var investigatorWorkloads = new List<DashboardInvestigatorWorkload>();
        foreach (var investigator in investigators)
        {
            var activeAssignedCases = await openCaseQuery.CountAsync(x => x.LeadInvestigatorId == investigator.Id);
            var assignedTasks = await _db.Tasks.CountAsync(x => !x.IsCompleted && x.AssignedTo == investigator.FullName);
            var hoursLast7Days = (await _db.TimeEntries
                .Where(x => x.InvestigatorName == investigator.FullName && x.Date >= last7Days)
                .Select(x => x.Hours)
                .ToListAsync())
                .Sum();

            investigatorWorkloads.Add(new DashboardInvestigatorWorkload
            {
                Name = investigator.FullName,
                Title = investigator.Title,
                ActiveCases = activeAssignedCases,
                AssignedTasks = assignedTasks,
                HoursLast7Days = hoursLast7Days
            });
        }

        investigatorWorkloads = investigatorWorkloads
            .OrderByDescending(x => x.ActiveCases)
            .ThenByDescending(x => x.AssignedTasks)
            .ThenByDescending(x => x.HoursLast7Days)
            .ToList();

        var currentUserName = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? "Investigator";

        return new DashboardViewModel
        {
            UserName = currentUserName,
            OpenCases = openCases,
            EvidenceCount = evidenceCount,
            BillableHoursThisMonth = billableHours,
            MonthlyRevenue = monthlyRevenue,
            OverdueTasks = overdueTasks,
            OpenTasks = openTasks,
            DueSoonCases = dueSoonCases,
            ActiveFieldEventsToday = activeFieldEventsToday,
            UnbilledTimeValue = unbilledTimeValue,
            OutstandingReceivables = outstandingReceivables,
            ActiveCases = activeCases,
            PriorityTasks = priorityTasks,
            StatusBreakdown = statusBreakdown,
            InvestigatorWorkloads = investigatorWorkloads,
            RecentNotes = recentNotes,
            RecentEvidence = recentEvidence,
            UpcomingEvents = upcomingEvents,
            RecentInvoices = recentInvoices
        };
    }
}
