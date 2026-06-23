# IntelliCase Pro

A modern investigative case management platform for private investigators, with a full **ASP.NET Core 8 MVC web app** and a native **Android Studio / Kotlin** mobile app.

## Platforms

- **Web app:** ASP.NET Core 8 MVC, Razor Views, EF Core, SQLite, Docker
- **Android app:** Kotlin, Jetpack Compose, Material 3, Gradle

## What this build includes

- Case intake and case tracking
- Client management
- Evidence management
- Time and expense tracking
- Invoicing overview
- Reporting and analytics dashboard
- Calendar and field activity planning
- Role-based login with demo investigator accounts
- Native Android prototype for mobile case review and field workflows

## Stack

- **Web frontend:** ASP.NET Core MVC, Razor Views, custom CSS
- **Web backend:** ASP.NET Core 8
- **Data access:** Entity Framework Core
- **Database:** SQLite
- **Authentication:** Cookie-based authentication with PBKDF2 password hashing
- **Mobile:** Kotlin, Jetpack Compose, Material 3
- **Tooling:** Docker, Docker Compose, Android Studio, Gradle

## Project structure

```text
.github/
data/
src/
  IntelliCasePro.Web/
    Controllers/
    Data/
    Models/
    Security/
    Services/
    Views/
    wwwroot/
android/
  app/
    src/main/java/com/intellicasepro/mobile/
  gradle/
docs/
Dockerfile
docker-compose.yml
```

## Run the web app locally with .NET

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

This repo includes a Dockerfile and Compose setup so the repo can be cloned and run without manually installing SQLite.

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

## Open the Android app

1. Open Android Studio.
2. Choose **File > Open**.
3. Select the `android` folder in this repo.
4. Let Gradle sync.
5. Run the `app` configuration on an Android emulator or device.

### Build from the terminal

```powershell
cd android
.\gradlew.bat :app:assembleDebug
```

The debug APK is created at:

```text
android/app/build/outputs/apk/debug/app-debug.apk
```

### Open without Android Studio

If you only want to try the Android app, install the built APK on an Android phone or emulator.

This repo does not commit APK build output. Build the APK locally with the Gradle command above, or download an APK from the repo's GitHub Releases if one has been attached for demos.

To install from a terminal with Android platform tools:

```powershell
adb install -r android/app/build/outputs/apk/debug/app-debug.apk
```

The debug APK is automatically signed for testing. It is meant for demo/review installs, not Play Store distribution.

### Persistent data

The SQLite database is stored in a mounted local folder:

```text
./data
```

That means your seeded data and anything you add in the app will persist between container restarts.

## Demo login

Use one of the seeded investigator accounts in the web app:

- **Admin:** `jane@intellicasepro.local` / `Demo#2026!`
- **Investigator:** `marcus@intellicasepro.local` / `Demo#2026!`
- **Analyst:** `priya@intellicasepro.local` / `Demo#2026!`

Authentication uses cookie-based sign-in with PBKDF2 password hashing. The **Settings** area is restricted to the admin role.

The Android prototype accepts:

```text
jane@intellicasepro.local
Demo#2026!
```

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
- Android Studio project kept in `/android` so the web and mobile versions live together as one product

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
- Android app integration with backend APIs and offline sync

## Suggested next upgrades

1. Add full user management and role permissions
2. Add secure document upload storage and evidence download
3. Add invoice generation and PDF export
4. Add mobile-friendly field capture
5. Add audit trail and chain-of-custody workflow
