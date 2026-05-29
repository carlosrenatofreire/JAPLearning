# DBML.md — JAPLearning

Schema completo em formato DBML (Database Markup Language).
Gerado a partir dos ficheiros de entidades e mappings EF Core.
Compatível com [dbdiagram.io](https://dbdiagram.io) para visualização gráfica.

> ⚠️ **Atenção:** A coluna `ScorePercent` em `E_Certificates` foi adicionada
> manualmente via SQL (sem migração EF). Ver secção "Alterações Manuais".

---

## Script DBML

```dbml
// ============================================================
// JAPLearning — Database Schema
// Engine: SQL Server
// Gerado de: EF Core Fluent API Mappings
// ============================================================

// ── PARÂMETROS (P_) ─────────────────────────────────────────

Table P_Teams {
  Id           uniqueidentifier [pk, not null]
  Name         varchar(100)     [not null]
  Description  varchar(1000)    [null]
  Thumbnail    nvarchar(max)    [null]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]
}

Table P_Categories {
  Id           uniqueidentifier [pk, not null]
  TeamId       uniqueidentifier [not null, ref: > P_Teams.Id]
  Name         varchar(100)     [not null]
  Subtitle     varchar(150)     [null]
  Description  varchar(255)     [null]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]
}

Table P_Levels {
  Id           uniqueidentifier [pk, not null]
  Name         varchar(100)     [not null]
  Description  varchar(255)     [null]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]
}

Table P_Teachers {
  Id           uniqueidentifier [pk, not null]
  Name         varchar(100)     [not null]
  Description  varchar(500)     [null]
  PhotoUrl     varchar(500)     [null]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]
}

Table P_Roles {
  Id           uniqueidentifier [pk, not null]
  Name         varchar(100)     [not null]
  Description  varchar(255)     [null]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]
}

Table P_Modules {
  Id           uniqueidentifier [pk, not null]
  Name         varchar(100)     [not null]
  Description  varchar(255)     [null]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]
}

Table P_Permissions {
  Id           uniqueidentifier [pk, not null]
  ModuleId     uniqueidentifier [not null, ref: > P_Modules.Id]
  Name         varchar(100)     [not null]
  Description  varchar(255)     [null]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]
}

Table P_Subjects {
  Id           uniqueidentifier [pk, not null]
  Name         varchar(100)     [not null]
  Description  varchar(255)     [null]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]
}

// ── ENTIDADES (E_) ───────────────────────────────────────────

Table E_Users {
  Id                uniqueidentifier [pk, not null]
  RoleId            uniqueidentifier [not null, ref: > P_Roles.Id]
  TeamId            uniqueidentifier [not null, ref: > P_Teams.Id]
  FirstName         varchar(100)     [not null]
  LastName          varchar(100)     [not null]
  Email             varchar(150)     [not null, unique]
  Password          varchar(500)     [not null]
  PhoneNumber       varchar(20)      [null]
  PhotoUrl          varchar(500)     [null]
  Address           varchar(255)     [null]
  Number            varchar(20)      [null]
  City              varchar(100)     [null]
  State             varchar(100)     [null]
  Country           varchar(100)     [null]
  ResetToken        varchar(500)     [null]
  ResetTokenExpiry  datetime2        [null]
  CreatedDate       datetime2        [not null]
  ChangedDate       datetime2        [null]
  IsActived         bit              [not null, default: 1]
  IsDeleted         bit              [not null, default: 0]

  indexes {
    Email [unique, name: 'IX_E_Users_Email']
  }
}

Table E_Courses {
  Id           uniqueidentifier [pk, not null]
  CategoryId   uniqueidentifier [not null, ref: > P_Categories.Id]
  TeacherId    uniqueidentifier [not null, ref: > P_Teachers.Id]
  LevelId      uniqueidentifier [not null, ref: > P_Levels.Id]
  Title        varchar(150)     [not null]
  Subtitle     varchar(255)     [null]
  Description  varchar(2000)    [null]
  Thumbnail    varchar(500)     [null]
  PassingScore int              [not null, default: 60]
  IsBrief      bit              [not null, default: 0]
  IsFree       bit              [not null, default: 0]
  CreatedDate  datetime2        [not null]
  ChangedDate  datetime2        [null]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]
}

Table E_Topics {
  Id           uniqueidentifier [pk, not null]
  CourseId     uniqueidentifier [not null, ref: > E_Courses.Id]
  Name         varchar(100)     [not null]
  Description  varchar(500)     [null]
  Order        int              [not null]
  CreatedDate  datetime2        [not null]
  ChangedDate  datetime2        [null]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]
}

Table E_Lessons {
  Id            uniqueidentifier [pk, not null]
  CourseId      uniqueidentifier [not null, ref: > E_Courses.Id]
  TopicId       uniqueidentifier [not null, ref: > E_Topics.Id]
  Order         int              [not null]
  Name          varchar(150)     [not null]
  Description   varchar(500)     [null]
  TimeLesson    time             [null]
  Video         varchar(500)     [null]
  IsTest        bit              [not null, default: 0]
  IsFreePreview bit              [not null, default: 0]
  CreatedDate   datetime2        [not null]
  ChangedDate   datetime2        [null]
  IsActived     bit              [not null, default: 1]
  IsDeleted     bit              [not null, default: 0]
}

Table E_Questions {
  Id           uniqueidentifier [pk, not null]
  LessonId     uniqueidentifier [not null, ref: > E_Lessons.Id]
  Name         varchar(500)     [not null]
  Description  varchar(1000)    [null]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]
}

Table E_QuestionOptions {
  Id           uniqueidentifier [pk, not null]
  QuestionId   uniqueidentifier [not null, ref: > E_Questions.Id]
  Name         varchar(500)     [not null]
  Description  varchar(1000)    [null]
  IsCorrect    bit              [not null, default: 0]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]
}

Table E_Certificates {
  Id             uniqueidentifier [pk, not null]
  UserId         uniqueidentifier [not null, ref: > E_Users.Id]
  CourseId       uniqueidentifier [not null, ref: > E_Courses.Id]
  CertifiedFile  varchar(500)     [not null]
  ValidationCode varchar(100)     [null, unique]
  ScorePercent   int              [not null, default: 0, note: '⚠️ Adicionado manualmente via SQL — sem migração EF']
  CompletedDate  datetime2        [not null]

  indexes {
    ValidationCode [unique, name: 'IX_E_Certificates_ValidationCode']
  }
}

Table E_Testimonials {
  Id           uniqueidentifier [pk, not null]
  UserId       uniqueidentifier [null, ref: > E_Users.Id]
  AuthorName   varchar(100)     [not null]
  Role         varchar(100)     [not null]
  City         varchar(100)     [not null]
  Country      varchar(100)     [not null]
  PhotoUrl     varchar(500)     [not null, note: 'Pode ser string vazia se sem foto']
  LinkedinUrl  varchar(500)     [null]
  Quote        varchar(1000)    [not null]
  Rating       int              [not null, default: 5]
  DisplayOrder int              [not null, default: 0]
  Featured     bit              [not null, default: 0]
  CreatedDate  datetime2        [not null]
  ChangedDate  datetime2        [null]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]
}

Table E_Articles {
  Id           uniqueidentifier [pk, not null]
  SubjectId    uniqueidentifier [not null, ref: > P_Subjects.Id]
  Name         varchar(150)     [not null]
  Description  varchar(500)     [null]
  Content      varchar(max)     [null]
  Slug         varchar(200)     [null, unique]
  CoverImage   varchar(500)     [null]
  Author       varchar(100)     [null]
  PublishDate  datetime2        [not null]
  ReadingTime  int              [null]
  CreatedDate  datetime2        [not null]
  ChangedDate  datetime2        [null]
  IsActived    bit              [not null, default: 1]
  IsDeleted    bit              [not null, default: 0]

  indexes {
    Slug [unique, name: 'IX_E_Articles_Slug']
  }
}

// ── RELACIONAMENTOS (R_) ─────────────────────────────────────

Table R_UserCourseLessons {
  Id             uniqueidentifier [pk, not null]
  UserId         uniqueidentifier [not null, ref: > E_Users.Id]
  LessonId       uniqueidentifier [not null, ref: > E_Lessons.Id]
  CompletedDate  datetime2        [null, note: 'NULL = lição não concluída']
  WatchedSeconds int              [null]
}

Table R_UserLessonTests {
  Id            uniqueidentifier [pk, not null]
  UserId        uniqueidentifier [not null, ref: > E_Users.Id]
  LessonId      uniqueidentifier [not null, ref: > E_Lessons.Id]
  Score         int              [null, note: 'Percentagem de acertos 0-100']
  Passed        bit              [not null, default: 0]
  AttemptNumber int              [not null, default: 1]
  CompletedDate datetime2        [not null]
}

Table R_UserLessonQuestions {
  Id                uniqueidentifier [pk, not null]
  UserLessonTestId  uniqueidentifier [not null, ref: > R_UserLessonTests.Id]
  QuestionId        uniqueidentifier [not null, ref: > E_Questions.Id]
  SelectedOptionId  uniqueidentifier [null, ref: > E_QuestionOptions.Id]
  IsRight           bit              [null]
  AnsweredDate      datetime2        [null]
}

Table R_CourseRequirements {
  Id                   uniqueidentifier [pk, not null]
  CourseId             uniqueidentifier [not null, ref: > E_Courses.Id]
  PrerequisiteCourseId uniqueidentifier [not null, ref: > E_Courses.Id]

  note: 'delete: restrict em ambas as FK para evitar ciclos'
}

Table R_RolePermissions {
  Id           uniqueidentifier [pk, not null]
  RoleId       uniqueidentifier [not null, ref: > P_Roles.Id]
  PermissionId uniqueidentifier [not null, ref: > P_Permissions.Id]
}

// ── AUXILIARES (A_) ──────────────────────────────────────────

Table A_AuditLogs {
  Id          uniqueidentifier [pk, not null]
  LogLevel    int              [not null, note: 'Enum: LogType (0=Info, 1=Warning, 2=Error)']
  CreatedBy   varchar(150)     [not null]
  CreatedDate datetime2        [not null]
  Message     varchar(1000)    [null]
  StackTrace  varchar(max)     [null]
  Json        varchar(max)     [null]
}
```

---

## Índices

| Tabela | Coluna | Tipo |
|--------|--------|------|
| `E_Users` | `Email` | UNIQUE |
| `E_Certificates` | `ValidationCode` | UNIQUE |
| `E_Articles` | `Slug` | UNIQUE |

---

## Alterações Manuais (fora de migrações EF)

> Executar em SSMS em qualquer ambiente novo (dev, staging, prod):

```sql
-- Sessão 3 — ScorePercent em E_Certificates
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('E_Certificates') AND name = 'ScorePercent'
)
    ALTER TABLE E_Certificates ADD ScorePercent int NOT NULL DEFAULT 0;
```

---

## Diagrama de Relações

```
P_Teams ──────────────────────────────────────────────────────────┐
  │                                                               │
  └─< P_Categories >──────────────────────────< E_Courses        │
                                                    │             │
                                      P_Levels >───┤             │
                                      P_Teachers >─┤             │
                                                    │             │
                                                E_Topics          │
                                                    │             │
                                                E_Lessons         │
                                                │   │             │
                                          E_Questions             │
                                                │                 │
                                      E_QuestionOptions           │
                                                                  │
P_Roles >──── E_Users >────────────────────────────────────────< ┘
  │               │
  │               ├─< R_UserCourseLessons
  │               ├─< R_UserLessonTests >──< R_UserLessonQuestions
  │               └─< E_Certificates
  │
  └─< R_RolePermissions >──< P_Permissions >──< P_Modules

P_Subjects >──< E_Articles

E_Courses >──< R_CourseRequirements (auto-referência)
```

---

## Notas de Implementação

| Assunto | Detalhe |
|---------|---------|
| **Soft Delete** | `IsDeleted bit` presente em todas as entidades principais. As queries actuais **não filtram** por `IsDeleted` — `GetAll()` devolve todos os registos. Filtro global a implementar futuramente. |
| **Detecção de quiz** | O campo `IsTest` em `E_Lessons` existe mas não é usado na lógica. A detecção é feita em código: `Questions.Any(q => q.Options.Any())`. |
| **PassingScore** | Campo `int` em `E_Courses` com default 60. Define o limiar mínimo (%) para emissão de certificado. |
| **ValidationCode** | 12 caracteres alfanuméricos uppercase gerados com `Guid.NewGuid().ToString("N").ToUpper()[..12]`. Índice UNIQUE na BD. |
| **CertifiedFile** | Campo `NOT NULL varchar(500)` mas habitualmente vazio (`""`). O certificado é gerado on-demand pela view `CertificateView.cshtml`. |
| **PhotoUrl nulos** | `E_Testimonials.PhotoUrl` é `NOT NULL` mas aceita string vazia. Sempre atribuir `string.Empty` no controller quando não há upload. |
| **OnDelete Restrict** | Aplicado em `R_CourseRequirements` para evitar cascata circular entre `E_Courses`. |
