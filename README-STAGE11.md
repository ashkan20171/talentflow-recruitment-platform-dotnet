# Stage 11 – Management, Security & UX Overhaul

Built on Stage 10 (.NET Framework 4.8).

## Added
- Permission-aware Quick Action Center for common recruiter/admin workflows.
- My Profile / secure session surface with audit-backed sign-out.
- Navigation integration for both modules in Persian RTL and English LTR.
- Stage 10 enterprise modules retained: Candidate 360, Kanban, scorecards, reports, feature flags, security/session center, backups and permissions.

## Reliability checks
- No Microsoft.VisualBasic dependency.
- New source files are explicitly included in the classic .csproj.
- Source brace balance and project include paths validated before packaging.
