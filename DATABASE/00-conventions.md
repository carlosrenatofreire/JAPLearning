# DATABASE/00-conventions.md — Convenções e Referência Geral

---

## Convenção de Prefixos

| Prefixo | Tipo | Descrição |
|---------|------|-----------|
| `E_` | Entidades | Tabelas principais de domínio |
| `P_` | Parâmetros | Tabelas de configuração/lookup |
| `R_` | Relacionamentos | Tabelas de junção e progresso |
| `A_` | Auxiliares | Logs e tabelas de suporte |

Todas as tabelas usam `uniqueidentifier` (GUID) como chave primária (`Id`).

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

## Migrações EF aplicadas (por ordem)

| Migração | Sessão | O que faz |
|---|---|---|
| (iniciais) | 1-7 | Estrutura base completa |
| `AddCourseFileFields` | 9 | + `SnapshotUrl`, `PdfFileUrl` em `E_Courses` |
| `AddAppVersions` | 9 | Cria `E_AppVersions` + `E_AppVersionItems` |
| `AddUserLoginTracking` | 10 | + `LoginCount`, `LastLoginDate`, `MustChangePassword` em `E_Users` |

> Para aplicar numa instância nova:
> ```powershell
> $env:CONNECTIONSTRINGS = "Data Source=...;Initial Catalog=DEV_JAPLEARNING;..."
> dotnet ef database update --project src/JAPLearning.Data --startup-project src/JAPLearning.Mvc
> ```

---

## Alterações Manuais (fora de migrações EF)

> ⚠️ As colunas abaixo foram adicionadas directamente via SQL sem migração EF.
> Devem ser aplicadas manualmente em qualquer ambiente novo.

```sql
-- Sessão 3 — adiciona nota de aprovação ao certificado
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('E_Certificates') AND name = 'ScorePercent'
)
    ALTER TABLE E_Certificates ADD ScorePercent int NOT NULL DEFAULT 0;

-- Sessão 7 — campos adicionais em AuditLogs
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('A_AuditLogs') AND name = 'Action')
    ALTER TABLE A_AuditLogs ADD Action varchar(50) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('A_AuditLogs') AND name = 'EntityName')
    ALTER TABLE A_AuditLogs ADD EntityName varchar(100) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('A_AuditLogs') AND name = 'HttpStatusCode')
    ALTER TABLE A_AuditLogs ADD HttpStatusCode int NULL;
```

---

## Campos Comuns nas Entidades

```csharp
public class Entity   // base em Business/Models/Shareds/Entity.cs
{
    public Guid Id { get; set; }
}

// Padrão em entidades principais:
public bool IsActived { get; set; } = true;
public bool IsDeleted { get; set; }
public DateTime CreatedDate { get; set; }
public DateTime? ChangedDate { get; set; }
```

---

## Notas sobre Soft Delete

As entidades principais têm `IsDeleted bit`. Actualmente o soft delete **não está implementado nas queries** — `GetAll()` retorna todos os registos incluindo `IsDeleted = 1`. Quando for implementado, adicionar filtro global no `DbContext`:

```csharp
// Exemplo de filtro global (a implementar):
modelBuilder.Entity<Course>().HasQueryFilter(e => !e.IsDeleted);
```

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
