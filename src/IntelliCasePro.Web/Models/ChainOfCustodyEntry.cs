namespace IntelliCasePro.Web.Models;

public class ChainOfCustodyEntry
{
    public int Id { get; set; }
    public int EvidenceItemId { get; set; }
    public EvidenceItem? EvidenceItem { get; set; }
    public DateTime Timestamp { get; set; }
    public string Action { get; set; } = "";
    public string PerformedBy { get; set; } = "";
    public string Notes { get; set; } = "";
}
