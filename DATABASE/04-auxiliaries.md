# DATABASE/04-auxiliaries.md — Tabelas Auxiliares (A_ / E_ de suporte)

Logs, versões e tabelas de apoio ao sistema.

---

## E_AppVersions — Versões/Releases da aplicação

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| VersionNumber | varchar(20) | ✓ | ex: "0.18" |
| Title | varchar(150) | ✓ | ex: "Junho 2026" |
| ReleaseDate | datetime2 | ✓ | data do lançamento |
| IsPublished | bit | ✓ | false = rascunho; true = visível no Changelog |

---

## E_AppVersionItems — Itens de cada versão

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| VersionId | uniqueidentifier | ✓ | FK → E_AppVersions |
| Type | int | ✓ | 1=Feature (Novo), 2=Improvement (Melhoria), 3=Fix (Correcção) |
| Description | varchar(500) | ✓ | texto do item |
| Order | int | ✓ | ordenação dentro da versão; default 0 |

---

## A_AuditLogs

| Coluna | Tipo | Notas |
|--------|------|-------|
| Id | uniqueidentifier | PK |
| UserId | uniqueidentifier | quem fez a acção |
| Action | varchar(50) | ex: Create, Update, Delete — ⚠️ adicionada manualmente (Sessão 7) |
| EntityName | varchar(100) | nome da entidade afectada — ⚠️ adicionada manualmente (Sessão 7) |
| Entity | nvarchar | nome da entidade (campo original) |
| EntityId | nvarchar | ID da entidade afectada |
| Details | nvarchar | informação adicional / StackTrace |
| HttpStatusCode | int | código HTTP associado — ⚠️ adicionada manualmente (Sessão 7) |
| CreatedDate | datetime2 | quando ocorreu |
