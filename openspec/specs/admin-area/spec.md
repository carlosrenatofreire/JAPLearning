## Purpose
Define the admin and supervisor management area: dashboard KPIs, student/team/course reports, audit logging, and the version changelog system.

## Requirements

### Requirement: Admin dashboard shows platform KPIs and top rankings
The system SHALL display 5 KPIs and Top 7 lists on the admin home page, scoped to the Supervisor's team when applicable.

#### Scenario: Admin views full dashboard
- **WHEN** an Administrador navigates to Home/Index
- **THEN** they SHALL see 5 KPIs (Alunos, Formações Disponíveis, Em Preparação, Certificados, Sessões) and Top 7 tables for students, teams, and courses across all data

#### Scenario: Supervisor dashboard is scoped to their team
- **WHEN** a Supervisor views the dashboard
- **THEN** all KPIs and Top 7 data SHALL be filtered to their team only

### Requirement: Student ranking lists students ordered by completed lessons
The system SHALL provide a student ranking at /Reports/Students with completed lessons, hours watched, and certificate count per student.

#### Scenario: Admin views full student ranking
- **WHEN** an Administrador accesses /Reports/Students
- **THEN** all active students SHALL be listed regardless of team

#### Scenario: Supervisor views scoped ranking
- **WHEN** a Supervisor accesses /Reports/Students
- **THEN** only students from their team SHALL be shown

#### Scenario: Viewing student detail
- **WHEN** an admin or supervisor clicks the detail button on a student row
- **THEN** they SHALL be taken to /Reports/StudentDetail/{id} showing the student's header card, KPIs, per-course progress, and earned certificates

### Requirement: Team ranking aggregates progress by team
The system SHALL provide a team ranking at /Reports/Teams showing member count, total lessons, average lessons per member, and certificates.

#### Scenario: Supervisor sees only their own team
- **WHEN** a Supervisor accesses /Reports/Teams
- **THEN** only their own team SHALL be displayed

### Requirement: Course ranking shows most-attended courses
The system SHALL provide a course ranking at /Reports/Courses showing student count, completed lessons, total lessons, and certificates per course.

#### Scenario: Admin views course ranking
- **WHEN** an Administrador or Supervisor accesses /Reports/Courses
- **THEN** courses SHALL be listed ordered by student count descending, showing student count, lessons completed, total lessons, and certificates issued

### Requirement: Audit log records significant system events
The system SHALL maintain an audit log visible only to Administrador, capturing logins, failures, errors, and data-modification requests.

#### Scenario: Audit log access restricted to Administrador
- **WHEN** a non-Administrador accesses /AuditLogs
- **THEN** the system SHALL return 403 Forbidden

#### Scenario: Unhandled exception is logged with stack trace
- **WHEN** an unhandled exception occurs
- **THEN** GlobalExceptionMiddleware SHALL log it to AuditLogs with stack trace and HTTP status code

### Requirement: Version changelog tracks platform releases with a public timeline
The system SHALL support versioned releases (E_AppVersions + E_AppVersionItems) with admin CRUD and a public changelog page at /AppVersions/Changelog.

#### Scenario: Version badge shown in topbar
- **WHEN** any authenticated user views the application
- **THEN** the topbar SHALL display the latest published version badge (e.g., v0.19)

#### Scenario: Draft versions hidden from public changelog
- **WHEN** a version has IsPublished = false
- **THEN** it SHALL NOT appear on the /AppVersions/Changelog page

#### Scenario: Changelog has sticky scroll timeline
- **WHEN** a user scrolls the changelog page
- **THEN** the right-column sticky timeline SHALL highlight the currently visible version automatically
