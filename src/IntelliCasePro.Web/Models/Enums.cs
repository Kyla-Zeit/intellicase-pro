namespace IntelliCasePro.Web.Models;

public enum CaseStatus
{
    Intake,
    Active,
    Surveillance,
    AwaitingClient,
    Closed,
    Archived
}

public enum PriorityLevel
{
    Low,
    Medium,
    High,
    Critical
}

public enum CaseIntakeType
{
    Surveillance,
    Background,
    Insurance,
    Domestic,
    Corporate,
    LocateSkipTrace,
    DueDiligence,
    FraudReview
}

public enum EvidenceType
{
    Document,
    Photo,
    Video,
    Audio,
    DigitalForensic,
    Receipt
}

public enum TimeActivityType
{
    Surveillance,
    Research,
    Statement,
    CourtPrep,
    FieldVisit,
    Reporting,
    Travel
}

public enum ExpenseCategory
{
    Mileage,
    Travel,
    Meals,
    Lodging,
    Equipment,
    FilingFee,
    Research,
    Misc
}

public enum InvoiceStatus
{
    Draft,
    Sent,
    Paid,
    Overdue
}
