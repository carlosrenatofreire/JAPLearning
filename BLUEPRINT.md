# BLUEPRINT: Knowledge Base (Estado Partilhado) — JAPLearning
> **Camada Central de Governação, Padrões e Alinhamento Técnico do Claude Code**

## 🗺️ Mapa de Contexto e Ficheiros

```text
┌─────────────────────────────────────────────────────────────────────────────────┐
│                        KNOWLEDGE BASE  (ESTADO PARTILHADO)                      │
│          Camada Central de Governação, Padrões e Alinhamento — JAPLearning      │
└────────────────────────────────────┬────────────────────────────────────────────┘
                                     │
          ┌──────────────────────────┼────────────────────────────────┐
          │                          │                                │
          ▼                          ▼                                ▼
┌─────────────────┐       ┌──────────────────────┐         ┌──────────────────────┐
│  1. OPERACIONAL │       │  2. TÉCNICO          │         │  3. PRODUTO & DESIGN │
│  (Ponto de      │       │  (Arquitectura e BD) │         │  (Spec e UI/UX)      │
│   Entrada)      │       │                      │         │                      │
└────────┬────────┘       └──────────┬───────────┘         └──────────┬───────────┘
         │                           │                                │
         ▼                           ├──────────────┐                 ├──────────────┐
  ┌─────────────┐                    ▼              ▼                 ▼              ▼
  │  CLAUDE.md  │           ┌──────────────┐ ┌──────────┐    ┌──────────────┐ ┌──────────┐
  ├─────────────┤           │ARCHITECTURE  │ │DATABASE/ │    │   SPEC/      │ │DESIGN.md │
  │ - Navegação │           │    .md       │ ├──────────┤    ├──────────────┤ ├──────────┤
  │   rápida    │           ├──────────────┤ │00-conven │    │ 00-overview  │ │ - Tokens │
  │ - Comandos  │           │ - Camadas    │ │01-params │    │ 01-learning  │ │   CSS    │
  │ - Gotchas   │           │ - Padrões    │ │02-entit  │    │ 02-student   │ │ - Compo- │
  │ - Estado    │           │ - DI e       │ │03-relat  │    │ 03-users     │ │  nentes  │
  │   actual    │           │   Patterns   │ │04-auxil  │    │ 04-content   │ │ - UX     │
  └──────┬──────┘           │ - CRUD novo  │ └──────────┘    │ 05-params    │ │  Rules   │
         │                  │   (10 pass)  │                  │ 06-admin     │ └──────────┘
         │                  └──────────────┘                  └──────────────┘
         │
         ▼
  ┌─────────────┐
  │CHANGELOG.md │
  ├─────────────┤
  │ - Histórico │
  │   sessão    │
  │ - Ficheiros │
  │   alterados │
  │ - Pendentes │
  │ - Template  │
  └─────────────┘
```

---

## 📋 Responsabilidade de Cada Ficheiro

| Ficheiro | Quando Ler | Quando Actualizar |
|----------|-----------|------------------|
| `CLAUDE.md` | **Sempre** — lido automaticamente | Quando mudam gotchas, estado ou estrutura |
| `ARCHITECTURE.md` | Nova funcionalidade ou refactor | Quando se adicionam padrões ou camadas |
| `SPEC/` | Implementar módulo ou regra de negócio | Quando se adicionam módulos ou fluxos |
| `DESIGN.md` | Criar ou alterar componentes de UI | Quando se adicionam componentes CSS |
| `DATABASE/` | Criar tabelas, queries ou migrações | Quando há ALTER TABLE ou nova migração |
| `CHANGELOG.md` | Ver o que foi feito na sessão anterior | **No fim de cada sessão** |

### Guia rápido SPEC/

| Ficheiro | Ler quando... |
|----------|--------------|
| `SPEC/00-overview.md` | Dúvidas sobre visão, actores ou integrações |
| `SPEC/01-learning-core.md` | Trabalhar em Formações, Lições, Questões |
| `SPEC/02-student-area.md` | Trabalhar no Player, Quiz ou Certificados |
| `SPEC/03-users.md` | Trabalhar em autenticação ou utilizadores |
| `SPEC/04-content.md` | Trabalhar em Testemunhos ou Artigos |
| `SPEC/05-parameters.md` | Trabalhar em Equipas, Categorias, Níveis... |
| `SPEC/06-admin.md` | Trabalhar em relatórios ou dashboard admin |

### Guia rápido DATABASE/

| Ficheiro | Ler quando... |
|----------|--------------|
| `DATABASE/00-conventions.md` | Migrações, queries de diagnóstico, soft delete |
| `DATABASE/01-parameters.md` | Schema das tabelas P_ |
| `DATABASE/02-entities.md` | Schema das tabelas E_ |
| `DATABASE/03-relationships.md` | Schema das tabelas R_ |
| `DATABASE/04-auxiliaries.md` | Schema de AppVersions e AuditLogs |

---

## 🔄 Fluxo de uma Nova Sessão Claude Code

```text
  INÍCIO DE SESSÃO
        │
        ▼
  ┌─────────────────────────────────┐
  │  Claude lê CLAUDE.md            │  ← automático
  │  (navegação + gotchas + estado) │
  └────────────────┬────────────────┘
                   │
        ┌──────────▼──────────┐
        │  Dev pede:          │
        │  "Lê o CHANGELOG    │
        │   e diz onde        │
        │   ficámos"          │
        └──────────┬──────────┘
                   │
        ┌──────────▼───────────────────────────────┐
        │  Claude lê CHANGELOG.md                   │
        │  → recupera estado da última sessão        │
        └──────────┬───────────────────────────────┘
                   │
       ┌───────────┴──────────────┐
       │  Conforme a tarefa:      │
       ▼                          ▼
  Nova feature              Bug / UI fix
       │                          │
       ▼                          ▼
  Ler SPEC/              Ler DESIGN.md
  + ARCHITECTURE.md      + DATABASE/
       │                          │
       └───────────┬──────────────┘
                   │
                   ▼
            Implementar
                   │
                   ▼
        ┌──────────────────────┐
        │  Actualizar          │
        │  CHANGELOG.md        │  ← fim de sessão
        └──────────────────────┘
```

---

## 🧱 Stack e Contexto Técnico

```text
  ┌────────────────────────────────────────────────────────────┐
  │  STACK                                                     │
  │  ASP.NET Core MVC (.NET 10)  +  SQL Server  +  Cloudinary  │
  ├────────────────────────────────────────────────────────────┤
  │  CAMADAS                                                   │
  │  Mvc  →  Business  ←  Data                                 │
  │             ↑                                              │
  │          Helper                                            │
  ├────────────────────────────────────────────────────────────┤
  │  PREFIXOS DE TABELAS                                       │
  │  E_  Entidades   │  P_  Parâmetros                         │
  │  R_  Relações    │  A_  Auxiliares                         │
  ├────────────────────────────────────────────────────────────┤
  │  PADRÕES OBRIGATÓRIOS                                      │
  │  • ModelState.Remove(PhotoUrl) antes de IsValid            │
  │  • md-alert md-alert-danger (nunca validation-summary)     │
  │  • form-card style="max-width:100%" em layouts horizontais │
  │  • AddErrors() após UpdateAsync/AddAsync retornar false    │
  └────────────────────────────────────────────────────────────┘
```

---

## ⚠️ Anti-Patterns — Nunca Fazer

```text
  ╔══════════════════════════════════════════════════════════════╗
  ║  ✗  class="validation-summary"   →  classe não existe no CSS ║
  ║  ✗  Omitir ModelState.Remove     →  form falha silenciosamente║
  ║  ✗  form-card sem max-width:100% →  layout parte em mobile   ║
  ║  ✗  ALTER TABLE sem documentar   →  "column not found" em prod║
  ║  ✗  Commit() sem verificar false →  dados não gravados        ║
  ║  ✗  Repository.Update() em login →  sobrescreve todos os camp ║
  ╚══════════════════════════════════════════════════════════════╝
```
