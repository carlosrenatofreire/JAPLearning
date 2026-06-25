# DATABASE/03-relationships.md — Tabelas de Relacionamentos (R_)

Tabelas de junção e progresso dos formandos. Prefixo `R_`.

---

## R_UserCourseLessons — Progresso do formando por lição

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| UserId | uniqueidentifier | ✓ | FK → E_Users |
| LessonId | uniqueidentifier | ✓ | FK → E_Lessons |
| CompletedDate | datetime2 | | null = não concluída |
| WatchedSeconds | int | | segundos de vídeo assistidos |

> Um registo por utilizador/lição. `CompletedDate` preenchido = lição concluída.

---

## R_UserLessonTests — Tentativas de quiz

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| UserId | uniqueidentifier | ✓ | FK → E_Users |
| LessonId | uniqueidentifier | ✓ | FK → E_Lessons |
| Score | int | | percentagem de acertos (0-100) |
| Passed | bit | ✓ | Score ≥ Course.PassingScore |
| AttemptNumber | int | ✓ | número da tentativa; default 1 |
| CompletedDate | datetime2 | ✓ | data/hora da tentativa |

> Múltiplos registos por utilizador/lição (uma linha por tentativa).
> Para o certificado, usa-se o **melhor Score** de todas as tentativas por lição.

---

## R_UserLessonQuestions — Respostas por tentativa

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| UserLessonTestId | uniqueidentifier | ✓ | FK → R_UserLessonTests |
| QuestionId | uniqueidentifier | ✓ | FK → E_Questions |
| SelectedOptionId | uniqueidentifier | | FK → E_QuestionOptions (nullable) |
| IsRight | bit | | true = resposta correcta |
| AnsweredDate | datetime2 | | |

---

## R_CourseRequirements — Pré-requisitos de formação

| Coluna | Tipo | Notas |
|--------|------|-------|
| Id | uniqueidentifier | PK |
| CourseId | uniqueidentifier | FK → E_Courses |
| RequiredCourseId | uniqueidentifier | FK → E_Courses |

---

## R_RolePermissions — Permissões por role

| Coluna | Tipo | Notas |
|--------|------|-------|
| Id | uniqueidentifier | PK |
| RoleId | uniqueidentifier | FK → P_Roles |
| PermissionId | uniqueidentifier | FK → P_Permissions |
