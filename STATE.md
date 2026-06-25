# STATE.md — JAPLearning

Estado vivo do projecto. Actualizar quando decisões mudam, surgem bloqueadores ou ideias são adiadas.
**Não é histórico** (ver [`CHANGELOG.md`](./CHANGELOG.md)) — é o estado actual do que está activo, pendente ou decidido.

---

## Decisões Activas

Decisões técnicas tomadas que afectam código futuro. Não reverter sem discussão explícita.

| Decisão | Contexto | Desde |
|---|---|---|
| Imagens → Cloudinary; ficheiros grandes (ZIP/PDF) → `PrivateFiles/` local | Cloudinary free limita raw a 10 MB; `PhysicalFile()` + `[Authorize]` para servir | Sessão 9 |
| `MustChangePassword` aplica a **todos** os roles, não só Aluno | Supervisor também deve trocar senha no primeiro acesso | Sessão 12 |
| `RecordLoginAsync` usa `ExecuteUpdateAsync` (cirúrgico) | `Repository.Update()` sobrescreve todos os campos incluindo `MustChangePassword` | Sessão 10 |
| Specs de produto mantidas em dois formatos paralelos: `SPEC/` (referência) + `openspec/specs/` (planeamento) | `SPEC/` tem mais detalhe de implementação; OpenSpec é usado para propor novas features | Sessão 12 |
| OpenSpec instalado como ferramenta de planeamento | Usar `/opsx:propose` antes de implementar features médias/grandes | Sessão 12 |
| Soft delete ainda **não** implementado no DbContext | Filtro global pendente; queries actuais não filtram `IsDeleted` automaticamente | Sessão 8 |

---

## Bloqueadores

Impedimentos activos que bloqueiam tarefas específicas.

_Nenhum bloqueador activo._

---

## TODOs Activos

Tarefas concretas para as próximas sessões, ordenadas por prioridade aproximada.

### Alta prioridade
- [ ] **Filtro Supervisor — Questões e Opções de Questão** — aplicar filtro por `TeamId` nos módulos `QuestionsController` e `QuestionOptionsController` (mesmo padrão de Formações/Tópicos/Lições)
- [ ] **Filtro Supervisor — Categorias** — Supervisor só vê as suas categorias
- [ ] **Protecção nos POSTs Create/Edit** — validação de ownership para Supervisor (impedir que edite conteúdo de outra equipa via POST directo)
- [x] **Eliminar Lição — 403 Supervisor / 500 Admin** — corrigido: role expandido + `HasProgressRecordsAsync` antes do DELETE (Sessão 12)
- [x] **Top 7 Alunos clicável nos dashboards** — corrigido: `Id` adicionado ao `topStudents`; linhas → links para `StudentDetail` (Sessão 12)
- [x] **StudentDetail — lições agrupadas por módulo** — implementado: accordion por Tópico com cabeçalho, ícone de estado, duração e data (Sessão 12)

### Média prioridade
- [ ] **Notificação por email ao emitir certificado** — `IEmailService` existe na interface mas não está implementado na UI
- [ ] **Dashboard de progresso por equipa** — gráficos de barras/linha por equipa na área admin/supervisor

### Baixa prioridade / Ideias adiadas
- [ ] **Soft delete nas queries** — filtro global `IsDeleted = false` no DbContext via `HasQueryFilter`
- [ ] **Dashboard de progresso por equipa** — gráficos detalhados (além do Top 7 já existente)

---

## Ideias em Aberto

Sugestões identificadas mas sem decisão de implementar.

- Exportação de relatórios para Excel/PDF
- Notificação push quando novo conteúdo é publicado
- Modo de pré-visualização de formação para Supervisor antes de publicar
