# CHANGELOG.md — JAPLearning

Histórico de desenvolvimento por sessão Claude Code.
Actualizar no fim de cada sessão com o que foi feito, corrigido e o que ficou pendente.

---

## [Sessão 12] — 2026-06-25

### Adicionado
- **Detalhe de Aluno com lições aninhadas por módulo** (`Reports/StudentDetail`) — expandido para mostrar, por formação, os tópicos como cabeçalhos de módulo (ordenados por `Topic.Order`) e as lições dentro de cada tópico (ordenadas por `Lesson.Order`), com numeração sequencial global, ícone ✓/○, duração e data de conclusão; accordion "Ver aulas / Ocultar aulas" em cada card de formação
- **Top 7 Alunos clicável nos dashboards Admin e Supervisor** — cada linha passa a link para `Reports/StudentDetail/{id}`; `Id` (UserId) adicionado ao objecto `topStudents` no `HomeController`

### Corrigido
- **403 Supervisor ao eliminar Lição** — `[Authorize(Roles = "Administrador")]` alterado para `[Authorize(Roles = "Administrador,Supervisor")]`; Supervisor tem verificação de ownership (Forbid se lição não pertencer à sua equipa)
- **500 Admin ao eliminar Lição com progresso** — `LessonService.DeleteAsync` verifica previamente `HasProgressRecordsAsync` antes de tentar o DELETE SQL; se existirem registos em `R_UserCourseLessons`, devolve notificação amigável em vez de lançar FK constraint exception; mensagem exibida via `TempData["ValidationWarnings"]` após redirect
- **Duração de aulas ≥ 1h no Player** — formato `mm\:ss` substituído por condicional: `h\:mm\:ss` se `TotalHours >= 1`, `mm\:ss` caso contrário

### Ficheiros alterados
- `Business/Interfaces/Internals/Entities/ILessonRepository.cs` — + `HasProgressRecordsAsync(Guid)`
- `Data/Repositories/Entities/LessonRepository.cs` — implementação do método + `AnyAsync` em `UserCourseLessons`
- `Business/Services/Entities/LessonService.cs` — override `DeleteAsync` com verificação de progresso antes do delete
- `Mvc/Controllers/LessonsController.cs` — `Delete` action: role expandido, ownership check Supervisor, tratamento do retorno
- `Mvc/Views/Lessons/Index.cshtml` — + bloco `TempData["ValidationWarnings"]`
- `Mvc/Controllers/HomeController.cs` — + `Id = x.UserId` nos objectos `topStudents` (Admin e Supervisor)
- `Mvc/Views/Home/_DashboardSupervisor.cshtml` — linhas Top 7 Alunos → links clicáveis
- `Mvc/Views/Home/_DashboardAdmin.cshtml` — idem
- `Mvc/Controllers/ReportsController.cs` — `StudentDetail`: + `lessonDetails` agrupado por Tópico → `ViewBag.LessonsByCourse`
- `Mvc/Views/Reports/StudentDetail.cshtml` — accordion de lições por módulo; estilos `.sd-toggle-btn`, `.sd-lessons-panel`, `.sd-lesson-row`
- `Mvc/Views/Student/Player.cshtml` — formato de duração corrigido para aulas ≥ 1h

### Pendente para próxima sessão
- Módulos Questões e Opções de Questão (filtro Supervisor por TeamId)
- Módulo Categorias (filtro Supervisor — só as suas)
- Protecção nos POSTs Create/Edit (validação de ownership para Supervisor)
- Notificação por email ao emitir certificado
- Dashboard de progresso por equipa (gráficos)
- Soft delete nas queries (filtro global no DbContext)

---

## [Sessão 11] — 2026-06-05

### Adicionado
- **Reorganização da knowledge base** — `SPEC.md` dividido em `SPEC/` (7 ficheiros por área); `DATABASE.md` dividido em `DATABASE/` (5 ficheiros por tipo); `BLUEPRINT.md` e `DBML.md` actualizados e referenciados no `CLAUDE.md`
- **Role Supervisor departamental** — Fase 1 completa:
  - Sidebar condicional: Supervisor não vê Utilizadores, Testemunhos, Certificados, Equipas, Níveis, Perfis, Módulos, Permissões, Auditoria, Versões
  - Dashboard Supervisor (`_DashboardSupervisor.cshtml`) — 5 KPIs scoped ao departamento (Alunos, Formações Disponíveis, Em Preparação, Certificados, Categorias), Top 7 Alunos + Top 7 Lições + Top 7 Formações do departamento
  - Filtro de dados nos módulos Formações, Tópicos e Lições — controller filtra por `TeamId` do Supervisor; dropdown de Equipa mostra só a sua equipa e auto-selecciona; filtro por categoria/formação/tópico via `ext.search` DataTables
  - `TeamId` adicionado às claims no login (`AccountController`)
  - `ViewBag.SupervisorTeamId` passado para as views para auto-selecção JS

### Corrigido
- **`isSupervisor` variável Razor indefinida** — causava compilação silenciosa da view antiga em cache; todas as correcções JS eram ignoradas pelo browser; corrigido para `supervisorTeamId`
- **`ext.search` DataTables com timing** — registado após o layout inicializar o DataTables; o filtro só corre quando o utilizador interage (não bloqueia o carregamento inicial)
- **`DBML.md`** — actualizado com `E_Users` (LoginCount, LastLoginDate, MustChangePassword), `E_Courses` (SnapshotUrl, PdfFileUrl), `E_AppVersions/Items`, `A_AuditLogs` (Action, EntityName, HttpStatusCode)
- **`BLUEPRINT.md`** — referências a `SPEC.md` e `DATABASE.md` substituídas pelas pastas `SPEC/` e `DATABASE/`

### Ficheiros criados
- `SPEC/00-overview.md` a `SPEC/06-admin.md`
- `DATABASE/00-conventions.md` a `DATABASE/04-auxiliaries.md`
- `Mvc/Views/Home/_DashboardSupervisor.cshtml`

### Ficheiros alterados
- `CLAUDE.md` — referências actualizadas para `SPEC/`, `DATABASE/`, `BLUEPRINT.md`, `DBML.md`
- `BLUEPRINT.md` — diagrama e tabelas actualizados
- `DBML.md` — schema completo actualizado (Sessões 7, 9, 10)
- `Mvc/Controllers/AccountController.cs` — + claim `TeamId` no login
- `Mvc/Controllers/CoursesController.cs` — filtro Supervisor por TeamId + ViewBag.SupervisorTeamId
- `Mvc/Controllers/TopicsController.cs` — idem
- `Mvc/Controllers/LessonsController.cs` — idem
- `Mvc/Views/Shared/_SidebarAdmin.cshtml` — condicionais por role
- `Mvc/Views/Home/Index.cshtml` — routing para `_DashboardSupervisor`
- `Mvc/Views/Courses/Index.cshtml` — filtro Supervisor + ext.search
- `Mvc/Views/Topics/Index.cshtml` — idem
- `Mvc/Views/Lessons/Index.cshtml` — idem

### Pendente para próxima sessão
- Módulos Questões e Opções de Questão (filtro Supervisor)
- Módulo Categorias (filtro Supervisor — só as suas)
- Protecção nos POSTs Create/Edit (validação de ownership para Supervisor)
- Notificação por email ao emitir certificado
- Dashboard de progresso por equipa (gráficos)
- Soft delete nas queries (filtro global no DbContext)

---

## [Sessão 10] — 2026-06-05

### Adicionado
- **Carrossel interactivo no Hero da Landing Page** — substitui as 4 janelas em cascata por 5 slides animados (Catálogo, Detalhe, Player, Dashboard Aluno, Dashboard Admin) com auto-play 4s, pausa no hover, dots + setas de navegação
- **Mesmo carrossel aplicado na página de Login** (`_LayoutAuth.cshtml`) — carrossel centrado verticalmente, 15% maior, com texto proporcional abaixo
- **Página pública "Porquê Formar?"** (`/Home/PorqueFormacao`) — 6 secções: Hero com mini-cards de estatísticas, Os Números Falam (4 stats com fontes), Os 4 Pilares, Antes vs Depois (tabela comparativa), Como Funciona (3 passos), Quote + CTA final. Link adicionado à navbar pública
- **Linha do tempo na página Changelog** (`/AppVersions/Changelog`) — coluna direita sticky com nós circulares, highlight automático ao scroll, clique navega até à versão; coluna expandida para 390px
- **Login tracking para Alunos** — regista `LoginCount` e `LastLoginDate` a cada login bem sucedido
- **Troca de senha obrigatória no primeiro acesso** — novos utilizadores criados com `MustChangePassword = true`; após login de Aluno com flag activo, redireccionado para `/Account/ChangePassword` antes de aceder ao Dashboard; formulário com indicador de força de senha e toggle de visibilidade

### Base de Dados (migração EF aplicada)
- `AddUserLoginTracking` — + `LoginCount int NOT NULL DEFAULT 0`, `LastLoginDate datetime2 NULL`, `MustChangePassword bit NOT NULL DEFAULT 0` em `E_Users`

### Ficheiros criados
- `Mvc/Views/Home/PorqueFormacao.cshtml`
- `Mvc/Views/Account/ChangePassword.cshtml`
- `Mvc/ViewModels/Account/ChangePasswordViewModel.cs`

### Ficheiros alterados
- `Mvc/Views/Home/Landing.cshtml` — carrossel hero (5 slides)
- `Mvc/Views/Shared/_LayoutAuth.cshtml` — carrossel + centrado + 15% maior
- `Mvc/Views/Shared/_LayoutPublic.cshtml` — + link "Porquê Formar?" na navbar
- `Mvc/Views/AppVersions/Changelog.cshtml` — linha do tempo lateral + coluna 390px
- `Mvc/Controllers/HomeController.cs` — + action `PorqueFormacao`
- `Mvc/Controllers/AccountController.cs` — login tracking + `ChangePassword` GET/POST
- `Mvc/Controllers/UsersController.cs` — novos utilizadores com `MustChangePassword = true`
- `Business/Models/Domains/Entities/User.cs` — + `LoginCount`, `LastLoginDate`, `MustChangePassword`
- `Business/Interfaces/Services/Entities/IUserService.cs` — + `RecordLoginAsync`, `ChangePasswordAsync`
- `Business/Services/Entities/UserService.cs` — implementação dos dois novos métodos
- `Data/Mappings/Entities/UserMapping.cs` — mapeamento dos 3 novos campos

### Pendente
- Notificação por email ao emitir certificado
- Dashboard de progresso por equipa (gráficos)
- Soft delete implementado nas queries (filtro global no DbContext)

---

## [Sessão 9] — 2026-06-04

### Adicionado
- **Sistema de Versões/Changelog** (`E_AppVersions` + `E_AppVersionItems`):
  - CRUD admin em `AppVersionsController` com itens aninhados inline (Novo/Melhoria/Correcção)
  - Página `/AppVersions/Changelog` acessível a todos os utilizadores autenticados
  - Badge `vX.XX` no topbar via `@inject IAppVersionService` no `_LayoutApp.cshtml`
  - Menu admin: "Versões" em Administração; menu aluno: "Novidades" em Explorar
- **Favicon SVG** (`wwwroot/favicon.svg`) — chapéu de estudante laranja (#E8501A)
- **Títulos** `JL | Página` em todos os layouts (_LayoutApp, _LayoutAuth, _LayoutPlayer, _LayoutPublic)
- **Upload de ficheiros privados** nas Formações:
  - Campo Snapshot (.zip) → `PrivateFiles/snapshots/{courseId}.zip` (local, sem limite)
  - Campo PDF → `PrivateFiles/pdfs/{courseId}.pdf` (local — Cloudinary free limita a 10 MB)
  - `[RequestSizeLimit(256MB)]` + `[RequestFormLimits(256MB)]` nos actions Create/Edit
  - Tabs no formulário (Dados / Uploads / Pré-Requisitos)
- **Botões de download** no Player e sidebar do CourseDetail (visíveis só se preenchidos)
- **Horas Assistidas** corrigido: calcula soma de `TimeLesson` das lições concluídas (não `WatchedSeconds`)
- **Coluna Horas Assistidas** no ranking de alunos (relatório admin)

### Corrigido
- `Path` em Razor conflitua com `HttpContext.Request.Path` → usar `System.IO.Path`
- `v@variavel` em Razor → usar `v@(variavel)` com parênteses
- Cloudinary raw upload: `Api.Timeout = 300` era 300ms → corrigido para `600000` (10 min); limite free 10 MB identificado → migração para local

### Base de Dados (migrações EF aplicadas)
- `E_Courses`: + `SnapshotUrl varchar(500)` + `PdfFileUrl varchar(500)` — migração `AddCourseFileFields`
- `E_AppVersions`: nova tabela (Id, VersionNumber, Title, ReleaseDate, IsPublished) — migração `AddAppVersions`
- `E_AppVersionItems`: nova tabela (Id, VersionId FK, Type int, Description, Order)

### Ficheiros criados
- `Business/Models/Domains/Auxiliaries/AppVersion.cs`
- `Business/Models/Domains/Auxiliaries/AppVersionItem.cs`
- `Business/Interfaces/Internals/Auxiliaries/IAppVersionRepository.cs`
- `Business/Interfaces/Services/Auxiliaries/IAppVersionService.cs`
- `Business/Services/Auxiliaries/AppVersionService.cs`
- `Data/Mappings/Auxiliaries/AppVersionMapping.cs`
- `Data/Mappings/Auxiliaries/AppVersionItemMapping.cs`
- `Data/Repositories/Auxiliaries/AppVersionRepository.cs`
- `Mvc/Controllers/AppVersionsController.cs`
- `Mvc/ViewModels/Auxiliaries/AppVersionViewModel.cs`
- `Mvc/Views/AppVersions/Index.cshtml`
- `Mvc/Views/AppVersions/Create.cshtml`
- `Mvc/Views/AppVersions/Edit.cshtml`
- `Mvc/Views/AppVersions/Changelog.cshtml`
- `Mvc/ViewComponents/LatestVersionViewComponent.cs`
- `wwwroot/favicon.svg`
- `PrivateFiles/snapshots/` (pasta local, fora do wwwroot)
- `PrivateFiles/pdfs/` (pasta local, fora do wwwroot)

### Ficheiros alterados
- `Mvc/Controllers/CoursesController.cs` — upload ZIP/PDF local + tabs view
- `Mvc/Controllers/StudentController.cs` — DownloadSnapshot + DownloadPdf (PhysicalFile)
- `Mvc/ViewModels/Entities/CourseViewModel.cs` — + SnapshotUrl, PdfFileUrl
- `Mvc/Views/Courses/Edit.cshtml` — tabs + campos upload
- `Mvc/Views/Courses/Create.cshtml` — tabs + campos upload
- `Mvc/Views/Student/Player.cshtml` — botões download ZIP + PDF
- `Mvc/Views/Student/CourseDetail.cshtml` — secção Anexos na sidebar
- `Mvc/Views/Student/Dashboard.cshtml` — horas assistidas com formato inteligente
- `Mvc/Views/Shared/_LayoutApp.cshtml` — favicon + título JL | + badge versão
- `Mvc/Views/Shared/_LayoutAuth.cshtml` — favicon + título JL |
- `Mvc/Views/Shared/_LayoutPlayer.cshtml` — favicon + título JL |
- `Mvc/Views/Shared/_LayoutPublic.cshtml` — favicon + título JL |
- `Mvc/Views/Shared/_SidebarAdmin.cshtml` — item "Versões"
- `Mvc/Views/Shared/_SidebarAluno.cshtml` — item "Novidades"
- `Mvc/Configurations/DependencyInjectionConfig.cs` — + IAppVersionRepository/Service
- `Mvc/Configurations/WebAppConfig.cs` — UseDeveloperExceptionPage em Development
- `Business/Services/Externals/CloudinaryService.cs` — + UploadRawFileAsync, DeleteRawFileAsync; timeout 600000ms
- `Business/Interfaces/Externals/ICloudinaryService.cs` — + UploadRawFileAsync, DeleteRawFileAsync

### Pendente
- Notificação por email ao emitir certificado
- Dashboard de progresso por equipa (gráficos)

---

## [Sessão 8] — 2026-06-01

### Adicionado
- **5 Middlewares** em `Mvc/Middlewares/`:
  - `SecurityHeadersMiddleware` — X-Frame-Options, X-Content-Type-Options, CSP, HSTS
  - `GlobalExceptionMiddleware` — captura excepções, loga em AuditLog, redireciona /Errors/500
  - `RequestLoggingMiddleware` — regista POST/PUT/DELETE de utilizadores autenticados
  - `RateLimitingConfig` — política "login" (5 tentativas/5min por IP+email) e "general" (120/min por IP)
  - `AuthConfig` cookie events — loga login, logout, 403 na AuditLog
- **FluentValidation** reorganizada em `Business/Validations/Internals/Entities|Parameters|Auxiliaries/`
  - Validações criadas para todos os módulos (Articles, Users, Testimonials, Questions, QuestionOptions, Teachers, Levels, Subjects, Modules, Permissions, Roles, Teams, Categories, AuditLog)
- **Relatórios admin** (`ReportsController`): Rankings de Alunos, Equipas e Formações
- **Dashboard admin** melhorado: 5 KPIs (Alunos, Formações Disponíveis, Em Preparação, Certificados, Sessões), Top 7 em cada coluna, botões "Ver todos" para relatórios
- **Rate limiting por IP+email** — cada utilizador tem contador independente (bloquear admin@admin não bloqueia outros utilizadores do mesmo IP)

### Corrigido
- `BaseService.ValidateAsync` — fire-and-forget causava `InvalidOperationException` (DbContext concorrente); corrigido com `async/await` explícito
- Blank page no login — `app.MapPost("/Account/Login", ...)` conflituava com MVC; removido
- Rate limiting não bloqueava — `UseRateLimiter()` antes de `UseRouting()` → reordenado pipeline
- KPI "Certificados Emitidos" mostrava 0 — `users.Sum(u => u.Certificates?.Count)` com nav property não carregada; corrigido com `ICertificateService.GetAllAsync()`

### Ficheiros criados/alterados
- `Mvc/Middlewares/SecurityHeadersMiddleware.cs`
- `Mvc/Middlewares/GlobalExceptionMiddleware.cs`
- `Mvc/Middlewares/RequestLoggingMiddleware.cs`
- `Mvc/Configurations/RateLimitingConfig.cs`
- `Mvc/Configurations/AuthConfig.cs` — cookie events
- `Mvc/Configurations/WebAppConfig.cs` — pipeline reordenado
- `Mvc/Controllers/AccountController.cs` — [EnableRateLimiting("login")] + IAuditLogService
- `Mvc/Controllers/HomeController.cs` — 5 KPIs, Top 7, ICertificateService
- `Mvc/Controllers/ReportsController.cs` — criado
- `Mvc/Views/Home/_DashboardAdmin.cshtml`
- `Mvc/Views/Reports/Students.cshtml`, `Teams.cshtml`, `Courses.cshtml` — criados
- `Mvc/Views/Account/Login.cshtml` — TempData["RateLimitError"]
- `Business/Validations/Internals/` — todos os ficheiros de validação
- `Business/Services/BaseService.cs` — ValidateAsync async

### Pendente
- (resolvido na sessão 9)

---

## [Sessão 7] — 2026-05-28

### Adicionado
- **Módulo de Auditoria** (`AuditLogsController` + `Views/AuditLogs/`) — lista com KPIs, filtros (nível/entidade/data/pesquisa) e página de detalhe com StackTrace e JSON
- **3 campos novos em `A_AuditLogs`**: `Action varchar(50)`, `EntityName varchar(100)`, `HttpStatusCode int null`
- **`IAuditLogService`** — novo método `LogErrorAsync` (substitui `LogAsync` por `LogInfoAsync` e `LogErrorAsync`)
- **Menu Admin** — item "Auditoria" adicionado à secção Administração (apenas role Administrador)
- **Menu Aluno** — item "Minhas Formações" adicionado à secção Principal
- **Fluxo CourseDetail na área do aluno** — novo passo intermédio entre lista de formações e player; botão inteligente (Começar/Continuar/Rever)
- **Dashboard aluno** — layout 50/50 com colunas "Formações em Progresso" e "Formações Concluídas"

### Corrigido
- Thumbnails nas views MyCourses e Dashboard (estavam sempre a mostrar ícone genérico)
- Proporção da imagem principal em `CourseDetail` pública: `16:9` → `5:2`
- Card sidebar público: imagem repetida substituída por header com gradiente + ícone
- Paginação de testemunhos na Landing: `Take(6)` removido do HomeController

### Alterações Manuais BD (executar no SSMS)
```sql
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('A_AuditLogs') AND name = 'Action')
    ALTER TABLE A_AuditLogs ADD Action varchar(50) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('A_AuditLogs') AND name = 'EntityName')
    ALTER TABLE A_AuditLogs ADD EntityName varchar(100) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('A_AuditLogs') AND name = 'HttpStatusCode')
    ALTER TABLE A_AuditLogs ADD HttpStatusCode int NULL;
```

### Ficheiros alterados
- `Business/Models/Domains/Auxiliaries/AuditLog.cs`
- `Business/Interfaces/Services/Auxiliaries/IAuditLogService.cs`
- `Business/Services/Auxiliaries/AuditLogService.cs`
- `Data/Mappings/Auxiliaries/AuditLogMapping.cs`
- `Mvc/ViewModels/Auxiliaries/AuditLogViewModel.cs`
- `Mvc/Controllers/AuditLogsController.cs` — criado
- `Mvc/Views/AuditLogs/Index.cshtml` — criado
- `Mvc/Views/AuditLogs/Details.cshtml` — criado
- `Mvc/Views/Shared/_SidebarAdmin.cshtml`
- `Mvc/Views/Shared/_SidebarAluno.cshtml`
- `Mvc/Controllers/StudentController.cs`
- `Mvc/Views/Student/CourseDetail.cshtml` — criado
- `Mvc/Views/Student/Courses.cshtml`
- `Mvc/Views/Student/Dashboard.cshtml`
- `Mvc/Views/Student/MyCourses.cshtml`
- `Mvc/Views/Home/CourseDetail.cshtml`
- `Mvc/Controllers/HomeController.cs`

---

## [Sessão 6] — 2026-05-28

### Corrigido
- **`SqlException: Cannot insert NULL into column 'PhotoUrl'`** — `.NET 10` com `<Nullable>enable</Nullable>` define strings ausentes do POST body como `null` (ignora inicializador `= string.Empty`). Em `TestimonialsController.Create`, a entidade recebia `PhotoUrl = null` via AutoMapper → falha SQL.
  - Fix Create: `ModelState.Remove` sempre antes de `IsValid`; atribuição explícita `entity.PhotoUrl = (photo != null) ? url : string.Empty`.
  - Fix Edit: `entity.PhotoUrl = vm.PhotoUrl ?? string.Empty` (nunca omitir o `?? string.Empty`).
- **`CLAUDE.md` Gotcha #1** — actualizado com padrão completo obrigatório para todos os controllers com `IFormFile? photo`.

### Adicionado
- **Paginação de Testemunhos na Landing Page** — 6 testemunhos por página, setas Prev/Next, dots de navegação, JS IIFE (`goTo`, `testimonialNav`, `testimonialGoTo`). Reutiliza CSS já existente (`.testimonials-dot`, `.testimonials-nav`).
- **`DESIGN.md`** — design system completo: variáveis CSS, componentes HTML, regras UX.
- **`BLUEPRINT.md`** — diagrama ASCII da estrutura da knowledge base, tabela de responsabilidades, fluxo de sessão, stack técnica, anti-patterns.
- **`DBML.md`** — script DBML completo para todas as 23 tabelas (compatível com dbdiagram.io), índices, alterações manuais, diagrama de relações ASCII.

### Ficheiros alterados
- `src/JAPLearning.Mvc/Controllers/TestimonialsController.cs` — fix `PhotoUrl` null (Create + Edit POST)
- `src/JAPLearning.Mvc/Views/Home/Landing.cshtml` — paginação de testemunhos
- `CLAUDE.md` — Gotcha #1 actualizado com padrão completo
- `DESIGN.md` — criado
- `BLUEPRINT.md` — criado
- `DBML.md` — criado

### Pendente
- Verificar `ScorePercent` aplicado na BD de produção
- Testar fluxo completo: quiz → conclusão → emissão de certificado
- Dashboard de progresso por equipa (relatórios)
- Notificação por email ao emitir certificado

---

## [Sessão 5] — 2026-05-27

### Corrigido
- **Testimonials Edit POST** — faltava `ModelState.Remove(nameof(vm.PhotoUrl))` (presente no Create mas ausente no Edit). Com `<Nullable>enable</Nullable>`, `PhotoUrl` tem `[Required]` implícito; testemunhos sem foto guardam `""` que falha validação silenciosamente.
- **Validation summary invisível** — classes `validation-summary` (inexistente no CSS) substituídas por `md-alert md-alert-danger mb-3` nos formulários Create e Edit de Testemunhos.

### Adicionado
- **`CLAUDE.md`** — guia operacional para sessões Claude Code (navegação, comandos, gotchas, estado).
- **`ARCHITECTURE.md`** — arquitectura técnica, padrões de código, convenções.
- **`SPEC.md`** — especificação de produto, módulos, regras de negócio, fluxos.
- **`CHANGELOG.md`** — este ficheiro.
- **`DATABASE.md`** — schema completo da base de dados.

### Ficheiros alterados
- `src/JAPLearning.Mvc/Controllers/TestimonialsController.cs` — Edit POST + `ModelState.Remove`
- `src/JAPLearning.Mvc/Views/Testimonials/Create.cshtml` — validation summary CSS
- `src/JAPLearning.Mvc/Views/Testimonials/Edit.cshtml` — validation summary CSS

---

## [Sessão 4] — 2026-05-xx

### Adicionado
- **Detalhes do Certificado** (`Certificates/Details.cshtml`) — reescrito com card centrado, ícones, grelha de detalhes e iframe de pré-visualização do certificado.
- **Formulários horizontais — Testemunhos** (`Testimonials/Create.cshtml` e `Edit.cshtml`) — layout Bootstrap grid, upload de foto com preview, campos reorganizados.
- **Formulários horizontais — Questões** (`Questions/Create.cshtml` e `Edit.cshtml`) — layout horizontal, campo Questão convertido para `textarea`.
- **Formulários horizontais — Opções de Resposta** (`QuestionOptions/Create.cshtml` e `Edit.cshtml`) — layout horizontal, campo Texto da Opção como `textarea`, override `max-width:100%`.
- **8 sugestões de testemunhos** geradas para a equipa DMC-Developers (6 Full-Stack Developers + 1 Tech Lead + 1 Tech Champion).

### Ficheiros alterados
- `src/JAPLearning.Mvc/Views/Certificates/Details.cshtml`
- `src/JAPLearning.Mvc/Views/Testimonials/Create.cshtml`
- `src/JAPLearning.Mvc/Views/Testimonials/Edit.cshtml`
- `src/JAPLearning.Mvc/Views/Questions/Create.cshtml`
- `src/JAPLearning.Mvc/Views/Questions/Edit.cshtml`
- `src/JAPLearning.Mvc/Views/QuestionOptions/Create.cshtml`
- `src/JAPLearning.Mvc/Views/QuestionOptions/Edit.cshtml`

---

## [Sessão 3] — 2026-05-xx

### Adicionado
- **Sistema de Quiz completo no Player**:
  - JS: `selectOption`, `nextQuestion`, `finishQuiz`, `retryQuiz`, `completeFromQuiz`
  - AJAX POST para `StudentController.SaveQuizResult` (sem antiforgery, JSON body)
  - Gravação de resultados em `R_UserLessonTests` (múltiplas tentativas)
- **Melhorias visuais do Quiz**:
  - Ícone grande circular laranja à esquerda da pergunta
  - Opções com fundo diferente (`bg-card`) para distinção visual
  - Letras A/B/C/D nos círculos das opções
- **Lições de Quiz** — comportamento distinto no Player:
  - Sem área de vídeo
  - Badge de estado: "Teste a Fazer" / "Tentativa: X%" / "Teste Concluído ✓"
  - Botão "Teste não disponível" quando quiz sem questões cadastradas
- **`CertificateView.cshtml`** — view standalone imprimível (`Layout=null`) com `window.print()` e `@media print`.
- **`MyCertificates.cshtml`** — botão "Ver Certificado" + exibe `ScorePercent`.
- **`IUserLessonTestService`** + **`UserLessonTestService`** — novos serviços para gerir tentativas de quiz.

### Corrigido
- **Botão "Concluir Aula"** desactivado visivelmente para lições sem vídeo — substituído por badge cinzento "Vídeo não disponível" (sem botão renderizado).

### Base de Dados
- **`ScorePercent`** adicionado a `E_Certificates` **manualmente** (sem migração EF):
  ```sql
  ALTER TABLE E_Certificates ADD ScorePercent int NOT NULL DEFAULT 0;
  ```

### Ficheiros alterados/criados
- `src/JAPLearning.Business/Models/Domains/Entities/Certificate.cs`
- `src/JAPLearning.Business/Interfaces/Services/Entities/ICertificateService.cs`
- `src/JAPLearning.Business/Services/Entities/CertificateService.cs`
- `src/JAPLearning.Business/Interfaces/Services/Relationships/IUserLessonTestService.cs` ← NOVO
- `src/JAPLearning.Business/Services/Relationships/UserLessonTestService.cs` ← NOVO
- `src/JAPLearning.Data/Mappings/Entities/CertificateMapping.cs`
- `src/JAPLearning.Mvc/Configurations/DependencyInjectionConfig.cs`
- `src/JAPLearning.Mvc/Controllers/StudentController.cs`
- `src/JAPLearning.Mvc/ViewModels/Student/PlayerViewModel.cs`
- `src/JAPLearning.Mvc/ViewModels/Entities/CertificateViewModel.cs`
- `src/JAPLearning.Mvc/Views/Student/Player.cshtml`
- `src/JAPLearning.Mvc/Views/Student/CertificateView.cshtml` ← NOVO
- `src/JAPLearning.Mvc/Views/Student/MyCertificates.cshtml`

---

## [Sessão 2] — 2026-05-xx

### Adicionado
- **Lógica de emissão de certificados** em `StudentController.CompleteLesson`:
  - Verifica se todas as lições estão concluídas
  - Calcula média das **melhores notas** por lição (lições sem quiz = 100%)
  - Emite certificado se média ≥ `Course.PassingScore`
  - Um certificado por utilizador/formação (`HasCertificateAsync`)
- **`IssueCertificateAsync`** em `CertificateService` — cria registo com `ValidationCode` único de 12 chars.
- **`PassingScore`** no `Course` (default 60) como limiar de aprovação.

### Ficheiros alterados
- `src/JAPLearning.Business/Services/Entities/CertificateService.cs`
- `src/JAPLearning.Mvc/Controllers/StudentController.cs`

---

## [Sessão 1] — 2026-05-xx

### Base inicial
- Estrutura da solução com 5 projectos (Business, Data, Helper, Mvc, Tests)
- CRUD de: Equipas, Categorias, Formações, Tópicos, Lições, Questões, Opções, Utilizadores, Testemunhos, Artigos, Certificados, Roles
- Player de lições com suporte a vídeo (iframe YouTube/Vimeo)
- Autenticação com roles (Administrador, Supervisor, Formando)
- Design system custom (variáveis CSS, componentes md-*)
- Integração Cloudinary para imagens
- Layout responsivo com sidebar e topbar

---

## Template para Nova Sessão

Copiar e preencher no fim de cada sessão:

```markdown
## [Sessão N] — YYYY-MM-DD

### Adicionado
-

### Corrigido
-

### Base de Dados
- (listar qualquer ALTER TABLE manual ou migração adicionada)

### Ficheiros alterados/criados
-

### Pendente para próxima sessão
-
```
