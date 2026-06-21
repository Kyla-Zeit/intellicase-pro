package com.intellicasepro.mobile.data

object DemoRepository {
    val investigators = listOf(
        Investigator(
            name = "Jane Doe",
            title = "Senior Investigator",
            email = "jane@intellicasepro.local",
            phone = "555-102-2001",
            isAdmin = true
        ),
        Investigator(
            name = "Marcus Hale",
            title = "Field Investigator",
            email = "marcus@intellicasepro.local",
            phone = "555-102-2002"
        ),
        Investigator(
            name = "Priya Shah",
            title = "Analyst",
            email = "priya@intellicasepro.local",
            phone = "555-102-2003"
        )
    )

    val cases = listOf(
        CaseRecord(
            number = "ICP-2026-001",
            title = "Corporate Asset Misappropriation",
            summary = "Discreet surveillance and records analysis regarding suspected asset diversion.",
            status = "Active",
            priority = "High",
            client = "Smith Legal Group",
            investigator = "Jane Doe",
            subject = "R. Thompson",
            jurisdiction = "Ontario",
            opened = "May 10",
            due = "Jul 2",
            lastActivity = "2h ago",
            retainer = 2500,
            budget = 12000,
            spent = 5570,
            tags = listOf("records", "surveillance", "finance")
        ),
        CaseRecord(
            number = "ICP-2026-002",
            title = "Insurance Fraud Verification",
            summary = "Scene review, social media review, and witness canvassing for claim inconsistencies.",
            status = "Surveillance",
            priority = "Critical",
            client = "Northshore Insurance",
            investigator = "Marcus Hale",
            subject = "A. Velez",
            jurisdiction = "Ontario",
            opened = "May 29",
            due = "Jun 27",
            lastActivity = "5h ago",
            retainer = 1800,
            budget = 9000,
            spent = 6120,
            tags = listOf("field", "claims", "urgent")
        ),
        CaseRecord(
            number = "ICP-2026-003",
            title = "Skip Trace and Recovery Support",
            summary = "Locate subject and confirm assets for recovery proceedings.",
            status = "Awaiting Client",
            priority = "Medium",
            client = "Beacon Recovery Services",
            investigator = "Priya Shah",
            subject = "D. Morgan",
            jurisdiction = "Ontario",
            opened = "Jun 5",
            due = "Jul 11",
            lastActivity = "Yesterday",
            retainer = 1000,
            budget = 5400,
            spent = 1890,
            tags = listOf("osint", "recovery", "registry")
        ),
        CaseRecord(
            number = "ICP-2025-044",
            title = "Neighbourhood Witness Canvass",
            summary = "Completed witness canvass and chronology preparation.",
            status = "Closed",
            priority = "Low",
            client = "Smith Legal Group",
            investigator = "Jane Doe",
            subject = "N/A",
            jurisdiction = "Ontario",
            opened = "Mar 29",
            due = "May 1",
            lastActivity = "Closed",
            retainer = 1500,
            budget = 4200,
            spent = 3180,
            tags = listOf("witness", "closed")
        )
    )

    val tasks = listOf(
        TaskRecord("ICP-2026-001", "Review vehicle registration records", "Jane Doe", "Tomorrow", false),
        TaskRecord("ICP-2026-001", "Prepare client update memo", "Priya Shah", "2 days", false),
        TaskRecord("ICP-2026-002", "Night surveillance block 2", "Marcus Hale", "Tonight", false),
        TaskRecord("ICP-2026-002", "Cross-check employer activity", "Priya Shah", "Overdue", true),
        TaskRecord("ICP-2026-003", "Draft recovery lead summary", "Priya Shah", "4 days", false),
        TaskRecord("ICP-2025-044", "Finalize closure package", "Jane Doe", "Done", false, complete = true)
    )

    val evidence = listOf(
        EvidenceRecord(
            caseNumber = "ICP-2026-001",
            reference = "EV-001",
            title = "Parking garage footage",
            type = "Video",
            addedBy = "Jane Doe",
            added = "Jun 15",
            location = "Secure Vault A1",
            sensitive = true,
            hash = "SHA256-DEMO-001"
        ),
        EvidenceRecord(
            caseNumber = "ICP-2026-001",
            reference = "EV-002",
            title = "Expense reimbursement spreadsheet",
            type = "Document",
            addedBy = "Priya Shah",
            added = "Jun 17",
            location = "Digital Locker / Case 001",
            sensitive = true,
            hash = "SHA256-DEMO-002"
        ),
        EvidenceRecord(
            caseNumber = "ICP-2026-002",
            reference = "EV-003",
            title = "Restaurant surveillance stills",
            type = "Photo",
            addedBy = "Marcus Hale",
            added = "Jun 18",
            location = "Digital Locker / Case 002",
            sensitive = true,
            hash = "SHA256-DEMO-003"
        ),
        EvidenceRecord(
            caseNumber = "ICP-2026-003",
            reference = "EV-004",
            title = "Open-source address history",
            type = "Digital Forensic",
            addedBy = "Priya Shah",
            added = "Jun 19",
            location = "Digital Locker / Case 003",
            sensitive = false,
            hash = "SHA256-DEMO-004"
        )
    )

    val events = listOf(
        EventRecord("ICP-2026-001", "Client status briefing", "Today 3:00 PM", "Virtual", "Priya Shah", "Briefing"),
        EventRecord("ICP-2026-002", "Evening surveillance", "Today 7:00 PM", "Mississauga", "Marcus Hale", "Field"),
        EventRecord("ICP-2026-003", "Skip trace review", "Tomorrow 10:00 AM", "Office", "Priya Shah", "Analysis"),
        EventRecord("ICP-2026-001", "Evidence review huddle", "Jun 22 9:00 AM", "Boardroom A", "Jane Doe", "Internal")
    )

    val invoices = listOf(
        InvoiceRecord("ICP-2026-001", "INV-2026-010", "Jul 1", 980, "Sent"),
        InvoiceRecord("ICP-2026-002", "INV-2026-011", "Jul 3", 892, "Sent"),
        InvoiceRecord("ICP-2025-044", "INV-2025-091", "Paid", 287, "Paid")
    )

    val workloads = listOf(
        WorkloadRecord("Jane Doe", "Senior Investigator", 1, 2, 8.5),
        WorkloadRecord("Marcus Hale", "Field Investigator", 1, 1, 5.5),
        WorkloadRecord("Priya Shah", "Analyst", 1, 3, 7.0)
    )

    val revenueSeries = listOf(
        RevenuePoint("Jan", 2400),
        RevenuePoint("Feb", 3100),
        RevenuePoint("Mar", 2800),
        RevenuePoint("Apr", 4200),
        RevenuePoint("May", 5100),
        RevenuePoint("Jun", 2160)
    )

    val dashboardMetrics = listOf(
        DashboardMetric("Open cases", "3", "1 in surveillance", MetricTone.Wine),
        DashboardMetric("Evidence", evidence.size.toString(), "3 sensitive items", MetricTone.Teal),
        DashboardMetric("Billable hours", "19.0", "This month", MetricTone.Blue),
        DashboardMetric("Revenue", "\$2.16k", "Open invoices", MetricTone.Amber),
        DashboardMetric("Overdue", "1", "Task needs attention", MetricTone.Red),
        DashboardMetric("Due soon", "2", "Within 7 days", MetricTone.Slate)
    )

    fun validateLogin(email: String, password: String): Boolean {
        return investigators.any { it.email.equals(email.trim(), ignoreCase = true) } &&
            password == "Demo#2026!"
    }
}
