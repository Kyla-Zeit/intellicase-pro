package com.intellicasepro.mobile.data

data class Investigator(
    val name: String,
    val title: String,
    val email: String,
    val phone: String,
    val isAdmin: Boolean = false
)

data class DashboardMetric(
    val label: String,
    val value: String,
    val detail: String,
    val tone: MetricTone
)

enum class MetricTone {
    Wine,
    Teal,
    Blue,
    Amber,
    Red,
    Slate
}

data class CaseRecord(
    val number: String,
    val title: String,
    val summary: String,
    val status: String,
    val priority: String,
    val client: String,
    val investigator: String,
    val subject: String,
    val jurisdiction: String,
    val opened: String,
    val due: String,
    val lastActivity: String,
    val retainer: Int,
    val budget: Int,
    val spent: Int,
    val tags: List<String>
)

data class TaskRecord(
    val caseNumber: String,
    val title: String,
    val assignedTo: String,
    val due: String,
    val overdue: Boolean,
    val complete: Boolean = false
)

data class EvidenceRecord(
    val caseNumber: String,
    val reference: String,
    val title: String,
    val type: String,
    val addedBy: String,
    val added: String,
    val location: String,
    val sensitive: Boolean,
    val hash: String
)

data class EventRecord(
    val caseNumber: String,
    val title: String,
    val time: String,
    val location: String,
    val assignedTo: String,
    val category: String
)

data class InvoiceRecord(
    val caseNumber: String,
    val number: String,
    val due: String,
    val amount: Int,
    val status: String
)

data class WorkloadRecord(
    val name: String,
    val title: String,
    val activeCases: Int,
    val assignedTasks: Int,
    val hoursLast7Days: Double
)

data class RevenuePoint(
    val month: String,
    val amount: Int
)
