# DATABASE/01-parameters.md — Tabelas de Parâmetros (P_)

Tabelas de configuração e lookup. Prefixo `P_`.

---

## P_Teams

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| Name | nvarchar | ✓ | |
| Description | nvarchar | | |
| Thumbnail | nvarchar | | URL Cloudinary |
| IsActived | bit | ✓ | default true |
| IsDeleted | bit | ✓ | soft delete |

---

## P_Categories

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| TeamId | uniqueidentifier | ✓ | FK → P_Teams |
| Name | nvarchar | ✓ | |
| Subtitle | nvarchar | | |
| Description | nvarchar | | |
| IsActived | bit | ✓ | default true |
| IsDeleted | bit | ✓ | soft delete |

---

## P_Levels

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| Name | nvarchar | ✓ | ex: Iniciante, Intermédio, Avançado |
| Description | nvarchar | | |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

---

## P_Teachers

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| Name | nvarchar | ✓ | |
| Description | nvarchar | | |
| PhotoUrl | nvarchar | | URL Cloudinary |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

---

## P_Roles

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| Name | nvarchar | ✓ | Administrador / Supervisor / Formando |
| Description | nvarchar | | |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

---

## P_Subjects

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| Name | nvarchar | ✓ | Assunto para artigos |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |
