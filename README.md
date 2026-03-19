# IntelliCase Pro

A modern **C# ASP.NET Core 8 MVC** case management platform for private investigators.

## What this build includes

- Case intake and case tracking
- Client management
- Evidence management
- Time and expense tracking
- Invoicing overview
- Reporting and analytics dashboard
- Calendar and field activity planning
- Role-based login with demo investigator accounts

## Stack

- **Frontend:** ASP.NET Core MVC, Razor Views, custom CSS
- **Backend:** ASP.NET Core 8
- **Data access:** Entity Framework Core
- **Database:** SQLite
- **Authentication:** Cookie-based authentication with PBKDF2 password hashing
- **Container support:** Docker + Docker Compose

## Project structure

```text
src/
  IntelliCasePro.Web/
    Controllers/
    Data/
    Models/
    Security/
    Services/
    Views/
    wwwroot/
docs/
Dockerfile
docker-compose.yml
```

## Run locally with .NET

### Prerequisites

- .NET 8 SDK
- VS Code or Visual Studio

### Start the app

```bash
dotnet restore ./src/IntelliCasePro.Web/IntelliCasePro.Web.csproj
dotnet run --project ./src/IntelliCasePro.Web/IntelliCasePro.Web.csproj
```

Then open the local URL shown in the terminal, usually something like:

```text
http://localhost:5000
```

## Run with Docker

This repo includes a Dockerfile and Compose setup so someone can clone the repo and run it without manually installing SQLite or messing with local paths like a goblin.

### Prerequisites

- Docker Desktop

### Start the app

```bash
docker compose up --build
```

Then open:

```text
http://localhost:8080
```

### Persistent data

The SQLite database is stored in a mounted local folder:

```text
./data
```

That means your seeded data and anything you add in the app will persist between container restarts.

## Demo login

Use one of the seeded investigator accounts:

- **Admin:** `jane@intellicasepro.local` / `Demo#2026!`
- **Investigator:** `marcus@intellicasepro.local` / `Demo#2026!`
- **Analyst:** `priya@intellicasepro.local` / `Demo#2026!`

Authentication uses cookie-based sign-in with PBKDF2 password hashing. The **Settings** area is restricted to the admin role.

## Demo data

On first launch, the app seeds sample investigators, clients, cases, evidence, expenses, time entries, invoices, and calendar items.

## Main pages

- **Dashboard:** KPIs, active cases, recent evidence, invoices, and upcoming events
- **Cases:** Searchable case list, case details, tasks, notes, evidence, time, and expenses
- **Clients:** Client list and quick-create tools
- **Expenses:** Time and miscellaneous expense breakdown with totals
- **Evidence:** Evidence register with quick intake form
- **Invoices:** Billing overview
- **Calendar:** Upcoming surveillance, interviews, and deadlines
- **Reports:** Case distribution, revenue, and closure rate
- **Settings:** Admin-only demo preferences screen

## GitHub-ready extras

- Dockerfile for containerized local runs
- Docker Compose for one-command startup
- GitHub Actions workflow for automatic build checks on push and pull request
- Cleaned repository layout with build output and local database files excluded from source control

## Phase 2 ideas

- Secure document upload storage (Azure Blob, S3, or local file provider)
- Chain-of-custody audit log UI
- Offline field sync for remote work
- External integrations:
  - forensic tools
  - legal research databases
  - mapping and geospatial services
  - email and communications
- Export to polished PDF investigative reports
- Multi-tenant agency support

## Suggested next upgrades

1. Add full user management and role permissions
2. Add secure document upload storage and evidence download
3. Add invoice generation and PDF export
4. Add mobile-friendly field capture
5. Add audit trail and chain-of-custody workflow
