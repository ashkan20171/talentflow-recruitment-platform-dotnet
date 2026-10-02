# TalentFlow Recruitment Platform

> A bilingual, role-aware recruitment and applicant-tracking desktop platform built with **C# / .NET Framework 4.8 / WinForms / SQL Server LocalDB**.

TalentFlow demonstrates how a legacy Windows recruitment application can be evolved into a structured, security-conscious recruitment workspace with dedicated experiences for **Candidates, Recruiters, Employers and Administrators**.

## Why this project stands out

- **End-to-end hiring workflow** — jobs, applications, pipeline, interviews, scorecards, offers and approvals.
- **Role-based product experience** — navigation and actions adapt to Candidate, Recruiter, Employer and Admin responsibilities.
- **Talent intelligence** — candidate 360 profiles, skill matching, candidate comparison, talent pool and hiring insights.
- **Operational tooling** — recruitment tasks, notifications, saved searches, job alerts, reports, audit trail, backup and system health.
- **Bilingual UX** — Persian is supported natively with RTL layout; English switches the workspace to LTR.
- **Explainable matching** — local matching logic can run without a cloud dependency and is designed for future AI-provider integration.
- **Incremental database migrations** — new capabilities are added without discarding previous application data.

## Product areas

| Area | Highlights |
| --- | --- |
| Candidate Experience | Candidate dashboard, job discovery, saved jobs, alerts, resume builder, documents, application timeline |
| Recruiter Workspace | Talent CRM, recruitment tasks, Kanban pipeline, candidate comparison, talent pool, interview planning |
| Employer Experience | Hiring overview, vacancies, funnel visibility, offer approval |
| Interview & Offer Flow | Calendar, interview scorecards, offer management, approval workflow |
| Intelligence | Smart matching, hiring insights, analytics, executive overview, advanced search |
| Governance | Permission matrix, audit log, session security, feature flags, system health, backup/restore |
| Communication | Notification center and reusable email templates |

## Stage 17 additions

### Talent Pool
Recruiters can build a reusable talent community instead of treating every candidate as a one-off application. Candidates can be classified as **Warm**, **Nurture**, **Ready** or **Do Not Contact**, with source and recruiter notes retained in SQL Server.

### Hiring Insights
A management view summarizes open vacancies, active candidates, upcoming interviews and open offers, while also exposing the current application funnel and aging applications that may require attention.

## Role model

| Capability | Candidate | Recruiter | Employer | Admin |
| --- | :---: | :---: | :---: | :---: |
| Search & track jobs | ✓ | ✓ | ✓ | ✓ |
| Manage candidates | — | ✓ | Limited | ✓ |
| Recruitment pipeline | Own progress | ✓ | View | ✓ |
| Interview scorecards | — | ✓ | View | ✓ |
| Offers / approvals | Own offers | ✓ | ✓ | ✓ |
| Talent pool | — | ✓ | Limited | ✓ |
| Users & permissions | — | — | — | ✓ |
| Audit / security / backup | — | — | — | ✓ |

> Permissions are enforced through the application permission model; the table is a product-level overview rather than a substitute for configured permissions.

## Architecture

```text
AshkanJobCenter/
├── Core/          # session, security, localization and theme primitives
├── Data/          # SQL Server LocalDB initialization and data access
├── Services/      # authentication and application services
├── UI/            # role-aware WinForms pages and reusable controls
├── Database/      # legacy database assets retained for migration/reference
└── Properties/
```

The modernized application intentionally isolates the current WinForms implementation from the original legacy project under `Legacy-KaryabiWindows/`. This makes the modernization path visible without forcing the new UI to depend on the old commercial-control stack.

## Technology

- C#
- .NET Framework 4.8
- Windows Forms
- SQL Server LocalDB / ADO.NET
- SHA-based salted password storage
- Visual Studio 2022
- RTL/LTR localization

## Security & reliability highlights

- Salted password hashes instead of plaintext credentials
- Parameterized SQL for application operations
- Role/permission-aware navigation and actions
- Audit logging for important mutations
- Session security workspace
- Feature flags for controlled capability rollout
- Backup/restore and system-health tooling
- Incremental schema initialization and performance indexes

## Getting started

### Requirements

- Windows 10/11
- Visual Studio 2022
- .NET Framework 4.8 Developer Pack
- SQL Server Express LocalDB

### Run

1. Open `AshkanJobCenter.sln` in Visual Studio 2022.
2. Select **Debug / Any CPU**.
3. Build the solution.
4. Run the `AshkanJobCenter` project.
5. The LocalDB database and required schema are initialized automatically on first launch.

### Demo administrator

```text
Username: admin
Password: Admin@123
```

**Important:** the demo credential is intended only for local evaluation. Change it before using the project beyond a development/demo environment.

## Screenshots

Add current screenshots before publishing the repository. Recommended set:

1. Premium bilingual login
2. Candidate dashboard
3. Recruiter workspace / Kanban
4. Candidate 360
5. Talent Pool
6. Hiring Insights / Executive overview
7. Permission Matrix

A concise visual tour is more useful to recruiters than dozens of near-identical screenshots.

## Engineering decisions

**Why .NET Framework 4.8?**  
This repository is a modernization of an existing Windows desktop system. Keeping .NET Framework 4.8 demonstrates practical legacy modernization while improving architecture, security, UX and maintainability without pretending the original constraints do not exist.

**Why LocalDB?**  
It gives reviewers a low-friction local setup while keeping the data layer compatible with SQL Server concepts and tooling.

**Why no mandatory cloud AI dependency?**  
Matching remains deterministic and explainable for offline demos. The design can later be extended behind a provider interface for semantic ranking or LLM-assisted workflows.

## Roadmap

- Automated unit/integration test suite
- CI build pipeline and release artifacts
- Repository screenshots and short demo GIF
- Data-access abstraction / repository layer
- Optional REST API and web client
- Optional AI provider for semantic CV/job matching
- Calendar/email integrations
- Accessibility and keyboard-navigation audit

## Portfolio context

This project is intended to demonstrate more than CRUD screens. It showcases **legacy modernization, desktop UX, SQL data modeling, access control, recruitment-domain workflows, localization, operational tooling and iterative product engineering**.

## License

Choose and add a license before publishing publicly. No license is included by default, so all rights remain with the repository owner until one is selected.
