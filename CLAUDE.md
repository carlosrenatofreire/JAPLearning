# CLAUDE.md — JAPLearning

Guia operacional para sessões Claude Code. Lido automaticamente no início de cada sessão.

---

## Estratégia de Contexto (Carregamento em Camadas)

Carregar apenas o necessário para cada tarefa. Nunca carregar múltiplos documentos de camadas diferentes simultaneamente sem necessidade.

### Camada Base — ler sempre no início da sessão
| Ficheiro | Porquê |
|---|---|
| `CLAUDE.md` | Este ficheiro — carregado automaticamente |
| [`STATE.md`](./STATE.md) | Decisões activas, TODOs e bloqueadores da sessão anterior |
| [`CHANGELOG.md`](./CHANGELOG.md) | O que foi feito na última sessão e o que ficou pendente |

### Camada de Domínio — ler só quando relevante para a tarefa
| Quando a tarefa envolve... | Ler |
|---|---|
| Nova funcionalidade ou módulo de negócio | `SPEC/` do módulo em questão |
| UI / componentes visuais | [`DESIGN.md`](./DESIGN.md) |
| Arquitectura, DI, padrões de código | [`ARCHITECTURE.md`](./ARCHITECTURE.md) |
| Tabelas, migrações, queries | `DATABASE/` do tipo em questão |
| Propor ou planear feature média/grande | `openspec/specs/` relevante + `/opsx:propose` |

### Camada de Referência — consultar pontualmente
| Ficheiro | Quando consultar |
|---|---|
| [`BLUEPRINT.md`](./BLUEPRINT.md) | Dúvidas sobre a estrutura da knowledge base |
| [`DBML.md`](./DBML.md) | Actualizar diagrama dbdiagram.io |

---

## Referências de Documentação

Para detalhes técnicos ver [`ARCHITECTURE.md`](./ARCHITECTURE.md).
Para regras de negócio e módulos ver [`SPEC/`](./SPEC/) — organizado por área:
- [`SPEC/00-overview.md`](./SPEC/00-overview.md) — Visão, Actores, Integrações
- [`SPEC/01-learning-core.md`](./SPEC/01-learning-core.md) — Formações, Tópicos, Lições, Questões, Opções
- [`SPEC/02-student-area.md`](./SPEC/02-student-area.md) — Fluxos do Formando, Player, Quiz, Certificados
- [`SPEC/03-users.md`](./SPEC/03-users.md) — Utilizadores, Autenticação, Roles
- [`SPEC/04-content.md`](./SPEC/04-content.md) — Testemunhos, Artigos
- [`SPEC/05-parameters.md`](./SPEC/05-parameters.md) — Equipas, Categorias, Níveis, Professores, Assuntos
- [`SPEC/06-admin.md`](./SPEC/06-admin.md) — Dashboard Admin, Relatórios, Changelog, Auditoria
Para design system e padrões de UI/UX ver [`DESIGN.md`](./DESIGN.md).
Para schema da base de dados ver [`DATABASE/`](./DATABASE/) — organizado por tipo:
- [`DATABASE/00-conventions.md`](./DATABASE/00-conventions.md) — Prefixos, diagrama, migrações, alterações manuais, queries
- [`DATABASE/01-parameters.md`](./DATABASE/01-parameters.md) — P_Teams, P_Categories, P_Levels, P_Teachers, P_Roles, P_Subjects
- [`DATABASE/02-entities.md`](./DATABASE/02-entities.md) — E_Users, E_Courses, E_Topics, E_Lessons, E_Questions, E_QuestionOptions, E_Certificates, E_Testimonials, E_Articles
- [`DATABASE/03-relationships.md`](./DATABASE/03-relationships.md) — R_UserCourseLessons, R_UserLessonTests, R_UserLessonQuestions, R_CourseRequirements
- [`DATABASE/04-auxiliaries.md`](./DATABASE/04-auxiliaries.md) — E_AppVersions, E_AppVersionItems, A_AuditLogs
Para histórico de sessões ver [`CHANGELOG.md`](./CHANGELOG.md).
Para estado activo do projecto ver [`STATE.md`](./STATE.md).
Para mapa da knowledge base e anti-patterns ver [`BLUEPRINT.md`](./BLUEPRINT.md).
Para schema DBML (dbdiagram.io) ver [`DBML.md`](./DBML.md).

---

## O Projecto

**JAPLearning** — plataforma de e-learning para a equipa **DMC-Developers**.
- Stack: **ASP.NET Core MVC (.NET 10)** + **SQL Server** + **Cloudinary** (imagens)
- Idioma da UI: **Português (Portugal)**
- Pasta raiz: `C:\JAPLearning\src\`
- Solução: `JAPLearning.sln` (5 projectos: Business, Data, Helper, Mvc, Tests)

---

## Navegação Rápida

| O que procuras | Onde está |
|---|---|
| Entidades de domínio | `Business/Models/Domains/Entities/` |
| Interfaces de serviço | `Business/Interfaces/Services/` |
| Implementações de serviço | `Business/Services/Entities/` (ou Parameters/, Relationships/) |
| Mapeamentos EF (Fluent API) | `Data/Mappings/Entities/` |
| Repositórios | `Data/Repositories/Entities/` |
| Controllers | `Mvc/Controllers/` |
| ViewModels | `Mvc/ViewModels/Entities/` (ou Parameters/, Student/, Account/) |
| Views | `Mvc/Views/[NomeController]/` |
| CSS partilhado CRUD | `Mvc/Views/Shared/_CrudStyles.cshtml` |
| Layout principal | `Mvc/Views/Shared/_LayoutApp.cshtml` |
| Sidebar admin | `Mvc/Views/Shared/_SidebarAdmin.cshtml` |
| Sidebar aluno | `Mvc/Views/Shared/_SidebarAluno.cshtml` |
| Registo de DI | `Mvc/Configurations/DependencyInjectionConfig.cs` |
| AutoMapper | `Mvc/Configurations/AutoMapperConfig.cs` |
| Middlewares | `Mvc/Middlewares/` |
| Validações FluentValidation | `Business/Validations/Internals/Entities\|Parameters\|Auxiliaries/` |
| Ficheiros privados (ZIP/PDF) | `PrivateFiles/snapshots/` e `PrivateFiles/pdfs/` (fora do wwwroot) |
| Relatórios admin | `Mvc/Controllers/ReportsController.cs` + `Mvc/Views/Reports/` |
| Changelog/Versões | `Mvc/Controllers/AppVersionsController.cs` + `Mvc/Views/AppVersions/` |

---

## Comandos

```bash
# Correr a aplicação
dotnet run --project src/JAPLearning.Mvc

# Compilar tudo
dotnet build src/JAPLearning.sln

# Nova migração EF
dotnet ef migrations add NomeMigracao --project src/JAPLearning.Data --startup-project src/JAPLearning.Mvc

# Aplicar migrações
dotnet ef database update --project src/JAPLearning.Data --startup-project src/JAPLearning.Mvc
```

---

## Armadilhas Conhecidas (Gotchas)

### 1. `ModelState.Remove` + atribuição explícita para campos de URL/ficheiro
Com `<Nullable>enable</Nullable>`, o model binder define como `null` qualquer propriedade `string` ausente do POST body (ignora o `= string.Empty` do inicializador). Isto causa `SqlException: Cannot insert NULL` mesmo sem erros de validação visíveis.

**Padrão obrigatório em controllers com upload de ficheiro:**
```csharp
// 1. Remover SEMPRE da validação (independentemente de haver foto ou não)
ModelState.Remove(nameof(vm.PhotoUrl));
if (!ModelState.IsValid) return View(vm);

// 2. Atribuir SEMPRE explicitamente — nunca confiar no valor mapeado
entity.PhotoUrl = (photo != null && photo.Length > 0)
    ? await _cloudinary.UploadImageAsync(photo, "pasta")
    : string.Empty;                      // ← nunca: entity.PhotoUrl = vm.PhotoUrl

// Em Edit (manter foto existente):
entity.PhotoUrl = (photo != null && photo.Length > 0)
    ? await _cloudinary.UploadImageAsync(photo, "pasta")
    : vm.PhotoUrl ?? string.Empty;       // ← ?? string.Empty é obrigatório
```

**Controllers afectados:** TestimonialsController, UsersController, ArticlesController (qualquer um com `IFormFile? photo`).

### 2. Validation summary — usar a classe correcta
Usar sempre `class="md-alert md-alert-danger mb-3"`, **nunca** `class="validation-summary"` (não existe no CSS).
```html
<div asp-validation-summary="ModelOnly" class="md-alert md-alert-danger mb-3"></div>
```

### 3. Form card width — override obrigatório
`.form-card` tem `max-width: 640px` por defeito no `_CrudStyles.cshtml`.
Para formulários com layout horizontal (múltiplas colunas), adicionar sempre:
```html
<div class="md-card form-card" style="max-width:100%;">
```

### 4. Coluna `ScorePercent` adicionada manualmente (sem migração)
```sql
-- Verificar e adicionar se não existir:
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('E_Certificates') AND name = 'ScorePercent')
    ALTER TABLE E_Certificates ADD ScorePercent int NOT NULL DEFAULT 0;
```

### 5. `UnitOfWork.Commit()` retorna `false` (não lança excepção)
`SaveChangesAsync() > 0` retorna `false` se 0 linhas forem gravadas.
O controller deve verificar e chamar `AddErrors()` para propagar mensagens ao utilizador.

### 6. `Path` em Razor conflitua com `HttpContext.Request.Path`
Em views Razor, `@Path` resolve para `HttpContext.Request.Path` (PathString), não `System.IO.Path`.
**Usar sempre o nome completamente qualificado:**
```razor
@System.IO.Path.GetFileName(Model.SnapshotUrl)   ✅
@Path.GetFileName(Model.SnapshotUrl)              ❌ erro CS1061
```

### 7. `v@variavel` em Razor — usar parênteses
Quando há texto literal colado antes do `@`, o Razor pode interpretar mal a expressão:
```razor
v@(Model.VersionNumber)    ✅ — mostra "v0.18"
v@Model.VersionNumber      ❌ — mostra "v@Model.VersionNumber" literalmente
```

### 8. Cloudinary raw — limite 10 MB no plano free
`UploadRawFileAsync` falha silenciosamente com `File size too large. Maximum is 10485760`.
**PDFs e ZIPs grandes devem ser guardados localmente em `PrivateFiles/`**, servidos via `PhysicalFile()` com `[Authorize]`.

### 9. `[RequestSizeLimit]` obrigatório para uploads grandes
O Kestrel tem limite padrão de 30 MB por request. Para uploads de ficheiros grandes:
```csharp
[RequestSizeLimit(256 * 1024 * 1024)]
[RequestFormLimits(MultipartBodyLengthLimit = 256 * 1024 * 1024)]
public async Task<IActionResult> Edit(..., IFormFile? snapshotFile, IFormFile? pdfFile)
```

### 10. Migração EF requer variável de ambiente `CONNECTIONSTRINGS`
O projecto usa Doppler para secrets. Para criar/aplicar migrações localmente:
```bash
CONNECTIONSTRINGS="Data Source=...;Initial Catalog=DEV_JAPLEARNING;..." \
  dotnet ef migrations add NomeMigracao \
  --project src/JAPLearning.Data \
  --startup-project src/JAPLearning.Mvc
```

---

## Estado Actual — Sessão 10 (2026-06-05)

### Funcionalidades completas
- Quiz interactivo no Player + emissão automática de certificados
- Área de aluno completa: Dashboard, MyCourses, CourseDetail, Player, MyCertificates
- 5 Middlewares: `SecurityHeaders`, `GlobalException`, `RequestLogging`, `RateLimiting`, `AuthenticationAudit`
- Rate limiting por IP+email (cada utilizador tem contador independente)
- FluentValidation em todos os módulos (`Validations/Internals/Entities|Parameters|Auxiliaries/`)
- Relatórios admin: Rankings de Alunos, Equipas e Formações com horas assistidas
- Dashboard admin: 5 KPIs, Top 7, botões "Ver todos" para relatórios
- Tabs (Dados / Uploads / Pré-Requisitos) no formulário de Formações
- Upload de Snapshot (.zip) → `PrivateFiles/snapshots/` (local, sem limite de tamanho)
- Upload de PDF → `PrivateFiles/pdfs/` (local — Cloudinary free limita raw a 10 MB)
- Botões de download no Player e na sidebar do CourseDetail (visíveis só se preenchidos)
- Favicon SVG (chapéu de estudante laranja) em todos os layouts
- Títulos `JL | Página` em todos os layouts
- Sistema de Versões/Changelog: CRUD admin com itens aninhados + página utilizador + badge no topbar
- Linha do tempo na página Changelog (coluna direita sticky 390px, highlight automático ao scroll)
- Carrossel interactivo no Hero da Landing + página de Login — 5 slides animados, auto-play 4s, dots + setas
- Página pública "Porquê Formar?" (`/Home/PorqueFormacao`) — stats, pilares, antes/depois, como funciona, quote, CTA
- Login tracking para Alunos — `LoginCount` + `LastLoginDate` via `ExecuteUpdateAsync` (cirúrgico)
- Troca de senha obrigatória no primeiro acesso — `MustChangePassword`, redirect para `/Account/ChangePassword`, logo sem link nessa página

### Gotcha #11 — `RecordLoginAsync` usa `ExecuteUpdateAsync`
`Repository.Update(entity)` faz UPDATE de **todos** os campos — sobrescreve `MustChangePassword`.
Para actualizar só `LoginCount` e `LastLoginDate` usar `ExecuteUpdateAsync` directamente:
```csharp
return await DbSet
    .Where(u => u.Id == userId)
    .ExecuteUpdateAsync(s => s
        .SetProperty(u => u.LoginCount,    u => u.LoginCount + 1)
        .SetProperty(u => u.LastLoginDate, loginDate));
```

### Armazenamento de ficheiros
| Tipo | Onde | Notas |
|---|---|---|
| Imagens (thumbnails, fotos) | ☁️ Cloudinary | CDN + transformações automáticas |
| Snapshot ZIP | 💾 `PrivateFiles/snapshots/` | Sem limite, protegido por `[Authorize]` |
| PDF | 💾 `PrivateFiles/pdfs/` | Sem limite, protegido por `[Authorize]` |

### Pendente / Próximas Tarefas
- Notificação por email ao emitir certificado
- Dashboard de progresso por equipa (gráficos)
- Soft delete implementado nas queries (filtro global no DbContext)
