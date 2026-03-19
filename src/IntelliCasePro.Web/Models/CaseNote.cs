namespace IntelliCasePro.Web.Models;

public class CaseNote
{
    public int Id { get; set; }
    public int CaseFileId { get; set; }
    public CaseFile? CaseFile { get; set; }
    public DateTime CreatedOn { get; set; }
    public string Author { get; set; } = "";
    public string Text { get; set; } = "";
    public bool IsInternal { get; set; }
}
