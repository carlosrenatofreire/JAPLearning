# SPEC/05-parameters.md — Parâmetros e Configuração

Módulos de lookup/configuração: Equipas, Categorias, Níveis, Professores, Assuntos, Roles.

---

## 1. Equipas (`P_Teams`)

Agrupam formações. Cada formação pertence a uma equipa.
- Campos: Nome, Descrição, IsActived

---

## 2. Categorias (`P_Categories`)

Classificação de formações. Cada categoria pertence a uma equipa.
- Campos: Nome, Subtítulo, Descrição, TeamId, IsActived

---

## Níveis (`P_Levels`)

Dificuldade das formações.
- Valores de seed: Iniciante, Intermédio, Avançado

---

## Professores / Formadores (`P_Teachers`)

Autor/responsável por uma formação.
- Campos: Nome, Descrição, PhotoUrl (Cloudinary), IsActived

---

## Assuntos (`P_Subjects`)

Classificação temática para Artigos.
- Campos: Nome, IsActived

---

## Roles (`P_Roles`)

Perfis de acesso dos utilizadores.
- Valores fixos: Administrador, Supervisor, Formando
- Geridos via `RolesController` (apenas Administrador)
