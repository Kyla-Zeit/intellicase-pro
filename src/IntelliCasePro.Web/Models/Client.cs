namespace IntelliCasePro.Web.Models;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string CompanyName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";
    public string Notes { get; set; } = "";

    public ICollection<CaseFile> Cases { get; set; } = new List<CaseFile>();
}
