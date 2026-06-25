## Purpose
Define the core learning structure: courses, topics, lessons, questions, and answer options, including certificate eligibility rules.

## Requirements

### Requirement: Courses are the primary learning unit
The system SHALL support courses (Formações) with title, description, thumbnail, level, category, PassingScore, and active status.

#### Scenario: Course has a passing score threshold
- **WHEN** a course is created or edited
- **THEN** PassingScore SHALL default to 60 (representing 60%) and be configurable per course

#### Scenario: Course belongs to a category and team
- **WHEN** a course is created
- **THEN** it MUST be assigned to a Category which belongs to a Team

### Requirement: Courses are organised into Topics
The system SHALL support Topics (Tópicos) that group lessons within a course, ordered by DisplayOrder.

#### Scenario: Topics are ordered
- **WHEN** a course is rendered in the player
- **THEN** topics SHALL appear in ascending DisplayOrder

### Requirement: Lessons are the atomic content unit with two types
The system SHALL support video lessons and quiz lessons, auto-detected by the presence of questions with options.

#### Scenario: Video lesson renders a player
- **WHEN** a student opens a video lesson
- **THEN** the system SHALL render an iframe embed (YouTube/Vimeo) and show a "Concluir Aula" button

#### Scenario: Quiz lesson has no video area
- **WHEN** a student opens a quiz lesson
- **THEN** the system SHALL NOT render a video area and SHALL display an interactive quiz instead

#### Scenario: Lesson type is auto-detected
- **WHEN** a lesson has questions that have at least one option
- **THEN** the system SHALL treat it as a quiz lesson (IsQuizLesson = Questions.Any(q => q.Options.Any()))

### Requirement: Questions belong to quiz lessons
The system SHALL support questions (Questões) linked to a lesson with text, optional explanation, and active status.

#### Scenario: Question is shown in quiz
- **WHEN** a quiz lesson is rendered
- **THEN** all active questions for that lesson SHALL be shown sequentially

### Requirement: Question options indicate correct answers
The system SHALL support answer options (Opções de Resposta) with text, IsCorrect flag, optional explanation, and active status.

#### Scenario: Correct answer evaluated
- **WHEN** a student submits a quiz answer
- **THEN** correctness SHALL be determined by the IsCorrect flag of the selected option

#### Scenario: Admin UI uses cascading selects
- **WHEN** creating or editing a question option in the admin area
- **THEN** the UI SHALL present cascading dropdowns: Team → Course → Topic → Lesson → Question

### Requirement: Certificate eligibility requires full course completion and minimum score
The system SHALL only issue a certificate when all lessons are completed AND the calculated score meets or exceeds PassingScore.

#### Scenario: All lessons complete with sufficient score
- **WHEN** a student completes the last lesson and their average score >= PassingScore
- **THEN** the system SHALL automatically issue a certificate

#### Scenario: Insufficient score prevents certificate
- **WHEN** a student completes all lessons but average score < PassingScore
- **THEN** the system SHALL NOT issue a certificate and SHALL notify the student

#### Scenario: Quiz score uses best attempt per lesson
- **WHEN** calculating certificate eligibility score
- **THEN** the system SHALL use the BEST attempt per quiz lesson, not the average of all attempts

#### Scenario: Video-only lessons count as 100%
- **WHEN** calculating certificate eligibility score
- **THEN** lessons without a quiz SHALL count as 100% towards the average score
