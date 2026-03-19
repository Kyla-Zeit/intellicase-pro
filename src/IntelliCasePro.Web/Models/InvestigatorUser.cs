namespace IntelliCasePro.Web.Models;

public class InvestigatorUser
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Title { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public bool IsAdmin { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<CaseFile> Cases { get; set; } = new List<CaseFile>();
}
