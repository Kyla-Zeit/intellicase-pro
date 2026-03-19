namespace IntelliCasePro.Web.Models;

public class ExpenseEntry
{
    public int Id { get; set; }
    public int CaseFileId { get; set; }
    public CaseFile? CaseFile { get; set; }
    public DateTime Date { get; set; }
    public ExpenseCategory Category { get; set; }
    public string Description { get; set; } = "";
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public bool IsBillable { get; set; }
    public string ReceiptNumber { get; set; } = "";

    public decimal Total => Quantity * UnitCost;
}
