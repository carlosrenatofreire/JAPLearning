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
  ├─────────────┤           │ARCHITECTURE  │ │DATABASE  │    │   SPEC.md    │ │DESIGN.md │
  │ - Navegação │           │    .md       │ │   .md    │    ├──────────────┤ ├──────────┤
  │   rápida    │           ├──────────────┤ ├──────────┤    │ - Módulos    │ │ - Tokens │
  │ - Comandos  │           │ - Camadas    │ │ - Schema │    │ - Regras de  │ │   CSS    │
  │ - Gotchas   │           │ - Padrões    │ │   BD     │    │   negócio    │ │ - Compo- │
  │ - Estado    │           │ - DI e DI    │ │ - Tabelas│    │ - Fluxos     │ │  nentes  │
  │   actual    │           │   Patterns   │ │  E/P/R/A │    │ - Actores    │ │ - UX     │
  └──────┬──────┘           │ - CRUD novo  │ │ - Queries│    │ - Rotas      │ │  Rules   │
         │                  │   (10 pass)  │ │  SQL     │    └──────────────┘ └──────────┘
         │                  └──────────────┘ └──────────┘
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
| `SPEC.md` | Implementar módulo ou regra de negócio | Quando se adicionam módulos ou fluxos |
| `DESIGN.md` | Criar ou alterar componentes de UI | Quando se adicionam componentes CSS |
| `DATABASE.md` | Criar tabelas, queries ou migrações | Quando há ALTER TABLE ou nova migração |
| `CHANGELOG.md` | Ver o que foi feito na sessão anterior | **No fim de cada sessão** |

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
  Ler SPEC.md              Ler DESIGN.md
  + ARCHITECTURE.md        + DATABASE.md
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
  ╚══════════════════════════════════════════════════════════════╝
```
