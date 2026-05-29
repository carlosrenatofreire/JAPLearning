# CHANGELOG.md — JAPLearning

Histórico de desenvolvimento por sessão Claude Code.
Actualizar no fim de cada sessão com o que foi feito, corrigido e o que ficou pendente.

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
