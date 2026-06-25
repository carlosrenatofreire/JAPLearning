# SPEC/03-users.md — Utilizadores e Autenticação

---

## 8. Utilizadores (`E_Users`)

Formandos e administradores da plataforma.
- Integrado com ASP.NET Core Identity
- Campos extra: NomeCompleto, Foto (Cloudinary), Equipa, Role, IsActived
- `LoginCount` + `LastLoginDate` — registados a cada login bem sucedido (apenas Alunos)
- `MustChangePassword` — `true` em novos utilizadores; obriga troca de senha antes de aceder ao Dashboard

---

## Fluxo de Autenticação

```
Login
├── Aluno com MustChangePassword = true
│   └─ Redirect → /Account/ChangePassword (logo sem link, sem sidebar)
│       └─ POST ChangePassword → MustChangePassword = false → Redirect Dashboard
│
└── Utilizador normal → Redirect Dashboard / área admin
```

---

## Login Tracking

- `RecordLoginAsync` usa `ExecuteUpdateAsync` (cirúrgico — não faz UPDATE de todos os campos)
- Actualiza apenas `LoginCount + 1` e `LastLoginDate = DateTime.UtcNow`
- Apenas aplicado a utilizadores com Role = Formando

---

## Roles Disponíveis

```csharp
[Authorize(Roles = "Administrador,Supervisor")]  // acesso de gestão
[Authorize(Roles = "Formando")]                  // área do aluno
[Authorize]                                       // qualquer utilizador autenticado
```
