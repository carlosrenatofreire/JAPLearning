# DATABASE.md — JAPLearning

Schema completo da base de dados SQL Server.
Todas as tabelas usam `uniqueidentifier` (GUID) como chave primária (`Id`).

---

## Convenção de Prefixos

| Prefixo | Tipo | Descrição |
|---------|------|-----------|
| `E_` | Entidades | Tabelas principais de domínio |
| `P_` | Parâmetros | Tabelas de configuração/lookup |
| `R_` | Relacionamentos | Tabelas de junção e progresso |
| `A_` | Auxiliares | Logs e tabelas de suporte |

---

## Alterações Manuais (fora de migrações EF)

> ⚠️ Estas colunas foram adicionadas directamente via SQL sem migração EF.
> Devem ser aplicadas manualmente em qualquer ambiente novo (dev, staging, prod).

```sql
-- Sessão 3 — 2026-05-xx
-- Adiciona nota de aprovação ao certificado
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('E_Certificates') AND name = 'ScorePercent'
)
    ALTER TABLE E_Certificates ADD ScorePercent int NOT NULL DEFAULT 0;
```

---

## Diagrama de Relações (simplificado)

```
P_Teams ──────────────────────────────────────────┐
    │                                             │
    └─< P_Categories >─────────────< E_Courses    │
                                        │         │
                          P_Levels >───┤         │
                          P_Teachers >─┤         │
                                        │         │
                                    E_Topics      │
                                        │         │
                                    E_Lessons     │
                                    │   │         │
                              E_Questions         │
                                    │             │
                          E_QuestionOptions       │
                                                  │
E_Users >──────────────────────────────────────< ┘
    │
    ├─< R_UserCourseLessons (progresso por lição)
    ├─< R_UserLessonTests   (tentativas de quiz)
    │       └─< R_UserLessonQuestions (respostas por tentativa)
    └─< E_Certificates
```

---

## Tabelas de Parâmetros (P_)

### P_Teams
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| Name | nvarchar | ✓ | |
| Description | nvarchar | | |
| Thumbnail | nvarchar | | URL Cloudinary |
| IsActived | bit | ✓ | default true |
| IsDeleted | bit | ✓ | soft delete |

### P_Categories
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| TeamId | uniqueidentifier | ✓ | FK → P_Teams |
| Name | nvarchar | ✓ | |
| Subtitle | nvarchar | | |
| Description | nvarchar | | |
| IsActived | bit | ✓ | default true |
| IsDeleted | bit | ✓ | soft delete |

### P_Levels
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| Name | nvarchar | ✓ | ex: Iniciante, Intermédio, Avançado |
| Description | nvarchar | | |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

### P_Teachers
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| Name | nvarchar | ✓ | |
| Description | nvarchar | | |
| PhotoUrl | nvarchar | | URL Cloudinary |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

### P_Roles
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| Name | nvarchar | ✓ | Administrador / Supervisor / Formando |
| Description | nvarchar | | |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

### P_Subjects
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| Name | nvarchar | ✓ | Assunto para artigos |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

---

## Tabelas de Entidades (E_)

### E_Users
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| RoleId | uniqueidentifier | ✓ | FK → P_Roles |
| TeamId | uniqueidentifier | ✓ | FK → P_Teams |
| FirstName | nvarchar | ✓ | |
| LastName | nvarchar | ✓ | |
| Email | nvarchar | ✓ | |
| Password | nvarchar | ✓ | hash BCrypt |
| PhoneNumber | nvarchar | | |
| PhotoUrl | nvarchar | | URL Cloudinary |
| Address | nvarchar | | |
| Number | nvarchar | | número de porta |
| City | nvarchar | | |
| State | nvarchar | | |
| Country | nvarchar | | |
| ResetToken | nvarchar | | token de recuperação de senha |
| ResetTokenExpiry | datetime2 | | |
| CreatedDate | datetime2 | ✓ | |
| ChangedDate | datetime2 | | |
| IsActived | bit | ✓ | default true |
| IsDeleted | bit | ✓ | soft delete |

### E_Courses
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| CategoryId | uniqueidentifier | ✓ | FK → P_Categories |
| TeacherId | uniqueidentifier | ✓ | FK → P_Teachers |
| LevelId | uniqueidentifier | ✓ | FK → P_Levels |
| Title | varchar(150) | ✓ | |
| Subtitle | varchar(255) | | |
| Description | varchar(2000) | | |
| Thumbnail | varchar(500) | | URL Cloudinary |
| PassingScore | int | ✓ | default 60 — limiar de aprovação (%) |
| IsBrief | bit | ✓ | formação breve/resumida |
| IsFree | bit | ✓ | acesso livre |
| CreatedDate | datetime2 | ✓ | |
| ChangedDate | datetime2 | | |
| IsActived | bit | ✓ | default true |
| IsDeleted | bit | ✓ | soft delete |

### E_Topics (Tópicos)
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| CourseId | uniqueidentifier | ✓ | FK → E_Courses |
| Name | nvarchar | ✓ | |
| Description | nvarchar | | |
| Order | int | ✓ | ordenação dentro da formação |
| CreatedDate | datetime2 | ✓ | |
| ChangedDate | datetime2 | | |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

### E_Lessons
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| CourseId | uniqueidentifier | ✓ | FK → E_Courses |
| TopicId | uniqueidentifier | ✓ | FK → E_Topics |
| Order | int | ✓ | ordenação dentro do tópico |
| Name | nvarchar | ✓ | |
| Description | nvarchar | | |
| TimeLesson | time | | duração estimada |
| Video | nvarchar | | URL embed (YouTube/Vimeo) |
| IsTest | bit | ✓ | true = lição do tipo quiz |
| IsFreePreview | bit | ✓ | preview sem inscrição |
| CreatedDate | datetime2 | ✓ | |
| ChangedDate | datetime2 | | |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

> **Nota:** a detecção de "lição quiz" no código não usa `IsTest` mas sim
> `Questions.Any(q => q.Options.Any())` — ver `PlayerViewModel.IsQuizLesson`.

### E_Questions
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| LessonId | uniqueidentifier | ✓ | FK → E_Lessons |
| Name | nvarchar | ✓ | texto da pergunta |
| Description | nvarchar | | explicação pós-resposta |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

### E_QuestionOptions
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| QuestionId | uniqueidentifier | ✓ | FK → E_Questions |
| Name | nvarchar | ✓ | texto da opção de resposta |
| Description | nvarchar | | explicação se esta opção for escolhida |
| IsCorrect | bit | ✓ | true = resposta correcta |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

### E_Certificates
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| UserId | uniqueidentifier | ✓ | FK → E_Users |
| CourseId | uniqueidentifier | ✓ | FK → E_Courses |
| CertifiedFile | varchar(500) | ✓ | URL do ficheiro (vazio = gerado on-demand) |
| ValidationCode | varchar(100) | | 12 chars alfanumérico único; index UNIQUE |
| ScorePercent | int | ✓ | ⚠️ coluna adicionada manualmente — default 0 |
| CompletedDate | datetime2 | ✓ | data de emissão |

### E_Testimonials
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| UserId | uniqueidentifier | | FK → E_Users (nullable — pode ser externo) |
| AuthorName | nvarchar | ✓ | |
| Role | nvarchar | ✓ | cargo/função |
| City | nvarchar | ✓ | |
| Country | nvarchar | ✓ | |
| PhotoUrl | nvarchar | ✓ | URL Cloudinary (pode ser `""` se sem foto) |
| LinkedinUrl | nvarchar | | |
| Quote | nvarchar | ✓ | texto do depoimento |
| Rating | int | ✓ | 1 a 5; default 5 |
| DisplayOrder | int | ✓ | ordenação na landing page; default 0 |
| Featured | bit | ✓ | destaque na landing page |
| CreatedDate | datetime2 | ✓ | |
| ChangedDate | datetime2 | | |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

### E_Articles
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| SubjectId | uniqueidentifier | ✓ | FK → P_Subjects |
| Name | nvarchar | ✓ | título |
| Description | nvarchar | | resumo |
| Content | nvarchar(max) | | corpo do artigo |
| Slug | nvarchar | | URL amigável |
| CoverImage | nvarchar | | URL Cloudinary |
| Author | nvarchar | | nome do autor |
| PublishDate | datetime2 | ✓ | |
| ReadingTime | int | | minutos estimados de leitura |
| CreatedDate | datetime2 | ✓ | |
| ChangedDate | datetime2 | | |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

---

## Tabelas de Relacionamentos (R_)

### R_UserCourseLessons — Progresso do formando por lição
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| UserId | uniqueidentifier | ✓ | FK → E_Users |
| LessonId | uniqueidentifier | ✓ | FK → E_Lessons |
| CompletedDate | datetime2 | | null = não concluída |
| WatchedSeconds | int | | segundos de vídeo assistidos |

> Um registo por utilizador/lição. `CompletedDate` preenchido = lição concluída.

### R_UserLessonTests — Tentativas de quiz
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

### R_UserLessonQuestions — Respostas por tentativa
| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| UserLessonTestId | uniqueidentifier | ✓ | FK → R_UserLessonTests |
| QuestionId | uniqueidentifier | ✓ | FK → E_Questions |
| SelectedOptionId | uniqueidentifier | | FK → E_QuestionOptions (nullable) |
| IsRight | bit | | true = resposta correcta |
| AnsweredDate | datetime2 | | |

### R_CourseRequirements — Pré-requisitos de formação
| Coluna | Tipo | Notas |
|--------|------|-------|
| Id | uniqueidentifier | PK |
| CourseId | uniqueidentifier | FK → E_Courses |
| RequiredCourseId | uniqueidentifier | FK → E_Courses |

### R_RolePermissions — Permissões por role
| Coluna | Tipo | Notas |
|--------|------|-------|
| Id | uniqueidentifier | PK |
| RoleId | uniqueidentifier | FK → P_Roles |
| PermissionId | uniqueidentifier | FK → P_Permissions |

---

## Tabelas Auxiliares (A_)

### A_AuditLogs
| Coluna | Tipo | Notas |
|--------|------|-------|
| Id | uniqueidentifier | PK |
| UserId | uniqueidentifier | quem fez a acção |
| Action | nvarchar | ex: Create, Update, Delete |
| Entity | nvarchar | nome da entidade afectada |
| EntityId | nvarchar | ID da entidade afectada |
| Details | nvarchar | informação adicional |
| CreatedDate | datetime2 | quando ocorreu |

---

## Queries Úteis de Diagnóstico

```sql
-- Ver certificados emitidos com nota
SELECT u.FirstName + ' ' + u.LastName AS Formando,
       c.Title AS Formacao,
       cert.ScorePercent,
       cert.ValidationCode,
       cert.CompletedDate
FROM E_Certificates cert
JOIN E_Users u ON u.Id = cert.UserId
JOIN E_Courses c ON c.Id = cert.CourseId
ORDER BY cert.CompletedDate DESC;

-- Ver progresso de um formando (lições concluídas)
SELECT l.Name AS Licao, ucl.CompletedDate
FROM R_UserCourseLessons ucl
JOIN E_Lessons l ON l.Id = ucl.LessonId
WHERE ucl.UserId = '<user-guid>'
ORDER BY ucl.CompletedDate;

-- Ver tentativas de quiz de um formando
SELECT l.Name AS Licao, ult.AttemptNumber, ult.Score, ult.Passed, ult.CompletedDate
FROM R_UserLessonTests ult
JOIN E_Lessons l ON l.Id = ult.LessonId
WHERE ult.UserId = '<user-guid>'
ORDER BY l.Name, ult.AttemptNumber;

-- Ver melhor resultado por lição (lógica usada para certificado)
SELECT LessonId, MAX(Score) AS MelhorNota
FROM R_UserLessonTests
WHERE UserId = '<user-guid>'
GROUP BY LessonId;

-- Verificar se coluna ScorePercent existe
SELECT name FROM sys.columns
WHERE object_id = OBJECT_ID('E_Certificates') AND name = 'ScorePercent';
```

---

## Notas sobre Soft Delete

As entidades principais têm `IsDeleted bit`. Actualmente o soft delete **não está implementado nas queries** — `GetAll()` retorna todos os registos incluindo `IsDeleted = 1`. Quando for implementado, adicionar filtro global no `DbContext` ou nos repositórios:

```csharp
// Exemplo de filtro global (a implementar):
modelBuilder.Entity<Course>().HasQueryFilter(e => !e.IsDeleted);
```
