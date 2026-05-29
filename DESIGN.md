# DESIGN.md — JAPLearning

Sistema de design e padrões de UX/UI da plataforma.
Baseado no ficheiro `wwwroot/css/mundodev.css` e `Views/Shared/_CrudStyles.cshtml`.

---

## Identidade Visual

**Nome do sistema:** MundoDev Design System
**Inspiração:** Doppler.com — dark theme profissional
**Tipografia:** Inter (Google Fonts) — pesos 300, 400, 500, 600, 700, 800
**Ícones:** Bootstrap Icons (`bi bi-*`)

---

## Tokens de Design (CSS Variables)

### Cores de Fundo

| Variável | Valor | Uso |
|----------|-------|-----|
| `--md-bg-primary` | `#0D0D1A` | Fundo geral da página |
| `--md-bg-secondary` | `#13132A` | Fundo de secções alternadas |
| `--md-bg-card` | `#16162E` | Cards e painéis |
| `--md-bg-card-hover` | `#1C1C38` | Card em hover |
| `--md-bg-input` | `#1A1A32` | Inputs, selects, textareas |
| `--md-bg-sidebar` | `#10101F` | Sidebar de navegação |
| `--md-bg-navbar` | `#0A0A16` | Barra de topo |

### Cor Principal (Brand)

| Variável | Valor | Uso |
|----------|-------|-----|
| `--md-accent` | `#E8501A` | Cor laranja principal — botões, links activos, foco |
| `--md-accent-dark` | `#C8372A` | Variante escura do accent |
| `--md-gradient` | `135deg, #E8501A → #C8372A → #A01F3A` | Gradiente do botão primário e logo |
| `--md-gradient-hover` | `135deg, #FF6B35 → #E04030 → #B82545` | Gradiente em hover |
| `.md-gradient-text` | classe CSS | Texto com gradiente (títulos de destaque) |
| `.gradient-bg` | classe CSS | Fundo com gradiente (utilitário) |

### Cores de Texto

| Variável | Valor | Uso |
|----------|-------|-----|
| `--md-text-primary` | `#F0F0FF` | Títulos, texto importante |
| `--md-text-secondary` | `#A0A0C0` | Texto de corpo, labels |
| `--md-text-muted` | `#606080` | Texto de apoio, placeholders, cabeçalhos de tabela |
| `--md-text-link` | `#C86040` | Links em texto corrido |

### Cores de Estado

| Variável | Valor | Uso |
|----------|-------|-----|
| `--md-success` | `#22C55E` | Sucesso, lição concluída, resposta certa |
| `--md-warning` | `#F59E0B` | Aviso, atenção |
| `--md-danger` | `#EF4444` | Erro, eliminar, inactivo |
| `--md-info` | `#6366F1` | Informação, badges neutros |

### Bordas e Espaçamento

| Variável | Valor | Uso |
|----------|-------|-----|
| `--md-border` | `#2A2A4A` | Borda padrão de cards e inputs |
| `--md-border-light` | `#1E1E3A` | Borda subtil (separadores internos) |
| `--md-radius` | `10px` | Border-radius padrão |
| `--md-radius-lg` | `16px` | Cards grandes, modais |
| `--md-radius-sm` | `6px` | Botões pequenos, badges |
| `--md-shadow` | `0 4px 24px rgba(0,0,0,0.4)` | Sombra de card |
| `--md-shadow-lg` | `0 8px 40px rgba(0,0,0,0.6)` | Sombra de modal/overlay |
| `--md-transition` | `all 0.2s ease` | Transição padrão |
| `--md-navbar-height` | `64px` | Altura da navbar |
| `--md-sidebar-width` | `300px` | Largura da sidebar do player |

---

## Componentes — Referência Rápida

### Botões

```html
<!-- Primário — laranja gradiente; trigger do overlay de loading -->
<button class="btn-md-primary">
    <i class="bi bi-check-lg"></i> Guardar
</button>

<!-- Outline — contorno, sem preenchimento -->
<a class="btn-md-outline">Ver Detalhes</a>

<!-- Ghost — discreto, para acções secundárias -->
<a class="btn-md-ghost">
    <i class="bi bi-arrow-left"></i> Voltar
</a>

<!-- Sucesso — verde, para confirmar acções positivas -->
<button class="btn-md-success">
    <i class="bi bi-award"></i> Concluir
</button>
```

> ⚠️ **Regra:** Qualquer `<form>` com `<button class="btn-md-primary" type="submit">` activa automaticamente o overlay de loading global definido em `_LayoutApp.cshtml`.

---

### Cards

```html
<!-- Card padrão (padding 24px, border-radius 16px) -->
<div class="md-card">
    conteúdo
</div>

<!-- Card pequeno (padding 16px, border-radius 10px) -->
<div class="md-card-sm">
    conteúdo
</div>

<!-- Form card — variante para formulários (max-width: 640px por defeito) -->
<div class="md-card form-card" style="max-width:100%;">
    <!-- sempre override para formulários horizontais -->
</div>
```

---

### Badges

```html
<!-- Laranja (accent) -->
<span class="md-badge md-badge-accent">Em Destaque</span>

<!-- Verde (sucesso) -->
<span class="md-badge md-badge-success">Concluído</span>

<!-- Roxo (info) -->
<span class="md-badge md-badge-info">JavaScript</span>

<!-- Amarelo (aviso) -->
<span class="md-badge md-badge-warning">Pendente</span>

<!-- Estado activo/inactivo (tabelas CRUD) -->
<span class="badge-active"><i class="bi bi-check-circle-fill"></i> Activo</span>
<span class="badge-inactive"><i class="bi bi-x-circle-fill"></i> Inactivo</span>
```

---

### Alertas

```html
<!-- Erro de validação de formulário — SEMPRE usar esta classe -->
<div asp-validation-summary="ModelOnly" class="md-alert md-alert-danger mb-3"></div>

<!-- Alerta de sucesso (TempData) -->
<div class="md-alert md-alert-success">
    <i class="bi bi-check-circle"></i> Operação realizada com sucesso.
</div>

<!-- Alerta informativo -->
<div class="md-alert md-alert-info">
    <i class="bi bi-info-circle"></i> Informação importante.
</div>
```

---

### Formulários

```html
<!-- Estrutura completa de um campo -->
<div class="md-form-row">
    <label class="md-label">Nome <span class="text-danger">*</span></label>
    <input asp-for="Name" class="md-input" placeholder="..." />
    <span asp-validation-for="Name" class="text-danger" style="font-size:12px;"></span>
</div>

<!-- Textarea -->
<textarea asp-for="Description" class="md-input" rows="4"></textarea>

<!-- Select -->
<select asp-for="CategoryId" asp-items="ViewBag.Categories" class="md-select">
    <option value="">Seleccionar...</option>
</select>

<!-- Checkbox -->
<label class="md-form-check">
    <input asp-for="IsActived" type="checkbox" />
    <span>Activo</span>
</label>
```

**Comportamento dos inputs:**
- Foco: borda muda para `--md-accent` + glow `rgba(232,80,26,0.12)`
- Placeholder: cor `--md-text-muted`
- Select: seta customizada laranja; `appearance: none`

---

### Tabelas CRUD

```html
<table class="table crud-table" id="dataTable">
    <thead>
        <tr>
            <th>Nome</th>
            <th>Estado</th>
            <th style="width:100px;">Acções</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Valor</td>
            <td><span class="badge-active">Activo</span></td>
            <td>
                <a class="action-btn" title="Editar"><i class="bi bi-pencil"></i></a>
                <a class="action-btn danger" title="Eliminar"><i class="bi bi-trash"></i></a>
            </td>
        </tr>
    </tbody>
</table>
```

**DataTables** estão configuradas com dark theme via overrides em `_CrudStyles.cshtml`:
- Caixa de pesquisa no topo direito
- Paginação no rodapé
- Ícones de ordenação Bootstrap Icons (não imagens padrão)
- Linha activa da paginação com cor accent

---

### Barra de Progresso

```html
<div class="md-progress">
    <div class="md-progress-bar" style="width: 65%;"></div>
</div>
```

Altura: 6px. Gradiente laranja. Transição suave (`width 0.4s ease`).

---

### Accordion (Currículo de Formação)

```html
<div class="md-accordion-item">
    <div class="md-accordion-header">
        Tópico 1 — Introdução
        <i class="bi bi-chevron-down"></i>
    </div>
    <div class="md-accordion-body">
        <a class="md-lesson-item active" href="#">
            <i class="bi bi-play-circle"></i> Lição 1
        </a>
        <a class="md-lesson-item completed" href="#">
            <i class="bi bi-check-circle-fill"></i> Lição 2
        </a>
    </div>
</div>
```

**Estados de lição:**
- `.active` → cor accent (lição actual)
- `.completed` → cor success verde
- default → texto secundário

---

### Tabs

```html
<div class="md-tabs">
    <a class="md-tab active" href="#">Visão Geral</a>
    <a class="md-tab" href="#">Conteúdo</a>
    <a class="md-tab" href="#">Avaliações</a>
</div>
```

---

### Avatar

```html
<!-- Avatar com iniciais (fallback sem foto) -->
<div class="md-avatar">JS</div>

<!-- Avatar grande -->
<div class="md-avatar md-avatar-lg">JS</div>

<!-- Avatar com foto -->
<img src="@user.PhotoUrl" class="md-avatar" style="object-fit:cover;" />
```

---

### Breadcrumb

```html
<div class="md-breadcrumb">
    <a href="/Courses">Formações</a>
    <span class="separator">/</span>
    <span class="current">Detalhes</span>
</div>
```

---

### Rows de Informação (detalhes)

```html
<div class="md-info-row">
    <span class="md-info-label"><i class="bi bi-person"></i> Utilizador</span>
    <span class="md-info-value">João Silva</span>
</div>
```

---

### Course Card (catálogo)

```html
<a class="md-course-card" href="/Student/Course/123">
    <img class="md-course-card-thumb" src="@course.Thumbnail" />
    <div class="md-course-card-body">
        <div class="md-course-card-title">Nome da Formação</div>
        <div class="md-course-card-meta">
            <span><i class="bi bi-clock"></i> 4h 30m</span>
            <span><i class="bi bi-bar-chart"></i> Intermédio</span>
        </div>
    </div>
</a>
```

Hover: eleva 3px + borda accent + sombra.

---

## Layout das Páginas

### Página de Administração (layout padrão)

```
┌─────────────────────────────────────────────────────┐
│  NAVBAR (64px) — logo + user menu                   │
├──────────┬──────────────────────────────────────────┤
│          │  .page-header                            │
│ SIDEBAR  │    título + breadcrumb + botão "Novo"    │
│          ├──────────────────────────────────────────┤
│          │  conteúdo (card + tabela / formulário)   │
│          │                                          │
└──────────┴──────────────────────────────────────────┘
```

**Page Header pattern:**
```html
<div class="page-header">
    <div>
        <h4 class="page-header-title">Testemunhos</h4>
        <p class="page-header-sub">
            <a asp-action="Index" style="color:var(--md-accent);">Testemunhos</a> / Editar
        </p>
    </div>
    <a asp-action="Create" class="btn-md-primary">
        <i class="bi bi-plus-lg"></i> Novo
    </a>
</div>
```

### Formulários — Layout Horizontal (múltiplas colunas)

Usar Bootstrap grid dentro de `row g-3`:

```html
<!-- Linha 1: campos pequenos lado a lado -->
<div class="row g-3">
    <div class="col-md-4">...</div>  <!-- 4/12 -->
    <div class="col-md-4">...</div>
    <div class="col-md-2">...</div>
    <div class="col-md-2">...</div>

    <!-- Linha 2: campo grande + checkboxes -->
    <div class="col-md-8">...</div>
    <div class="col-md-4">...</div>  <!-- checkboxes com flex column -->

    <!-- Linha 3: fullwidth -->
    <div class="col-12">...</div>
</div>
```

> **Regra:** Checkboxes em coluna vertical usam `display:flex; flex-direction:column; gap:10px;`

---

## UX — Princípios e Decisões

### Feedback ao Utilizador

| Situação | Mecanismo |
|----------|-----------|
| Formulário a submeter | Overlay global "A guardar..." (spinner branco, fundo escuro 45%) |
| Operação bem sucedida | `TempData["Success"]` → alerta verde no topo da página seguinte |
| Erro de validação | `md-alert md-alert-danger` com lista de erros |
| Campo inválido | Span vermelho abaixo do campo (`asp-validation-for`) |
| Acção destrutiva (eliminar) | Modal de confirmação (mdModal) antes do POST |

### Estados de Lição no Player

| Estado | Visual |
|--------|--------|
| Lição com vídeo, não concluída | Botão "Concluir Aula" verde |
| Lição com vídeo, sem URL | Badge cinzento "Vídeo não disponível" (sem botão) |
| Lição de quiz, por fazer | Badge "Teste a Fazer" (laranja) |
| Lição de quiz, tentativa em curso | Badge "Tentativa: X%" |
| Lição de quiz, concluída (passed) | Badge "Teste Concluído ✓" (verde) |
| Lição de quiz, sem questões | Badge "Teste não disponível" (cinzento) |

### Cores Semânticas no Quiz

| Resultado | Cor | Mensagem |
|-----------|-----|---------|
| ≥ PassingScore | `--md-success` (#22C55E) | "Parabéns!" |
| < PassingScore | `--md-accent` (#E8501A) | "Continua a tentar!" |
| Resposta correcta | `#4caf82` (verde suave) | ✓ |
| Resposta errada | `--md-danger` | ✗ |

### Scrollbar Customizada

Definida globalmente: 6px de largura, thumb com cor `--md-border`, hover muda para `--md-accent`. Aplicada automaticamente em toda a aplicação.

### Tipografia — Hierarquia

| Elemento | Tamanho | Peso | Cor |
|----------|---------|------|-----|
| Título de página (h4) | 22px | 800 | `--md-text-primary` |
| Subtítulo de secção | 28px | 800 | `--md-text-primary` |
| Label de campo | 13px | 600 | `--md-text-secondary` (uppercase, letter-spacing) |
| Texto de corpo | 15px | 400 | `--md-text-secondary` |
| Texto de suporte | 13px | 400 | `--md-text-muted` |
| Cabeçalho de tabela | 11px | 700 | `--md-text-muted` (uppercase) |
| Badge | 12px | 600 | variável por tipo |

---

## Classes Utilitárias

```css
.text-accent        → color: var(--md-accent)
.text-muted-md      → color: var(--md-text-muted)
.text-secondary-md  → color: var(--md-text-secondary)
.bg-card            → background: var(--md-bg-card)
.border-md          → border-color: var(--md-border)
.rounded-md         → border-radius: var(--md-radius)  /* 10px */
.gradient-bg        → background: var(--md-gradient)
.md-gradient-text   → texto com gradiente laranja
```

---

## Ficheiros de Estilo

| Ficheiro | Conteúdo |
|----------|----------|
| `wwwroot/css/mundodev.css` | Design system completo — tokens, componentes, utilitários |
| `wwwroot/css/site.css` | Overrides pontuais e estilos de páginas específicas |
| `Views/Shared/_CrudStyles.cshtml` | Estilos inline para páginas CRUD (tabelas, forms, DataTables) |
| `Views/Shared/_LayoutApp.cshtml` | Estilos do layout, sidebar, navbar, overlay loading |

> **Nota:** Os estilos estão divididos entre CSS estático e partials Razor.
> Ao adicionar novos componentes, preferir `mundodev.css` para reutilização global
> ou `_CrudStyles.cshtml` se for exclusivo de páginas de administração.
