## Purpose
Define lookup and configuration entities: Teams, Categories, Levels, Teachers, Subjects, and Roles.

## Requirements

### Requirement: Teams are the top-level organisational unit
The system SHALL support Teams (Equipas) that group courses and users by department.

#### Scenario: Course is scoped to a team via category
- **WHEN** a course is assigned a category
- **THEN** it is implicitly scoped to the team that owns that category

#### Scenario: User belongs to a team
- **WHEN** a user is created
- **THEN** they MUST be assigned to a team

### Requirement: Categories classify courses within a team
The system SHALL support Categories (Categorias) that belong to a team and are used to classify courses.

#### Scenario: Category requires a team
- **WHEN** a category is created
- **THEN** it MUST have a TeamId assigned

### Requirement: Levels define course difficulty
The system SHALL support Levels (Níveis) seeded with Iniciante, Intermédio, and Avançado.

#### Scenario: Level assigned to course
- **WHEN** a course is created
- **THEN** a Level SHALL be selected from the seeded values

### Requirement: Teachers are the responsible authors of courses
The system SHALL support Teachers (Professores/Formadores) with name, description, optional Cloudinary photo, and active status.

#### Scenario: Teacher assigned to course
- **WHEN** a course is created
- **THEN** a Teacher SHALL be assigned as the responsible author

### Requirement: Subjects classify articles
The system SHALL support Subjects (Assuntos) used to categorise articles, with name and active status.

#### Scenario: Article assigned a subject
- **WHEN** an article is created
- **THEN** a Subject SHALL be selected from the available list

### Requirement: Roles define system access levels and are managed only by Administrador
The system SHALL enforce exactly three roles — Administrador, Supervisor, Formando — with RolesController accessible only to Administrador.

#### Scenario: Non-admin attempts to manage roles
- **WHEN** a user without the Administrador role accesses RolesController
- **THEN** the system SHALL return 403 Forbidden
