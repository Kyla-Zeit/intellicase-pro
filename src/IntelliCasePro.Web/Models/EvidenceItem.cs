namespace IntelliCasePro.Web.Models;

public class EvidenceItem
{
    public int Id { get; set; }
    public int CaseFileId { get; set; }
    public CaseFile? CaseFile { get; set; }
    public string ReferenceNumber { get; set; } = "";
    public string Title { get; set; } = "";
    public EvidenceType Type { get; set; }
    public string Description { get; set; } = "";
    public DateTime AddedOn { get; set; }
    public string AddedBy { get; set; } = "";
    public string FileName { get; set; } = "";
    public string Tags { get; set; } = "";
    public string StorageLocation { get; set; } = "";
    public bool IsSensitive { get; set; }
    public string HashValue { get; set; } = "";

    public ICollection<ChainOfCustodyEntry> ChainOfCustodyEntries { get; set; } = new List<ChainOfCustodyEntry>();
}
