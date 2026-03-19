namespace IntelliCasePro.Web.Models;

public class CaseTask
{
    public int Id { get; set; }
    public int CaseFileId { get; set; }
    public CaseFile? CaseFile { get; set; }
    public string Title { get; set; } = "";
    public string AssignedTo { get; set; } = "";
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public string Notes { get; set; } = "";
}
