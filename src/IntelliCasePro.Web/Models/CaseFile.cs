namespace IntelliCasePro.Web.Models;

public class CaseFile
{
    public int Id { get; set; }
    public string CaseNumber { get; set; } = "";
    public string Title { get; set; } = "";
    public string Summary { get; set; } = "";
    public CaseStatus Status { get; set; }
    public PriorityLevel Priority { get; set; }
    public DateTime OpenedOn { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime LastActivityOn { get; set; }
    public string SubjectName { get; set; } = "";
    public string Jurisdiction { get; set; } = "";
    public decimal RetainerAmount { get; set; }
    public decimal Budget { get; set; }
    public bool IsBillable { get; set; }

    public int ClientId { get; set; }
    public Client? Client { get; set; }

    public int LeadInvestigatorId { get; set; }
    public InvestigatorUser? LeadInvestigator { get; set; }

    public ICollection<CaseTask> Tasks { get; set; } = new List<CaseTask>();
    public ICollection<EvidenceItem> EvidenceItems { get; set; } = new List<EvidenceItem>();
    public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    public ICollection<ExpenseEntry> ExpenseEntries { get; set; } = new List<ExpenseEntry>();
    public ICollection<CaseNote> Notes { get; set; } = new List<CaseNote>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
