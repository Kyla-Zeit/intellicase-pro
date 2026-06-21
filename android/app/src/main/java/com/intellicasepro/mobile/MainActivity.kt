package com.intellicasepro.mobile

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.horizontalScroll
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.Logout
import androidx.compose.material.icons.filled.AccessTime
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.AttachMoney
import androidx.compose.material.icons.filled.BarChart
import androidx.compose.material.icons.filled.CalendarToday
import androidx.compose.material.icons.filled.CheckCircle
import androidx.compose.material.icons.filled.Description
import androidx.compose.material.icons.filled.Folder
import androidx.compose.material.icons.filled.Home
import androidx.compose.material.icons.filled.Lock
import androidx.compose.material.icons.filled.Person
import androidx.compose.material.icons.filled.Receipt
import androidx.compose.material.icons.filled.Search
import androidx.compose.material.icons.filled.Warning
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.ElevatedCard
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.LinearProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.NavigationBar
import androidx.compose.material3.NavigationBarItem
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.material3.TopAppBar
import androidx.compose.material3.TopAppBarDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Brush
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.intellicasepro.mobile.data.CaseRecord
import com.intellicasepro.mobile.data.DashboardMetric
import com.intellicasepro.mobile.data.DemoRepository
import com.intellicasepro.mobile.data.EvidenceRecord
import com.intellicasepro.mobile.data.EventRecord
import com.intellicasepro.mobile.data.InvoiceRecord
import com.intellicasepro.mobile.data.MetricTone
import com.intellicasepro.mobile.data.RevenuePoint
import com.intellicasepro.mobile.data.TaskRecord
import com.intellicasepro.mobile.data.WorkloadRecord
import com.intellicasepro.mobile.ui.theme.AlertRed
import com.intellicasepro.mobile.ui.theme.AppBackground
import com.intellicasepro.mobile.ui.theme.CaseAmber
import com.intellicasepro.mobile.ui.theme.DeepWine
import com.intellicasepro.mobile.ui.theme.FieldTeal
import com.intellicasepro.mobile.ui.theme.FreshWine
import com.intellicasepro.mobile.ui.theme.IntelliCaseTheme
import com.intellicasepro.mobile.ui.theme.SignalBlue
import com.intellicasepro.mobile.ui.theme.Slate
import com.intellicasepro.mobile.ui.theme.SurfaceSoft

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            IntelliCaseTheme {
                IntelliCaseMobileApp()
            }
        }
    }
}

@Composable
private fun IntelliCaseMobileApp() {
    var signedIn by rememberSaveable { mutableStateOf(false) }

    if (signedIn) {
        MainShell(onSignOut = { signedIn = false })
    } else {
        LoginScreen(onSignedIn = { signedIn = true })
    }
}

@Composable
private fun LoginScreen(onSignedIn: () -> Unit) {
    var email by rememberSaveable { mutableStateOf("jane@intellicasepro.local") }
    var password by rememberSaveable { mutableStateOf("Demo#2026!") }
    var error by rememberSaveable { mutableStateOf("") }

    Surface(modifier = Modifier.fillMaxSize(), color = AppBackground) {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .background(
                    Brush.verticalGradient(
                        colors = listOf(Color(0xFFFFF7F8), Color(0xFFF4F8FB), Color.White)
                    )
                )
                .verticalScroll(rememberScrollState())
                .padding(22.dp),
            verticalArrangement = Arrangement.Center
        ) {
            Row(verticalAlignment = Alignment.CenterVertically) {
                Surface(
                    modifier = Modifier.size(56.dp),
                    shape = RoundedCornerShape(18.dp),
                    color = DeepWine
                ) {
                    Icon(
                        imageVector = Icons.Filled.Lock,
                        contentDescription = null,
                        tint = Color.White,
                        modifier = Modifier.padding(14.dp)
                    )
                }
                Spacer(modifier = Modifier.width(14.dp))
                Column {
                    Text(
                        text = "IntelliCase Pro",
                        style = MaterialTheme.typography.headlineSmall,
                        fontWeight = FontWeight.ExtraBold,
                        color = DeepWine
                    )
                    Text(
                        text = "Mobile investigator workspace",
                        style = MaterialTheme.typography.bodyMedium,
                        color = Slate
                    )
                }
            }

            Spacer(modifier = Modifier.height(28.dp))

            ElevatedCard(
                shape = RoundedCornerShape(28.dp),
                colors = CardDefaults.elevatedCardColors(containerColor = Color.White),
                elevation = CardDefaults.elevatedCardElevation(defaultElevation = 8.dp)
            ) {
                Column(
                    modifier = Modifier.padding(22.dp),
                    verticalArrangement = Arrangement.spacedBy(16.dp)
                ) {
                    Text(
                        text = "Sign in",
                        style = MaterialTheme.typography.headlineMedium,
                        fontWeight = FontWeight.Bold
                    )
                    OutlinedTextField(
                        value = email,
                        onValueChange = {
                            email = it
                            error = ""
                        },
                        label = { Text("Email") },
                        leadingIcon = {
                            Icon(Icons.Filled.Person, contentDescription = null)
                        },
                        singleLine = true,
                        modifier = Modifier.fillMaxWidth()
                    )
                    OutlinedTextField(
                        value = password,
                        onValueChange = {
                            password = it
                            error = ""
                        },
                        label = { Text("Password") },
                        leadingIcon = {
                            Icon(Icons.Filled.Lock, contentDescription = null)
                        },
                        visualTransformation = PasswordVisualTransformation(),
                        singleLine = true,
                        modifier = Modifier.fillMaxWidth()
                    )
                    if (error.isNotBlank()) {
                        Text(text = error, color = AlertRed, fontWeight = FontWeight.SemiBold)
                    }
                    Button(
                        onClick = {
                            if (DemoRepository.validateLogin(email, password)) {
                                onSignedIn()
                            } else {
                                error = "Use one of the seeded demo accounts."
                            }
                        },
                        colors = ButtonDefaults.buttonColors(containerColor = DeepWine),
                        shape = RoundedCornerShape(16.dp),
                        contentPadding = PaddingValues(16.dp),
                        modifier = Modifier.fillMaxWidth()
                    ) {
                        Text(text = "Open workspace", fontWeight = FontWeight.Bold)
                    }
                    Surface(
                        color = SurfaceSoft,
                        shape = RoundedCornerShape(18.dp),
                        modifier = Modifier.fillMaxWidth()
                    ) {
                        Column(modifier = Modifier.padding(16.dp)) {
                            Text(
                                text = "Demo access",
                                style = MaterialTheme.typography.labelLarge,
                                color = DeepWine,
                                fontWeight = FontWeight.Bold
                            )
                            Text(
                                text = "jane@intellicasepro.local / Demo#2026!",
                                style = MaterialTheme.typography.bodyMedium,
                                color = Slate
                            )
                        }
                    }
                }
            }
        }
    }
}

private data class AppScreen(
    val route: String,
    val label: String,
    val icon: ImageVector
)

private val dashboardDestination = AppScreen("dashboard", "Dashboard", Icons.Filled.Home)
private val casesDestination = AppScreen("cases", "Cases", Icons.Filled.Folder)
private val evidenceDestination = AppScreen("evidence", "Evidence", Icons.Filled.Description)
private val calendarDestination = AppScreen("calendar", "Calendar", Icons.Filled.CalendarToday)
private val billingDestination = AppScreen("billing", "Billing", Icons.Filled.Receipt)
private val reportsDestination = AppScreen("reports", "Reports", Icons.Filled.BarChart)
private val appScreens = listOf(
    dashboardDestination,
    casesDestination,
    evidenceDestination,
    calendarDestination,
    billingDestination,
    reportsDestination
)

private fun appScreenFromRoute(route: String): AppScreen {
    return appScreens.firstOrNull { it.route == route } ?: dashboardDestination
}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
private fun MainShell(onSignOut: () -> Unit) {
    var selectedRoute by rememberSaveable { mutableStateOf(dashboardDestination.route) }
    val current = appScreenFromRoute(selectedRoute)

    Scaffold(
        topBar = {
            TopAppBar(
                title = {
                    Column {
                        Text(text = current.label, fontWeight = FontWeight.ExtraBold)
                        Text(
                            text = "Jane Doe - Senior Investigator",
                            style = MaterialTheme.typography.labelMedium,
                            color = Slate
                        )
                    }
                },
                actions = {
                    Surface(
                        shape = RoundedCornerShape(50),
                        color = SurfaceSoft,
                        modifier = Modifier.padding(end = 4.dp)
                    ) {
                        Row(
                            verticalAlignment = Alignment.CenterVertically,
                            modifier = Modifier.padding(horizontal = 10.dp, vertical = 7.dp)
                        ) {
                            Icon(
                                imageVector = Icons.Filled.CheckCircle,
                                contentDescription = null,
                                tint = FieldTeal,
                                modifier = Modifier.size(16.dp)
                            )
                            Spacer(modifier = Modifier.width(6.dp))
                            Text("Admin", style = MaterialTheme.typography.labelMedium)
                        }
                    }
                    IconButton(onClick = onSignOut) {
                        Icon(Icons.AutoMirrored.Filled.Logout, contentDescription = "Sign out")
                    }
                },
                colors = TopAppBarDefaults.topAppBarColors(
                    containerColor = Color.White,
                    titleContentColor = DeepWine,
                    actionIconContentColor = DeepWine
                )
            )
        },
        bottomBar = {
            NavigationBar(containerColor = Color.White, tonalElevation = 10.dp) {
                appScreens.forEach { screen ->
                    NavigationBarItem(
                        selected = selectedRoute == screen.route,
                        onClick = { selectedRoute = screen.route },
                        icon = { Icon(screen.icon, contentDescription = screen.label) },
                        label = { Text(screen.label, maxLines = 1, overflow = TextOverflow.Ellipsis) }
                    )
                }
            }
        }
    ) { innerPadding ->
        Box(
            modifier = Modifier
                .fillMaxSize()
                .background(AppBackground)
                .padding(innerPadding)
        ) {
            when (current.route) {
                dashboardDestination.route -> DashboardScreen()
                casesDestination.route -> CasesScreen()
                evidenceDestination.route -> EvidenceScreen()
                calendarDestination.route -> CalendarScreen()
                billingDestination.route -> BillingScreen()
                reportsDestination.route -> ReportsScreen()
            }
        }
    }
}

@Composable
private fun DashboardScreen() {
    LazyColumn(
        contentPadding = PaddingValues(18.dp),
        verticalArrangement = Arrangement.spacedBy(16.dp)
    ) {
        item {
            HeroCard()
        }
        item {
            MetricsGrid(DemoRepository.dashboardMetrics)
        }
        item {
            SectionTitle("Priority cases", "Live workload")
        }
        items(DemoRepository.cases.filter { it.status != "Closed" }, key = { it.number }) { case ->
            CaseSummaryCard(caseRecord = case, onClick = null)
        }
        item {
            SectionTitle("Team load", "Next 7 days")
        }
        items(DemoRepository.workloads, key = { it.name }) { workload ->
            WorkloadCard(workload)
        }
    }
}

@Composable
private fun HeroCard() {
    ElevatedCard(
        modifier = Modifier.fillMaxWidth(),
        shape = RoundedCornerShape(28.dp),
        colors = CardDefaults.elevatedCardColors(containerColor = DeepWine)
    ) {
        Column(
            modifier = Modifier
                .background(
                    Brush.linearGradient(
                        colors = listOf(DeepWine, FreshWine, FieldTeal.copy(alpha = 0.95f))
                    )
                )
                .padding(22.dp),
            verticalArrangement = Arrangement.spacedBy(14.dp)
        ) {
            Text(
                text = "Today",
                style = MaterialTheme.typography.labelLarge,
                color = Color.White.copy(alpha = 0.78f),
                fontWeight = FontWeight.Bold
            )
            Text(
                text = "2 field events, 1 overdue task",
                style = MaterialTheme.typography.headlineSmall,
                color = Color.White,
                fontWeight = FontWeight.ExtraBold
            )
            Text(
                text = "Evening surveillance starts at 7:00 PM in Mississauga.",
                color = Color.White.copy(alpha = 0.82f)
            )
            Row(
                modifier = Modifier.horizontalScroll(rememberScrollState()),
                horizontalArrangement = Arrangement.spacedBy(10.dp)
            ) {
                Pill("Secure vault active", Color.White.copy(alpha = 0.22f), Color.White)
                Pill("Ontario files", Color.White.copy(alpha = 0.22f), Color.White)
                Pill("Billing ready", Color.White.copy(alpha = 0.22f), Color.White)
            }
        }
    }
}

@Composable
private fun MetricsGrid(metrics: List<DashboardMetric>) {
    Column(verticalArrangement = Arrangement.spacedBy(12.dp)) {
        metrics.chunked(2).forEach { row ->
            Row(horizontalArrangement = Arrangement.spacedBy(12.dp)) {
                row.forEach { metric ->
                    MetricTile(metric, modifier = Modifier.weight(1f))
                }
                if (row.size == 1) {
                    Spacer(modifier = Modifier.weight(1f))
                }
            }
        }
    }
}

@Composable
private fun MetricTile(metric: DashboardMetric, modifier: Modifier = Modifier) {
    val color = metricTone(metric.tone)
    ElevatedCard(
        modifier = modifier,
        shape = RoundedCornerShape(22.dp),
        colors = CardDefaults.elevatedCardColors(containerColor = Color.White)
    ) {
        Column(
            modifier = Modifier.padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(8.dp)
        ) {
            Box(
                modifier = Modifier
                    .size(10.dp)
                    .clip(CircleShape)
                    .background(color)
            )
            Text(text = metric.label, color = Slate, style = MaterialTheme.typography.labelMedium)
            Text(
                text = metric.value,
                color = color,
                fontSize = 24.sp,
                fontWeight = FontWeight.ExtraBold
            )
            Text(text = metric.detail, color = Slate, style = MaterialTheme.typography.bodySmall)
        }
    }
}

@Composable
private fun SectionTitle(title: String, eyebrow: String) {
    Column(verticalArrangement = Arrangement.spacedBy(2.dp)) {
        Text(
            text = eyebrow.uppercase(),
            style = MaterialTheme.typography.labelSmall,
            color = FieldTeal,
            fontWeight = FontWeight.Bold
        )
        Text(
            text = title,
            style = MaterialTheme.typography.titleLarge,
            fontWeight = FontWeight.ExtraBold,
            color = DeepWine
        )
    }
}

@Composable
private fun CaseSummaryCard(caseRecord: CaseRecord, onClick: (() -> Unit)?) {
    val modifier = Modifier
        .fillMaxWidth()
        .then(if (onClick != null) Modifier.clickable { onClick() } else Modifier)
    val progress = (caseRecord.spent.toFloat() / caseRecord.budget.toFloat()).coerceIn(0f, 1f)

    ElevatedCard(
        modifier = modifier,
        shape = RoundedCornerShape(24.dp),
        colors = CardDefaults.elevatedCardColors(containerColor = Color.White),
        elevation = CardDefaults.elevatedCardElevation(defaultElevation = 2.dp)
    ) {
        Column(
            modifier = Modifier.padding(18.dp),
            verticalArrangement = Arrangement.spacedBy(14.dp)
        ) {
            Row(verticalAlignment = Alignment.Top) {
                Column(modifier = Modifier.weight(1f)) {
                    Text(
                        text = caseRecord.number,
                        style = MaterialTheme.typography.labelMedium,
                        color = FieldTeal,
                        fontWeight = FontWeight.Bold
                    )
                    Text(
                        text = caseRecord.title,
                        style = MaterialTheme.typography.titleMedium,
                        fontWeight = FontWeight.ExtraBold,
                        maxLines = 2,
                        overflow = TextOverflow.Ellipsis
                    )
                    Text(
                        text = caseRecord.client,
                        style = MaterialTheme.typography.bodySmall,
                        color = Slate
                    )
                }
                StatusChip(caseRecord.status)
            }
            Text(
                text = caseRecord.summary,
                style = MaterialTheme.typography.bodyMedium,
                color = Slate,
                maxLines = 2,
                overflow = TextOverflow.Ellipsis
            )
            Row(
                modifier = Modifier.horizontalScroll(rememberScrollState()),
                horizontalArrangement = Arrangement.spacedBy(10.dp)
            ) {
                Pill(caseRecord.priority, priorityColor(caseRecord.priority).copy(alpha = 0.14f), priorityColor(caseRecord.priority))
                Pill("Due ${caseRecord.due}", SurfaceSoft, Slate)
                Pill(caseRecord.investigator, SurfaceSoft, Slate)
            }
            Column(verticalArrangement = Arrangement.spacedBy(7.dp)) {
                Row {
                    Text("Budget used", color = Slate, style = MaterialTheme.typography.labelMedium)
                    Spacer(modifier = Modifier.weight(1f))
                    Text(
                        "${money(caseRecord.spent)} / ${money(caseRecord.budget)}",
                        fontWeight = FontWeight.Bold,
                        style = MaterialTheme.typography.labelMedium
                    )
                }
                LinearProgressIndicator(
                    progress = { progress },
                    modifier = Modifier
                        .fillMaxWidth()
                        .height(8.dp)
                        .clip(RoundedCornerShape(50)),
                    color = FieldTeal,
                    trackColor = SurfaceSoft
                )
            }
        }
    }
}

@Composable
private fun CasesScreen() {
    var query by rememberSaveable { mutableStateOf("") }
    var selectedCase by remember { mutableStateOf<CaseRecord?>(null) }
    val filtered = DemoRepository.cases.filter {
        query.isBlank() ||
            it.number.contains(query, ignoreCase = true) ||
            it.title.contains(query, ignoreCase = true) ||
            it.client.contains(query, ignoreCase = true) ||
            it.subject.contains(query, ignoreCase = true)
    }

    selectedCase?.let { caseRecord ->
        CaseDetailScreen(caseRecord = caseRecord, onBack = { selectedCase = null })
        return
    }

    LazyColumn(
        contentPadding = PaddingValues(18.dp),
        verticalArrangement = Arrangement.spacedBy(14.dp)
    ) {
        item {
            OutlinedTextField(
                value = query,
                onValueChange = { query = it },
                label = { Text("Search cases") },
                leadingIcon = { Icon(Icons.Filled.Search, contentDescription = null) },
                singleLine = true,
                modifier = Modifier.fillMaxWidth()
            )
        }
        item {
            Row(
                modifier = Modifier.horizontalScroll(rememberScrollState()),
                horizontalArrangement = Arrangement.spacedBy(10.dp)
            ) {
                Pill("${filtered.size} files", SurfaceSoft, DeepWine)
                Pill("Tap a case", SurfaceSoft, Slate)
            }
        }
        items(filtered, key = { it.number }) { caseRecord ->
            CaseSummaryCard(caseRecord = caseRecord, onClick = { selectedCase = caseRecord })
        }
    }
}

@Composable
private fun CaseDetailScreen(caseRecord: CaseRecord, onBack: () -> Unit) {
    val tasks = DemoRepository.tasks.filter { it.caseNumber == caseRecord.number }
    val evidence = DemoRepository.evidence.filter { it.caseNumber == caseRecord.number }
    val invoice = DemoRepository.invoices.firstOrNull { it.caseNumber == caseRecord.number }

    LazyColumn(
        contentPadding = PaddingValues(18.dp),
        verticalArrangement = Arrangement.spacedBy(14.dp)
    ) {
        item {
            TextButton(onClick = onBack) {
                Text("Back to cases")
            }
        }
        item {
            ElevatedCard(
                shape = RoundedCornerShape(26.dp),
                colors = CardDefaults.elevatedCardColors(containerColor = Color.White)
            ) {
                Column(
                    modifier = Modifier.padding(20.dp),
                    verticalArrangement = Arrangement.spacedBy(14.dp)
                ) {
                    Row(verticalAlignment = Alignment.Top) {
                        Column(modifier = Modifier.weight(1f)) {
                            Text(caseRecord.number, color = FieldTeal, fontWeight = FontWeight.Bold)
                            Text(
                                caseRecord.title,
                                style = MaterialTheme.typography.headlineSmall,
                                fontWeight = FontWeight.ExtraBold
                            )
                        }
                        StatusChip(caseRecord.status)
                    }
                    Text(caseRecord.summary, color = Slate)
                    InfoRow("Client", caseRecord.client)
                    InfoRow("Subject", caseRecord.subject)
                    InfoRow("Lead", caseRecord.investigator)
                    InfoRow("Jurisdiction", caseRecord.jurisdiction)
                }
            }
        }
        item {
            MetricsGrid(
                listOf(
                    DashboardMetric("Tasks", tasks.count { !it.complete }.toString(), "Open items", MetricTone.Blue),
                    DashboardMetric("Evidence", evidence.size.toString(), "Registered", MetricTone.Teal),
                    DashboardMetric("Retainer", money(caseRecord.retainer), "Received", MetricTone.Amber),
                    DashboardMetric("Spent", money(caseRecord.spent), "Budget use", MetricTone.Wine)
                )
            )
        }
        item {
            SectionTitle("Tasks", "case action")
        }
        items(tasks, key = { it.title }) { task ->
            TaskCard(task)
        }
        item {
            SectionTitle("Evidence", "chain of custody")
        }
        items(evidence, key = { it.reference }) { record ->
            EvidenceCard(record)
        }
        invoice?.let {
            item {
                SectionTitle("Invoice", "billing")
            }
            item {
                InvoiceCard(it)
            }
        }
    }
}

@Composable
private fun EvidenceScreen() {
    LazyColumn(
        contentPadding = PaddingValues(18.dp),
        verticalArrangement = Arrangement.spacedBy(14.dp)
    ) {
        item {
            Row(verticalAlignment = Alignment.CenterVertically) {
                SectionTitle("Evidence register", "secure intake")
                Spacer(modifier = Modifier.weight(1f))
                OutlinedButton(onClick = { }) {
                    Icon(Icons.Filled.Add, contentDescription = null, modifier = Modifier.size(18.dp))
                    Spacer(modifier = Modifier.width(6.dp))
                    Text("Log")
                }
            }
        }
        items(DemoRepository.evidence, key = { it.reference }) { record ->
            EvidenceCard(record)
        }
    }
}

@Composable
private fun CalendarScreen() {
    LazyColumn(
        contentPadding = PaddingValues(18.dp),
        verticalArrangement = Arrangement.spacedBy(14.dp)
    ) {
        item {
            SectionTitle("Field calendar", "today and upcoming")
        }
        items(DemoRepository.events, key = { it.title + it.time }) { event ->
            EventCard(event)
        }
    }
}

@Composable
private fun BillingScreen() {
    val outstanding = DemoRepository.invoices.filter { it.status != "Paid" }.sumOf { it.amount }
    val paid = DemoRepository.invoices.filter { it.status == "Paid" }.sumOf { it.amount }

    LazyColumn(
        contentPadding = PaddingValues(18.dp),
        verticalArrangement = Arrangement.spacedBy(14.dp)
    ) {
        item {
            MetricsGrid(
                listOf(
                    DashboardMetric("Outstanding", money(outstanding), "Sent invoices", MetricTone.Amber),
                    DashboardMetric("Paid", money(paid), "Closed files", MetricTone.Teal),
                    DashboardMetric("Unbilled", money(2460), "Time and expenses", MetricTone.Blue),
                    DashboardMetric("Billable", "19.0h", "This month", MetricTone.Wine)
                )
            )
        }
        item {
            SectionTitle("Invoices", "billing queue")
        }
        items(DemoRepository.invoices, key = { it.number }) { invoice ->
            InvoiceCard(invoice)
        }
    }
}

@Composable
private fun ReportsScreen() {
    LazyColumn(
        contentPadding = PaddingValues(18.dp),
        verticalArrangement = Arrangement.spacedBy(16.dp)
    ) {
        item {
            SectionTitle("Reports", "agency snapshot")
        }
        item {
            ElevatedCard(
                shape = RoundedCornerShape(24.dp),
                colors = CardDefaults.elevatedCardColors(containerColor = Color.White)
            ) {
                Column(
                    modifier = Modifier.padding(18.dp),
                    verticalArrangement = Arrangement.spacedBy(16.dp)
                ) {
                    Text("Case distribution", fontWeight = FontWeight.ExtraBold)
                    ReportProgressRow("Active", 1, 4, DeepWine)
                    ReportProgressRow("Surveillance", 1, 4, FieldTeal)
                    ReportProgressRow("Awaiting client", 1, 4, CaseAmber)
                    ReportProgressRow("Closed", 1, 4, SignalBlue)
                }
            }
        }
        item {
            ElevatedCard(
                shape = RoundedCornerShape(24.dp),
                colors = CardDefaults.elevatedCardColors(containerColor = Color.White)
            ) {
                Column(
                    modifier = Modifier.padding(18.dp),
                    verticalArrangement = Arrangement.spacedBy(14.dp)
                ) {
                    Text("Revenue trend", fontWeight = FontWeight.ExtraBold)
                    RevenueBars(DemoRepository.revenueSeries)
                }
            }
        }
        item {
            MetricsGrid(
                listOf(
                    DashboardMetric("Closure rate", "25%", "Demo year", MetricTone.Teal),
                    DashboardMetric("Avg case", money(3490), "Rolling value", MetricTone.Blue)
                )
            )
        }
    }
}

@Composable
private fun WorkloadCard(record: WorkloadRecord) {
    ElevatedCard(
        shape = RoundedCornerShape(22.dp),
        colors = CardDefaults.elevatedCardColors(containerColor = Color.White)
    ) {
        Column(
            modifier = Modifier.padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(12.dp)
        ) {
            Row(verticalAlignment = Alignment.CenterVertically) {
                Surface(shape = CircleShape, color = DeepWine.copy(alpha = 0.12f)) {
                    Icon(
                        Icons.Filled.Person,
                        contentDescription = null,
                        tint = DeepWine,
                        modifier = Modifier.padding(10.dp)
                    )
                }
                Spacer(modifier = Modifier.width(12.dp))
                Column(modifier = Modifier.weight(1f)) {
                    Text(record.name, fontWeight = FontWeight.ExtraBold)
                    Text(record.title, color = Slate, style = MaterialTheme.typography.bodySmall)
                }
            }
            Row(horizontalArrangement = Arrangement.spacedBy(10.dp)) {
                SmallStat("${record.activeCases}", "Cases", Modifier.weight(1f))
                SmallStat("${record.assignedTasks}", "Tasks", Modifier.weight(1f))
                SmallStat("${record.hoursLast7Days}h", "7 days", Modifier.weight(1f))
            }
        }
    }
}

@Composable
private fun SmallStat(value: String, label: String, modifier: Modifier = Modifier) {
    Surface(
        modifier = modifier,
        shape = RoundedCornerShape(16.dp),
        color = SurfaceSoft
    ) {
        Column(modifier = Modifier.padding(12.dp)) {
            Text(value, fontWeight = FontWeight.ExtraBold, color = DeepWine)
            Text(label, style = MaterialTheme.typography.labelSmall, color = Slate)
        }
    }
}

@Composable
private fun TaskCard(task: TaskRecord) {
    val color = when {
        task.complete -> FieldTeal
        task.overdue -> AlertRed
        else -> SignalBlue
    }
    ElevatedCard(
        shape = RoundedCornerShape(20.dp),
        colors = CardDefaults.elevatedCardColors(containerColor = Color.White)
    ) {
        Row(
            modifier = Modifier.padding(16.dp),
            verticalAlignment = Alignment.CenterVertically
        ) {
            Icon(
                imageVector = if (task.overdue) Icons.Filled.Warning else Icons.Filled.CheckCircle,
                contentDescription = null,
                tint = color
            )
            Spacer(modifier = Modifier.width(12.dp))
            Column(modifier = Modifier.weight(1f)) {
                Text(task.title, fontWeight = FontWeight.Bold)
                Text("${task.assignedTo} - ${task.due}", color = Slate, style = MaterialTheme.typography.bodySmall)
            }
        }
    }
}

@Composable
private fun EvidenceCard(record: EvidenceRecord) {
    ElevatedCard(
        shape = RoundedCornerShape(22.dp),
        colors = CardDefaults.elevatedCardColors(containerColor = Color.White)
    ) {
        Column(
            modifier = Modifier.padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(12.dp)
        ) {
            Row(verticalAlignment = Alignment.Top) {
                Surface(shape = RoundedCornerShape(16.dp), color = FieldTeal.copy(alpha = 0.12f)) {
                    Icon(
                        Icons.Filled.Description,
                        contentDescription = null,
                        tint = FieldTeal,
                        modifier = Modifier.padding(10.dp)
                    )
                }
                Spacer(modifier = Modifier.width(12.dp))
                Column(modifier = Modifier.weight(1f)) {
                    Text(record.reference, color = FieldTeal, fontWeight = FontWeight.Bold)
                    Text(record.title, fontWeight = FontWeight.ExtraBold)
                    Text("${record.caseNumber} - ${record.type}", color = Slate, style = MaterialTheme.typography.bodySmall)
                }
                if (record.sensitive) {
                    Pill("Sensitive", AlertRed.copy(alpha = 0.12f), AlertRed)
                }
            }
            InfoRow("Added", "${record.added} by ${record.addedBy}")
            InfoRow("Storage", record.location)
            Text(record.hash, color = Slate, style = MaterialTheme.typography.labelSmall)
        }
    }
}

@Composable
private fun EventCard(event: EventRecord) {
    ElevatedCard(
        shape = RoundedCornerShape(22.dp),
        colors = CardDefaults.elevatedCardColors(containerColor = Color.White)
    ) {
        Row(
            modifier = Modifier.padding(16.dp),
            verticalAlignment = Alignment.Top
        ) {
            Surface(shape = RoundedCornerShape(18.dp), color = CaseAmber.copy(alpha = 0.12f)) {
                Icon(
                    Icons.Filled.AccessTime,
                    contentDescription = null,
                    tint = CaseAmber,
                    modifier = Modifier.padding(10.dp)
                )
            }
            Spacer(modifier = Modifier.width(12.dp))
            Column(modifier = Modifier.weight(1f), verticalArrangement = Arrangement.spacedBy(5.dp)) {
                Text(event.time, color = CaseAmber, fontWeight = FontWeight.Bold)
                Text(event.title, fontWeight = FontWeight.ExtraBold)
                Text("${event.location} - ${event.assignedTo}", color = Slate)
                Row(
                    modifier = Modifier.horizontalScroll(rememberScrollState()),
                    horizontalArrangement = Arrangement.spacedBy(8.dp)
                ) {
                    Pill(event.category, SurfaceSoft, DeepWine)
                    Pill(event.caseNumber, SurfaceSoft, Slate)
                }
            }
        }
    }
}

@Composable
private fun InvoiceCard(invoice: InvoiceRecord) {
    val color = if (invoice.status == "Paid") FieldTeal else CaseAmber
    ElevatedCard(
        shape = RoundedCornerShape(22.dp),
        colors = CardDefaults.elevatedCardColors(containerColor = Color.White)
    ) {
        Row(
            modifier = Modifier.padding(16.dp),
            verticalAlignment = Alignment.CenterVertically
        ) {
            Surface(shape = RoundedCornerShape(16.dp), color = color.copy(alpha = 0.12f)) {
                Icon(
                    Icons.Filled.AttachMoney,
                    contentDescription = null,
                    tint = color,
                    modifier = Modifier.padding(10.dp)
                )
            }
            Spacer(modifier = Modifier.width(12.dp))
            Column(modifier = Modifier.weight(1f)) {
                Text(invoice.number, fontWeight = FontWeight.ExtraBold)
                Text("${invoice.caseNumber} - due ${invoice.due}", color = Slate)
            }
            Column(horizontalAlignment = Alignment.End) {
                Text(money(invoice.amount), fontWeight = FontWeight.ExtraBold, color = color)
                Text(invoice.status, color = Slate, style = MaterialTheme.typography.labelMedium)
            }
        }
    }
}

@Composable
private fun InfoRow(label: String, value: String) {
    Row(verticalAlignment = Alignment.Top) {
        Text(
            text = label,
            color = Slate,
            style = MaterialTheme.typography.labelMedium,
            modifier = Modifier.width(92.dp)
        )
        Text(
            text = value,
            fontWeight = FontWeight.SemiBold,
            modifier = Modifier.weight(1f)
        )
    }
}

@Composable
private fun StatusChip(status: String) {
    val color = statusColor(status)
    Pill(status, color.copy(alpha = 0.12f), color)
}

@Composable
private fun Pill(label: String, background: Color, content: Color) {
    Surface(shape = RoundedCornerShape(50), color = background, contentColor = content) {
        Text(
            text = label,
            modifier = Modifier.padding(horizontal = 10.dp, vertical = 6.dp),
            style = MaterialTheme.typography.labelMedium,
            fontWeight = FontWeight.Bold,
            maxLines = 1,
            overflow = TextOverflow.Ellipsis
        )
    }
}

@Composable
private fun ReportProgressRow(label: String, value: Int, total: Int, color: Color) {
    val progress = (value.toFloat() / total.toFloat()).coerceIn(0f, 1f)
    Column(verticalArrangement = Arrangement.spacedBy(8.dp)) {
        Row {
            Text(label, fontWeight = FontWeight.Bold)
            Spacer(modifier = Modifier.weight(1f))
            Text("$value / $total", color = Slate)
        }
        LinearProgressIndicator(
            progress = { progress },
            modifier = Modifier
                .fillMaxWidth()
                .height(9.dp)
                .clip(RoundedCornerShape(50)),
            color = color,
            trackColor = SurfaceSoft
        )
    }
}

@Composable
private fun RevenueBars(points: List<RevenuePoint>) {
    val max = points.maxOf { it.amount }.coerceAtLeast(1)
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .height(170.dp),
        horizontalArrangement = Arrangement.spacedBy(8.dp),
        verticalAlignment = Alignment.Bottom
    ) {
        points.forEach { point ->
            val barHeight = (120f * point.amount / max).coerceAtLeast(12f).dp
            Column(
                modifier = Modifier
                    .weight(1f)
                    .fillMaxHeight(),
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.Bottom
            ) {
                Text(
                    text = "${point.amount / 1000}k",
                    style = MaterialTheme.typography.labelSmall,
                    color = Slate
                )
                Spacer(modifier = Modifier.height(6.dp))
                Box(
                    modifier = Modifier
                        .fillMaxWidth(0.58f)
                        .height(barHeight)
                        .clip(RoundedCornerShape(topStart = 12.dp, topEnd = 12.dp, bottomStart = 5.dp, bottomEnd = 5.dp))
                        .background(Brush.verticalGradient(listOf(FieldTeal, SignalBlue)))
                )
                Spacer(modifier = Modifier.height(8.dp))
                Text(point.month, style = MaterialTheme.typography.labelSmall, color = Slate)
            }
        }
    }
}

private fun metricTone(tone: MetricTone): Color {
    return when (tone) {
        MetricTone.Wine -> DeepWine
        MetricTone.Teal -> FieldTeal
        MetricTone.Blue -> SignalBlue
        MetricTone.Amber -> CaseAmber
        MetricTone.Red -> AlertRed
        MetricTone.Slate -> Slate
    }
}

private fun statusColor(status: String): Color {
    return when (status) {
        "Active" -> DeepWine
        "Surveillance" -> FieldTeal
        "Awaiting Client" -> CaseAmber
        "Closed" -> SignalBlue
        else -> Slate
    }
}

private fun priorityColor(priority: String): Color {
    return when (priority) {
        "Critical" -> AlertRed
        "High" -> CaseAmber
        "Medium" -> SignalBlue
        else -> Slate
    }
}

private fun money(amount: Int): String = "$" + "%,d".format(amount)
