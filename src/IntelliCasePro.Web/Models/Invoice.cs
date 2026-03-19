namespace IntelliCasePro.Web.Models;

public class Invoice
{
    public int Id { get; set; }
    public int CaseFileId { get; set; }
    public CaseFile? CaseFile { get; set; }
    public string InvoiceNumber { get; set; } = "";
    public DateTime IssuedOn { get; set; }
    public DateTime DueOn { get; set; }
    public decimal HoursAmount { get; set; }
    public decimal ExpenseAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public InvoiceStatus Status { get; set; }

    public decimal Total => HoursAmount + ExpenseAmount + TaxAmount;
}
