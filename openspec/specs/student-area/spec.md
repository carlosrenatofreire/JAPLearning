## Purpose
Define the student-facing area: dashboard, lesson player, quiz interaction flow, and certificate viewing.

## Requirements

### Requirement: Student dashboard shows in-progress and completed courses
The system SHALL display in-progress and completed course lists at /Student/Dashboard with progress bars and percentages.

#### Scenario: Student has courses in progress
- **WHEN** a student navigates to /Student/Dashboard
- **THEN** they SHALL see a card for each in-progress course with a progress bar, completed/total lesson count, and percentage

#### Scenario: Student has no courses started
- **WHEN** a student has no progress records
- **THEN** the dashboard SHALL show an empty state with a prompt to start learning

### Requirement: Player renders the correct lesson type
The system SHALL render a video player for video lessons and an interactive quiz for quiz lessons at /Student/Player/{lessonId}.

#### Scenario: Video lesson in player
- **WHEN** a student opens a video lesson
- **THEN** the player SHALL show a video iframe and a "Concluir Aula" button

#### Scenario: Quiz lesson not yet attempted
- **WHEN** a student opens a quiz lesson with no previous attempts
- **THEN** the player SHALL show a "Teste a Fazer" badge and the quiz interface

#### Scenario: Quiz lesson previously attempted
- **WHEN** a student opens a quiz lesson with previous attempts
- **THEN** the player SHALL show the best score badge (e.g., "Tentativa: 80%")

#### Scenario: Quiz lesson with no questions
- **WHEN** a quiz lesson has no questions configured
- **THEN** the player SHALL show "Teste não disponível" with no interactive quiz

### Requirement: Quiz follows a sequential one-question-at-a-time flow
The system SHALL guide students through quiz questions sequentially with selection, navigation, and a final result screen.

#### Scenario: Student selects an option
- **WHEN** a student clicks an answer option
- **THEN** the option SHALL be visually marked and the "Próxima" or "Terminar" button SHALL become active

#### Scenario: Student finishes the quiz
- **WHEN** a student clicks "Terminar"
- **THEN** the system SHALL calculate the percentage of correct answers and POST the result via AJAX to /Student/SaveQuizResult (JSON, no antiforgery token)

#### Scenario: Quiz passed
- **WHEN** the student's score >= course PassingScore
- **THEN** the result screen SHALL show a success message and a "Concluir" button

#### Scenario: Quiz failed
- **WHEN** the student's score < course PassingScore
- **THEN** the result screen SHALL show an encouragement message and a "Tentar Novamente" button

#### Scenario: Student retries the quiz
- **WHEN** a student clicks "Tentar Novamente"
- **THEN** the quiz SHALL reset to question 1 with all selections cleared

### Requirement: Certificate is issued automatically on course completion
The system SHALL trigger certificate evaluation in CompleteLesson and issue at most one certificate per student per course.

#### Scenario: All lessons complete and score sufficient
- **WHEN** a student completes the last lesson and average score >= PassingScore
- **THEN** IssueCertificateAsync SHALL be called with a unique 12-character ValidationCode

#### Scenario: Certificate already exists
- **WHEN** CompleteLesson is triggered but a certificate already exists for this user/course
- **THEN** the system SHALL NOT issue a duplicate certificate

### Requirement: Student can view and print their certificates
The system SHALL provide a list at /Student/MyCertificates and a printable standalone view at /Student/CertificateView/{id}.

#### Scenario: Student views certificate list
- **WHEN** a student navigates to MyCertificates
- **THEN** they SHALL see all earned certificates with course name, score, and a "Ver Certificado" button

#### Scenario: Printable certificate opens without layout
- **WHEN** a student opens CertificateView/{id}
- **THEN** the page SHALL render with Layout=null and include window.print() support
