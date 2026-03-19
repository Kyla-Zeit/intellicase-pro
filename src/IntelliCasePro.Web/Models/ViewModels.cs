using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntelliCasePro.Web.Models;

public class DashboardViewModel
{
    public string UserName { get; set; } = "Jane Doe";
    public int OpenCases { get; set; }
    public int EvidenceCount { get; set; }
    public decimal BillableHoursThisMonth { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public int OverdueTasks { get; set; }
    public int OpenTasks { get; set; }
    public int DueSoonCases { get; set; }
    public int ActiveFieldEventsToday { get; set; }
    public decimal UnbilledTimeValue { get; set; }
    public decimal OutstandingReceivables { get; set; }
    public List<CaseFile> ActiveCases { get; set; } = new();
    public List<CaseTask> PriorityTasks { get; set; } = new();
    public List<DashboardStatusSummary> StatusBreakdown { get; set; } = new();
    public List<DashboardInvestigatorWorkload> InvestigatorWorkloads { get; set; } = new();
    public List<CaseNote> RecentNotes { get; set; } = new();
    public List<EvidenceItem> RecentEvidence { get; set; } = new();
    public List<CalendarEvent> UpcomingEvents { get; set; } = new();
    public List<Invoice> RecentInvoices { get; set; } = new();
}

public class DashboardStatusSummary
{
    public string Label { get; set; } = "";
    public int Count { get; set; }
    public decimal Percentage { get; set; }
}

public class DashboardInvestigatorWorkload
{
    public string Name { get; set; } = "";
    public string Title { get; set; } = "";
    public int ActiveCases { get; set; }
    public int AssignedTasks { get; set; }
    public decimal HoursLast7Days { get; set; }
}

public class CaseListViewModel
{
    public string Search { get; set; } = "";
    public string Status { get; set; } = "";
    public List<SelectListItem> StatusOptions { get; set; } = new();
    public List<CaseFile> Cases { get; set; } = new();
}

public class CaseDetailsViewModel
{
    public CaseFile Case { get; set; } = new();
    public string ActiveTab { get; set; } = "overview";
    public List<CalendarEvent> RelatedEvents { get; set; } = new();
    public List<CaseActivityItemViewModel> RecentActivity { get; set; } = new();
    public AddCaseTaskModel AddTask { get; set; } = new();
    public AddCaseNoteModel AddNote { get; set; } = new();
    public AddTimeEntryModel AddTimeEntry { get; set; } = new();
    public AddExpenseEntryModel AddExpenseEntry { get; set; } = new();
    public AddEvidenceModel AddEvidence { get; set; } = new();
    public decimal TimeValue { get; set; }
    public decimal ExpenseValue { get; set; }
    public decimal InvoiceValue { get; set; }
    public decimal BudgetUsed { get; set; }
    public int OpenTaskCount { get; set; }
    public int CompletedTaskCount { get; set; }
    public int SensitiveEvidenceCount { get; set; }
    public int InternalNoteCount { get; set; }
    public DateTime? NextTaskDue { get; set; }
    public string NextTaskTitle { get; set; } = "";
    public decimal BudgetRemaining => Math.Max(0m, Case.Budget - BudgetUsed);
    public decimal BudgetUtilization => Case.Budget <= 0 ? 0 : Math.Min(100m, Math.Round((BudgetUsed / Case.Budget) * 100m, 1));
    public decimal RetainerRemaining => Math.Max(0m, Case.RetainerAmount - InvoiceValue);
}

public class CaseActivityItemViewModel
{
    public DateTime Timestamp { get; set; }
    public string Title { get; set; } = "";
    public string Detail { get; set; } = "";
    public string Kind { get; set; } = "";
}

public class AddCaseTaskModel
{
    public int CaseFileId { get; set; }
    public string Title { get; set; } = "";
    public string AssignedTo { get; set; } = "";
    public DateTime? DueDate { get; set; }
    public string Notes { get; set; } = "";
}

public class AddCaseNoteModel
{
    public int CaseFileId { get; set; }
    public string Author { get; set; } = "";
    public string Text { get; set; } = "";
    public bool IsInternal { get; set; } = true;
}

public class CaseFormModel : IValidatableObject
{
    [Display(Name = "Case Number")]
    public string CaseNumber { get; set; } = "";

    [Required(ErrorMessage = "Give the file a title.")]
    [StringLength(120)]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "A brief case summary is required.")]
    [StringLength(1000)]
    public string Summary { get; set; } = "";

    [Display(Name = "Case Type")]
    public CaseIntakeType IntakeType { get; set; } = CaseIntakeType.Surveillance;

    public CaseStatus Status { get; set; } = CaseStatus.Intake;
    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

    [Display(Name = "Opened On")]
    [DataType(DataType.Date)]
    public DateTime OpenedOn { get; set; } = DateTime.Today;

    [Display(Name = "Due Date")]
    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    [Required(ErrorMessage = "Enter the subject or target name.")]
    [Display(Name = "Subject Name")]
    [StringLength(120)]
    public string SubjectName { get; set; } = "";

    [StringLength(80)]
    public string Jurisdiction { get; set; } = "Ontario";

    [Display(Name = "Retainer Amount")]
    [Range(0, 1000000)]
    public decimal RetainerAmount { get; set; }

    [Range(0, 1000000)]
    public decimal Budget { get; set; }

    [Display(Name = "Billable case")]
    public bool IsBillable { get; set; } = true;

    [Display(Name = "Use existing client")]
    [Range(0, int.MaxValue)]
    public int ClientId { get; set; }

    [Display(Name = "Lead Investigator")]
    [Range(1, int.MaxValue, ErrorMessage = "Choose a lead investigator.")]
    public int LeadInvestigatorId { get; set; }

    [Display(Name = "Secondary Investigator")]
    public int? SecondaryInvestigatorId { get; set; }

    [Display(Name = "Conflict check completed")]
    public bool ConflictCheckComplete { get; set; }

    [Display(Name = "Authorization signed")]
    public bool AuthorizationSigned { get; set; }

    [Display(Name = "Retainer received")]
    public bool RetainerReceived { get; set; }

    [Display(Name = "Initial evidence received")]
    public bool EvidenceReceived { get; set; }

    [Display(Name = "Intake instructions")]
    [StringLength(1000)]
    public string IntakeInstructions { get; set; } = "";

    [Display(Name = "Create new client instead")]
    public bool CreateNewClient { get; set; }

    [Display(Name = "Client / Contact Name")]
    [StringLength(120)]
    public string NewClientName { get; set; } = "";

    [Display(Name = "Company / Firm")]
    [StringLength(120)]
    public string NewClientCompanyName { get; set; } = "";

    [Display(Name = "Client Email")]
    [EmailAddress]
    public string NewClientEmail { get; set; } = "";

    [Display(Name = "Client Phone")]
    [StringLength(40)]
    public string NewClientPhone { get; set; } = "";

    [Display(Name = "Client Address")]
    [StringLength(200)]
    public string NewClientAddress { get; set; } = "";

    [Display(Name = "Client Notes")]
    [StringLength(500)]
    public string NewClientNotes { get; set; } = "";

    [Display(Name = "Log first evidence item now")]
    public bool CreateInitialEvidence { get; set; }

    [Display(Name = "Evidence Title")]
    [StringLength(120)]
    public string InitialEvidenceTitle { get; set; } = "";

    [Display(Name = "Evidence Type")]
    public EvidenceType InitialEvidenceType { get; set; } = EvidenceType.Document;

    [Display(Name = "Evidence Description")]
    [StringLength(600)]
    public string InitialEvidenceDescription { get; set; } = "";

    [Display(Name = "File Name / Source")]
    [StringLength(160)]
    public string InitialEvidenceFileName { get; set; } = "";

    [Display(Name = "Tags")]
    [StringLength(160)]
    public string InitialEvidenceTags { get; set; } = "";

    [Display(Name = "Storage Location")]
    [StringLength(120)]
    public string InitialEvidenceStorageLocation { get; set; } = "Digital Locker";

    [Display(Name = "Sensitive evidence")]
    public bool InitialEvidenceSensitive { get; set; } = true;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DueDate.HasValue && DueDate.Value.Date < OpenedOn.Date)
        {
            yield return new ValidationResult("Due date cannot be earlier than the opened date.", new[] { nameof(DueDate) });
        }

        if (!CreateNewClient && ClientId <= 0)
        {
            yield return new ValidationResult("Choose an existing client or create a new one.", new[] { nameof(ClientId) });
        }

        if (CreateNewClient && string.IsNullOrWhiteSpace(NewClientName))
        {
            yield return new ValidationResult("Enter the client or contact name for the new client.", new[] { nameof(NewClientName) });
        }

        if (RetainerReceived && RetainerAmount <= 0)
        {
            yield return new ValidationResult("If the retainer is marked as received, enter a retainer amount above zero.", new[] { nameof(RetainerAmount) });
        }

        if (CreateInitialEvidence && string.IsNullOrWhiteSpace(InitialEvidenceTitle))
        {
            yield return new ValidationResult("Add an evidence title or turn off the initial evidence option.", new[] { nameof(InitialEvidenceTitle) });
        }
    }
}

public class ClientFormModel
{
    public string Name { get; set; } = "";
    public string CompanyName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";
    public string Notes { get; set; } = "";
}

public class ExpensePageViewModel
{
    public int SelectedCaseId { get; set; }
    public CaseFile? SelectedCase { get; set; }
    public List<SelectListItem> CaseOptions { get; set; } = new();
    public List<TimeEntry> TimeEntries { get; set; } = new();
    public List<ExpenseEntry> ExpenseEntries { get; set; } = new();
    public AddTimeEntryModel AddTimeEntry { get; set; } = new();
    public AddExpenseEntryModel AddExpenseEntry { get; set; } = new();
    public decimal TotalHours => TimeEntries.Sum(x => x.Hours);
    public decimal TimeTotal => TimeEntries.Sum(x => x.Amount);
    public decimal ExpenseTotal => ExpenseEntries.Sum(x => x.Total);
    public decimal GrandTotal => TimeTotal + ExpenseTotal;
}

public class AddTimeEntryModel
{
    public int CaseFileId { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public string InvestigatorName { get; set; } = "Jane Doe";
    public TimeActivityType ActivityType { get; set; } = TimeActivityType.Surveillance;
    public decimal Hours { get; set; }
    public decimal Rate { get; set; }
    public string Notes { get; set; } = "";
}

public class AddExpenseEntryModel
{
    public int CaseFileId { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public ExpenseCategory Category { get; set; } = ExpenseCategory.Mileage;
    public string Description { get; set; } = "";
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public bool IsBillable { get; set; } = true;
    public string ReceiptNumber { get; set; } = "";
}

public class EvidencePageViewModel
{
    public int SelectedCaseId { get; set; }
    public List<SelectListItem> CaseOptions { get; set; } = new();
    public List<EvidenceItem> EvidenceItems { get; set; } = new();
    public AddEvidenceModel AddEvidence { get; set; } = new();
}

public class AddEvidenceModel
{
    public int CaseFileId { get; set; }
    public string ReferenceNumber { get; set; } = "";
    public string Title { get; set; } = "";
    public EvidenceType Type { get; set; } = EvidenceType.Document;
    public string Description { get; set; } = "";
    public string AddedBy { get; set; } = "Jane Doe";
    public string FileName { get; set; } = "";
    public string Tags { get; set; } = "";
    public string StorageLocation { get; set; } = "Digital Locker";
    public bool IsSensitive { get; set; } = true;
    public string HashValue { get; set; } = "";
}

public class ReportsViewModel
{
    public List<ReportSegmentViewModel> CaseDistribution { get; set; } = new();
    public string CaseDistributionGradient { get; set; } = "conic-gradient(#3b82f6 0 100%)";
    public List<RevenuePointViewModel> RevenueSeries { get; set; } = new();
    public int ClosureRate { get; set; }
    public string ClosureGradient { get; set; } = "conic-gradient(#3b82f6 0 100%)";
    public decimal TotalRevenue { get; set; }
    public int CasesClosedThisYear { get; set; }
    public decimal AverageCaseValue { get; set; }
}

public class ReportSegmentViewModel
{
    public string Label { get; set; } = "";
    public int Value { get; set; }
    public decimal Percentage { get; set; }
    public string Color { get; set; } = "#3b82f6";
}

public class RevenuePointViewModel
{
    public string Month { get; set; } = "";
    public decimal Amount { get; set; }
}

public class CalendarPageViewModel
{
    public List<CalendarEvent> Events { get; set; } = new();
}

public class SettingsViewModel
{
    public string AgencyName { get; set; } = "IntelliCase Pro";
    public string DefaultJurisdiction { get; set; } = "Ontario";
    public bool RequireEvidenceHash { get; set; } = true;
    public bool EnableBillingReminders { get; set; } = true;
    public bool EnableFieldAlerts { get; set; } = true;
}
