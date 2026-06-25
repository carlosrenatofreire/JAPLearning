## Purpose
Define user authentication, first-login password change enforcement, login tracking, and rate limiting.

## Requirements

### Requirement: Users authenticate via cookie-based login
The system SHALL authenticate users with email and password, issuing a persistent cookie on success.

#### Scenario: Valid credentials
- **WHEN** a user submits correct email and password
- **THEN** the system SHALL sign in via CookieAuthentication and redirect to the role-appropriate dashboard

#### Scenario: Invalid credentials
- **WHEN** a user submits incorrect email or password
- **THEN** the system SHALL show "E-mail ou senha inválidos." and NOT sign in

#### Scenario: Inactive account
- **WHEN** a user with IsActived = false attempts to log in
- **THEN** the system SHALL show "Conta inativa. Contacte o administrador." and NOT sign in

### Requirement: First-time login forces a password change for all roles
The system SHALL redirect any authenticated user with MustChangePassword = true to /Account/ChangePassword before any other page.

#### Scenario: Any role with MustChangePassword logs in
- **WHEN** a user with MustChangePassword = true successfully authenticates
- **THEN** the system SHALL redirect to /Account/ChangePassword regardless of their role

#### Scenario: Password changed successfully
- **WHEN** a user submits a valid new password on the ChangePassword page
- **THEN** MustChangePassword SHALL be set to false and the user redirected to their role dashboard (Formando → Student/Dashboard, others → Home/Index)

#### Scenario: New users are created with MustChangePassword = true
- **WHEN** an administrator creates a new user account
- **THEN** MustChangePassword SHALL default to true

### Requirement: Login activity is tracked for Formando users only
The system SHALL record LoginCount and LastLoginDate for every successful Formando login using a surgical ExecuteUpdateAsync (not a full entity update).

#### Scenario: Formando logs in
- **WHEN** a user with role Formando successfully authenticates
- **THEN** LoginCount SHALL be incremented by 1 and LastLoginDate set to current UTC time

#### Scenario: Non-Formando logs in
- **WHEN** a user with role Administrador or Supervisor logs in
- **THEN** LoginCount and LastLoginDate SHALL NOT be updated

### Requirement: Login attempts are rate-limited per IP and email
The system SHALL block login after 5 failed attempts within 5 minutes, scoped per IP+email pair so one blocked user does not affect others.

#### Scenario: Rate limit exceeded
- **WHEN** the same IP+email pair exceeds 5 attempts within 5 minutes
- **THEN** subsequent requests SHALL be blocked and a rate-limit error message displayed

#### Scenario: Other users on same IP are unaffected
- **WHEN** one user is rate-limited
- **THEN** other users on the same IP SHALL still be able to attempt login

### Requirement: User photos are stored via Cloudinary with non-null URL
The system SHALL upload user photos to Cloudinary and always store a non-null PhotoUrl (empty string when no photo).

#### Scenario: Photo uploaded
- **WHEN** an admin uploads a photo for a user
- **THEN** the system SHALL upload to Cloudinary and store the resulting URL

#### Scenario: No photo provided
- **WHEN** a user is created or edited without a photo
- **THEN** PhotoUrl SHALL be stored as empty string, never null
