# CLAUDE.md — JAPLearning

Guia operacional para sessões Claude Code. Lido automaticamente no início de cada sessão.
Para detalhes técnicos ver [`ARCHITECTURE.md`](./ARCHITECTURE.md).
Para regras de negócio e módulos ver [`SPEC.md`](./SPEC.md).

---

## O Projecto

**JAPLearning** — plataforma de e-learning para a equipa **DMC-Developers**.
- Stack: **ASP.NET Core MVC (.NET 10)** + **SQL Server** + **Cloudinary** (imagens)
- Idioma da UI: **Português (Portugal)**
- Pasta raiz: `C:\JAPLearning\src\`
- Solução: `JAPLearning.sln` (5 projectos: Business, Data, Helper, Mvc, Tests)

---

## Navegação Rápida

| O que procuras | Onde está |
|---|---|
| Entidades de domínio | `Business/Models/Domains/Entities/` |
| Interfaces de serviço | `Business/Interfaces/Services/` |
| Implementações de serviço | `Business/Services/Entities/` (ou Parameters/, Relationships/) |
| Mapeamentos EF (Fluent API) | `Data/Mappings/Entities/` |
| Repositórios | `Data/Repositories/Entities/` |
| Controllers | `Mvc/Controllers/` |
| ViewModels | `Mvc/ViewModels/Entities/` (ou Parameters/, Student/, Account/) |
| Views | `Mvc/Views/[NomeController]/` |
| CSS partilhado CRUD | `Mvc/Views/Shared/_CrudStyles.cshtml` |
| Layout principal | `Mvc/Views/Shared/_LayoutApp.cshtml` |
| Registo de DI | `Mvc/Configurations/DependencyInjectionConfig.cs` |
| AutoMapper | `Mvc/Configurations/AutoMapperConfig.cs` |

---

## Comandos

```bash
# Correr a aplicação
dotnet run --project src/JAPLearning.Mvc

# Compilar tudo
dotnet build src/JAPLearning.sln

# Nova migração EF
dotnet ef migrations add NomeMigracao --project src/JAPLearning.Data --startup-project src/JAPLearning.Mvc

# Aplicar migrações
dotnet ef database update --project src/JAPLearning.Data --startup-project src/JAPLearning.Mvc
```

---

## Armadilhas Conhecidas (Gotchas)

### 1. `ModelState.Remove` obrigatório para campos de URL/ficheiro
Com `<Nullable>enable</Nullable>`, strings não-nullable têm `[Required]` implícito.
Campos como `PhotoUrl` que não vêm directamente do POST (ou chegam como `""`) falham validação silenciosamente — o form volta para a mesma página sem mensagem.
**Sempre adicionar antes de `ModelState.IsValid`:**
```csharp
ModelState.Remove(nameof(vm.PhotoUrl));
if (!ModelState.IsValid) return View(vm);
```

### 2. Validation summary — usar a classe correcta
Usar sempre `class="md-alert md-alert-danger mb-3"`, **nunca** `class="validation-summary"` (não existe no CSS).
```html
<div asp-validation-summary="ModelOnly" class="md-alert md-alert-danger mb-3"></div>
```

### 3. Form card width — override obrigatório
`.form-card` tem `max-width: 640px` por defeito no `_CrudStyles.cshtml`.
Para formulários com layout horizontal (múltiplas colunas), adicionar sempre:
```html
<div class="md-card form-card" style="max-width:100%;">
```

### 4. Coluna `ScorePercent` adicionada manualmente (sem migração)
```sql
-- Verificar e adicionar se não existir:
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('E_Certificates') AND name = 'ScorePercent')
    ALTER TABLE E_Certificates ADD ScorePercent int NOT NULL DEFAULT 0;
```

### 5. `UnitOfWork.Commit()` retorna `false` (não lança excepção)
`SaveChangesAsync() > 0` retorna `false` se 0 linhas forem gravadas.
O controller deve verificar e chamar `AddErrors()` para propagar mensagens ao utilizador.

---

## Estado Actual

### Concluído (sessões anteriores)
- Sistema completo de quiz no Player (JS interactivo + AJAX + gravação em `R_UserLessonTests`)
- Emissão automática de certificados (lógica: todas lições concluídas + média ≥ `PassingScore`)
- Certificado imprimível standalone (`Student/CertificateView.cshtml`, `Layout=null`)
- Detalhes do certificado com iframe preview (`Certificates/Details.cshtml`)
- Layout horizontal em formulários: Questions, QuestionOptions, Testimonials
- Bug corrigido: Testimonials Edit POST (`ModelState.Remove` + `validation-summary`)

### Pendente / Próximas Tarefas
- Confirmar coluna `ScorePercent` aplicada na BD de produção
- Testar fluxo completo: quiz → conclusão → emissão de certificado
- Dashboard de progresso por equipa (relatórios)
- Notificação por email ao emitir certificado
