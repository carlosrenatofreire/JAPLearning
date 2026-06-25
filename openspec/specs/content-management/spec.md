## Purpose
Define editorial content management: testimonials shown on the landing page and articles for the platform blog.

## Requirements

### Requirement: Testimonials are displayed on the public landing page
The system SHALL support testimonials with author info, quote, rating (1–5), optional Cloudinary photo, display order, and active status.

#### Scenario: Testimonial created with photo
- **WHEN** an admin creates a testimonial with a photo
- **THEN** the photo SHALL be uploaded to Cloudinary and the URL stored

#### Scenario: Testimonial created without photo
- **WHEN** an admin creates a testimonial without a photo
- **THEN** PhotoUrl SHALL be stored as empty string, never null

#### Scenario: Editing keeps existing photo when no new photo is uploaded
- **WHEN** an admin edits a testimonial without uploading a new photo
- **THEN** the existing PhotoUrl SHALL be preserved using the pattern: PhotoUrl = vm.PhotoUrl ?? string.Empty

#### Scenario: Editing replaces photo when a new one is uploaded
- **WHEN** an admin edits a testimonial and uploads a new photo
- **THEN** the old Cloudinary image SHALL be deleted and the new URL stored

### Requirement: Testimonial rating is between 1 and 5
The system SHALL accept a numeric rating between 1 and 5 for each testimonial.

#### Scenario: Valid rating submitted
- **WHEN** a rating between 1 and 5 is submitted
- **THEN** it SHALL be stored as-is

### Requirement: Articles provide editorial content with a URL slug
The system SHALL support articles with title, auto-generated slug, content, Cloudinary cover image, subject, reading time, and active status.

#### Scenario: Article slug is generated from title
- **WHEN** an article is created
- **THEN** a URL-friendly slug SHALL be generated from the title

#### Scenario: Article cover image uploaded
- **WHEN** an article is created with a cover image
- **THEN** the image SHALL be uploaded to Cloudinary and the URL stored

#### Scenario: Reading time is entered manually
- **WHEN** an admin creates or edits an article
- **THEN** the ReadingTime field (estimated minutes) SHALL be set manually by the admin
