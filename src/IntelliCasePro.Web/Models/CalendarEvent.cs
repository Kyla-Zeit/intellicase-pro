namespace IntelliCasePro.Web.Models;

public class CalendarEvent
{
    public int Id { get; set; }
    public int? CaseFileId { get; set; }
    public CaseFile? CaseFile { get; set; }
    public string Title { get; set; } = "";
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public string Location { get; set; } = "";
    public string AssignedTo { get; set; } = "";
    public string Category { get; set; } = "";
}
