## Purpose
Define the platform vision, user roles, and external integrations for JAPLearning — an internal e-learning platform for the DMC-Developers team.

## Requirements

### Requirement: Platform serves three distinct roles
The system SHALL support three user roles — Administrador, Supervisor, and Formando — each with different access levels.

#### Scenario: Administrador has full access
- **WHEN** a user with role Administrador is authenticated
- **THEN** they SHALL have access to all platform modules including user management, roles, audit, and all content

#### Scenario: Supervisor has scoped management access
- **WHEN** a user with role Supervisor is authenticated
- **THEN** they SHALL have CRUD access to courses/topics/lessons scoped to their team but NOT access to users, roles, permissions, audit, or versions

#### Scenario: Formando has student area access only
- **WHEN** a user with role Formando is authenticated
- **THEN** they SHALL only access the student area (player, dashboard, certificates, profile)

### Requirement: Images are managed via Cloudinary
The system SHALL use Cloudinary as the CDN for all user-uploaded images (user photos, testimonial photos, article covers, teacher photos).

#### Scenario: Image uploaded successfully
- **WHEN** a user submits a form with an image file
- **THEN** the system SHALL upload it to Cloudinary and store the resulting URL in the database

#### Scenario: No image provided
- **WHEN** a form is submitted without an image
- **THEN** the system SHALL store an empty string (not null) for the image URL field

### Requirement: Secrets are managed via Doppler
The system SHALL source all environment secrets and connection strings from Doppler in production.

#### Scenario: Application starts in production
- **WHEN** the application starts in production
- **THEN** connection strings and secrets SHALL come from Doppler, not appsettings.json

### Requirement: Seed data is present on first run
The system SHALL seed initial reference data (roles and default admin) on first database initialisation.

#### Scenario: Roles are seeded
- **WHEN** the database is initialised for the first time
- **THEN** the roles Administrador, Supervisor, and Formando SHALL exist

#### Scenario: Default admin user is seeded
- **WHEN** the database is initialised for the first time
- **THEN** a default Administrador account SHALL be created
