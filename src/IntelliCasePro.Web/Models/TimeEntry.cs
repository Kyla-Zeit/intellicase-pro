namespace IntelliCasePro.Web.Models;

public class TimeEntry
{
    public int Id { get; set; }
    public int CaseFileId { get; set; }
    public CaseFile? CaseFile { get; set; }
    public string InvestigatorName { get; set; } = "";
    public TimeActivityType ActivityType { get; set; }
    public DateTime Date { get; set; }
    public decimal Hours { get; set; }
    public decimal Rate { get; set; }
    public string Notes { get; set; } = "";
    public bool IsBilled { get; set; }

    public decimal Amount => Hours * Rate;
}
