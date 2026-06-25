# SPEC/06-admin.md — Área de Administração

Páginas de gestão, relatórios e dashboard admin.

---

## Páginas de Administração

| Módulo | Controller | Views |
|--------|-----------|-------|
| Equipas | `TeamsController` | Index, Create, Edit |
| Categorias | `CategoriesController` | Index, Create, Edit |
| Formações | `CoursesController` | Index, Create, Edit, Details |
| Tópicos | `TopicsController` | Index, Create, Edit |
| Lições | `LessonsController` | Index, Create, Edit |
| Questões | `QuestionsController` | Index, Create, Edit |
| Opções de Resposta | `QuestionOptionsController` | Index, Create, Edit |
| Utilizadores | `UsersController` | Index, Create, Edit |
| Testemunhos | `TestimonialsController` | Index, Create, Edit |
| Artigos | `ArticlesController` | Index, Create, Edit |
| Certificados | `CertificatesController` | Index, Details |
| Roles | `RolesController` | Index, Create, Edit |
| Auditoria | `AuditLogsController` | Index, Details |
| Versões/Changelog | `AppVersionsController` | Index, Create, Edit, Changelog |
| Relatórios | `ReportsController` | Students, Teams, Courses |

---

## Dashboard Admin (`HomeController`)

- **5 KPIs:** Alunos, Formações Disponíveis, Em Preparação, Certificados Emitidos, Sessões
- **Top 7** de alunos, equipas e formações
- **Botões "Ver todos"** que navegam para os relatórios completos

---

## Relatórios (`ReportsController`)

| Relatório | Rota | Descrição |
|-----------|------|-----------|
| Rankings de Alunos | `/Reports/Students` | Ordenado por horas assistidas + certificados |
| Rankings de Equipas | `/Reports/Teams` | Progresso agregado por equipa |
| Rankings de Formações | `/Reports/Courses` | Formações mais frequentadas |

---

## Sistema de Versões (`AppVersionsController`)

- CRUD de versões com itens aninhados inline (Novo / Melhoria / Correcção)
- Página pública `/AppVersions/Changelog` — linha do tempo lateral sticky
- Badge `vX.XX` no topbar via `@inject IAppVersionService` no `_LayoutApp.cshtml`
- `IsPublished = false` → rascunho (não visível no Changelog)

---

## Auditoria (`AuditLogsController`)

- Lista com KPIs, filtros por nível/entidade/data/pesquisa
- Página de detalhe com StackTrace e JSON
- Apenas visível para role **Administrador**
