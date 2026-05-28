# ARCHITECTURE.md — JAPLearning

Decisões técnicas, padrões de código e convenções da plataforma.

---

## Visão Geral da Arquitectura

Arquitectura em camadas (**Clean-ish / Layered**), sem DDD estrito:

```
┌─────────────────────────────┐
│       JAPLearning.Mvc       │  ← UI: Controllers, Views, ViewModels
└─────────────┬───────────────┘
              │ referencia
┌─────────────▼───────────────┐
│    JAPLearning.Business     │  ← Regras de negócio, Interfaces, Serviços, Validações
└─────────────┬───────────────┘
              │ referencia
┌─────────────▼───────────────┐
│      JAPLearning.Data       │  ← EF Core, Repositórios, Mapeamentos, Migrações
└─────────────────────────────┘
              ↑
┌─────────────┴───────────────┐
│     JAPLearning.Helper      │  ← Utilitários partilhados (sem dependências internas)
└─────────────────────────────┘
```

**Regra de dependências:** Mvc → Business ← Data. Data nunca referencia Mvc.

---

## Estrutura de Pastas

### JAPLearning.Business

```
Business/
├── Interfaces/
│   ├── Externals/              ← ICloudinaryService, IEmailService
│   ├── Internals/
│   │   ├── Entities/           ← ITestimonialRepository, ICertificateRepository, ...
│   │   ├── Parameters/         ← ITeamRepository, ICategoryRepository, ...
│   │   ├── Relationships/      ← IUserCourseLessonRepository, IUserLessonTestRepository, ...
│   │   ├── Auxiliaries/        ← IAuditLogRepository
│   │   └── Shareds/            ← IRepository<T>, IUnitOfWork, INotificator
│   └── Services/
│       ├── Entities/           ← ITestimonialService, ICertificateService, IUserService, ...
│       ├── Parameters/         ← ITeamService, ICategoryService, ...
│       ├── Relationships/      ← IUserCourseLessonService, IUserLessonTestService, ...
│       └── Auxiliaries/        ← IAuditLogService
├── Models/
│   ├── Domains/
│   │   ├── Entities/           ← Course, Lesson, Question, QuestionOption,
│   │   │                          Certificate, Testimonial, Article, User
│   │   ├── Parameters/         ← Team, Category, Level, Topic, Teacher, Subject, ...
│   │   ├── Relationships/      ← UserCourseLesson, UserLessonTest, UserLessonQuestion,
│   │   │                          CourseRequirement
│   │   └── Auxiliaries/        ← AuditLog
│   ├── Enums/
│   ├── Settings/               ← CloudinarySettings, etc.
│   └── Shareds/
│       └── Entity.cs           ← base com `Guid Id`
├── Services/
│   ├── BaseService.cs          ← CRUD genérico (ver abaixo)
│   ├── Entities/
│   ├── Parameters/
│   ├── Relationships/
│   └── Internals/              ← serviços internos (middlewares, etc.)
├── Validators/                 ← FluentValidation (CategoryValidator, CourseValidator, ...)
│                                  NÃO existe TestimonialValidator
└── Notifications/
    └── Notificator.cs          ← padrão de notificação de erros de domínio
```

### JAPLearning.Data

```
Data/
├── Contexts/
│   └── MainDbContext.cs        ← DbContext; aplica todos os Mappings via reflection
├── Mappings/                   ← Fluent API (IEntityTypeConfiguration<T>)
│   ├── Entities/               ← CertificateMapping, TestimonialMapping, ...
│   ├── Parameters/
│   ├── Relationships/
│   └── Auxiliaries/
├── Migrations/                 ← geradas por EF (não editar manualmente)
├── Repositories/
│   ├── Shareds/
│   │   ├── Repository.cs       ← base genérico (ver abaixo)
│   │   └── UnitOfWork.cs
│   ├── Entities/
│   ├── Parameters/
│   ├── Relationships/
│   └── Auxiliaries/
└── Seeders/                    ← dados iniciais
```

### JAPLearning.Mvc

```
Mvc/
├── Configurations/
│   ├── AutoMapperConfig.cs         ← todos os CreateMap<Entity, ViewModel>().ReverseMap()
│   ├── DependencyInjectionConfig.cs← AddScoped para todos os serviços e repositórios
│   └── AuthConfiguration.cs
├── Controllers/
│   ├── BaseController.cs           ← AddErrors(), IsOperationValid()
│   ├── StudentController.cs        ← player, quiz, certificados do formando
│   └── [Módulo]Controller.cs       ← um por entidade
├── ViewModels/
│   ├── Account/                    ← LoginViewModel, RegisterViewModel
│   ├── Entities/                   ← [Entidade]ViewModel
│   ├── Parameters/                 ← [Parâmetro]ViewModel
│   └── Student/                    ← PlayerViewModel, DashboardViewModel
├── Views/
│   ├── Shared/
│   │   ├── _LayoutApp.cshtml       ← layout autenticado (sidebar, topbar, overlay loading)
│   │   ├── _LayoutPublic.cshtml    ← layout público
│   │   └── _CrudStyles.cshtml      ← CSS partilhado + classes utilitárias
│   └── [Módulo]/
│       ├── Index.cshtml
│       ├── Create.cshtml
│       ├── Edit.cshtml
│       └── Details.cshtml
└── wwwroot/
    ├── css/                        ← estilos globais customizados
    ├── js/
    └── lib/                        ← bootstrap 5, jquery, jquery-validation, jquery-validation-unobtrusive
```

---

## Convenções de Base de Dados

### Prefixos de Tabelas

| Prefixo | Tipo | Exemplos |
|---------|------|---------|
| `E_` | Entidades principais | `E_Courses`, `E_Lessons`, `E_Certificates`, `E_Testimonials`, `E_Users` |
| `P_` | Parâmetros / configuração | `P_Teams`, `P_Categories`, `P_Levels`, `P_Topics`, `P_Teachers` |
| `R_` | Relacionamentos | `R_UserCourseLessons`, `R_UserLessonTests`, `R_UserLessonQuestions` |
| `A_` | Auxiliares | `A_AuditLogs` |

### Campos Comuns nas Entidades

```csharp
public class Entity          // base em Business/Models/Shareds/Entity.cs
{
    public Guid Id { get; set; }
}

// Padrão em entidades principais:
public bool IsActived { get; set; } = true;
public bool IsDeleted { get; set; }
public DateTime CreatedDate { get; set; }
public DateTime? ChangedDate { get; set; }
```

---

## Padrões de Implementação

### BaseService (Business/Services/BaseService.cs)

```csharp
public abstract class BaseService<TEntity, TRepository> : IBaseService<TEntity>
{
    // Disponível em todos os serviços:
    Task<List<TEntity>> GetAllAsync()
    Task<TEntity?> GetByIdAsync(Guid id)
    Task<bool> AddAsync(TEntity entity)      // retorna false se Commit() == 0
    Task<bool> UpdateAsync(TEntity entity)   // idem
    Task<bool> DeleteAsync(Guid id)          // idem

    // Para validação FluentValidation:
    protected bool Validate<TV>(TV validator, TEntity entity)
        where TV : AbstractValidator<TEntity>
}
```

### Repository Base (Data/Repositories/Shareds/Repository.cs)

```csharp
// Operações base disponíveis:
Task Add(TEntity entity)        // DbSet.Add
Task Update(TEntity entity)     // DbSet.Update (marca TODOS os campos como Modified)
Task Remove(Guid id)            // DbSet.Remove com stub (só Id)
Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)  // AsNoTracking
Task<TEntity> GetById(Guid id)  // DbSet.FindAsync
Task<List<TEntity>> GetAll()    // ToListAsync (com tracking)
```

**Nota:** `Find()` usa `AsNoTracking()`. `GetAll()` e `GetById()` usam tracking.

### UnitOfWork

```csharp
public async Task<bool> Commit()
    => await _context.SaveChangesAsync() > 0;
// Retorna false (não lança excepção) se 0 linhas gravadas.
```

### BaseController (Mvc/Controllers/BaseController.cs)

```csharp
protected void AddErrors()
    // Lê INotificator.GetNotifications() e adiciona ao ModelState como erros globais

protected bool IsOperationValid()
    => !_notificator.HasNotifications;
```

### AutoMapper

Todos os mapeamentos em `AutoMapperConfig.cs`. Padrão:
```csharp
cfg.CreateMap<Entidade, EntidadeViewModel>().ReverseMap()
   .ForMember(d => d.NavegacaoNavigation, o => o.Ignore());
```
`ReverseMap()` gera mapeamento bidirecional. Propriedades de navegação EF são sempre ignoradas no sentido ViewModel → Entity.

---

## Padrões de Views e Formulários

### Validation Summary — padrão obrigatório

```html
<!-- CORRECTO -->
<div asp-validation-summary="ModelOnly" class="md-alert md-alert-danger mb-3"></div>

<!-- INCORRECTO — classe não existe no CSS -->
<div asp-validation-summary="All" class="validation-summary"></div>
```

### Form Card — width

```html
<!-- .form-card tem max-width:640px por defeito — SEMPRE override para layouts horizontais -->
<div class="md-card form-card" style="max-width:100%;">
```

### Layout Horizontal de Campos

```html
<div class="row g-3">
    <div class="col-md-4">
        <label class="md-label">Campo A <span class="text-danger">*</span></label>
        <input asp-for="CampoA" class="md-input" />
        <span asp-validation-for="CampoA" class="text-danger" style="font-size:12px;"></span>
    </div>
    <div class="col-md-8">
        <label class="md-label">Campo B</label>
        <textarea asp-for="CampoB" class="md-input" rows="3"></textarea>
    </div>
</div>
```

### Classes CSS do Design System

```
Layout e contentor:
  .md-card            → cartão escuro com borda e border-radius
  .form-card          → variante para formulários (max-width:640px por defeito)
  .page-header        → cabeçalho de página com título e breadcrumb

Campos de formulário:
  .md-label           → label de campo
  .md-input           → input, textarea, select estilizado
  .md-select          → select com seta customizada
  .md-form-check      → wrapper para checkbox
  .md-form-row        → wrapper para campo (label + input)

Botões:
  .btn-md-primary     → laranja, preenchido (trigger do overlay de loading)
  .btn-md-ghost       → contorno, sem preenchimento
  .btn-md-danger      → vermelho, para acções destrutivas
  .btn-md-success     → verde

Feedback:
  .md-alert           → caixa de alerta base
  .md-alert-danger    → variante vermelha
  .md-badge           → badge inline
  .md-badge-info      → azul
  .md-badge-success   → verde
  .md-badge-warning   → amarelo/laranja

Variáveis CSS:
  --md-accent         → #E8501A (laranja principal)
  --md-border         → cor da borda
  --md-text-primary   → texto principal
  --md-text-muted     → texto secundário/cinza
  --md-radius         → border-radius padrão
```

### Overlay de Loading Global

Definido em `_LayoutApp.cshtml`. Auto-activa em qualquer `<form>` que contenha `<button class="btn-md-primary" type="submit">`. Mostra spinner "A guardar..." enquanto o POST processa. Não requer código nas views.

---

## Adição de Novo Módulo CRUD

Passos em ordem:

1. **Entidade** → `Business/Models/Domains/[pasta]/NomeEntidade.cs` (herda `Entity`)
2. **Interface Repositório** → `Business/Interfaces/Internals/[pasta]/INomeRepository.cs` (herda `IRepository<NomeEntidade>`)
3. **Interface Serviço** → `Business/Interfaces/Services/[pasta]/INomeService.cs` (herda `IBaseService<NomeEntidade>`)
4. **Mapeamento EF** → `Data/Mappings/[pasta]/NomeMapping.cs` (`IEntityTypeConfiguration<NomeEntidade>`, define nome da tabela com prefixo)
5. **Repositório** → `Data/Repositories/[pasta]/NomeRepository.cs` (herda `Repository<NomeEntidade>`)
6. **Serviço** → `Business/Services/[pasta]/NomeService.cs` (herda `BaseService<NomeEntidade, INomeRepository>`)
7. **ViewModel** → `Mvc/ViewModels/[pasta]/NomeViewModel.cs`
8. **AutoMapper** → adicionar `CreateMap` em `AutoMapperConfig.cs`
9. **Controller** → `Mvc/Controllers/NomeController.cs` (herda `BaseController`)
10. **Views** → `Mvc/Views/Nome/` (Index, Create, Edit, Details)
11. **DI** → registar em `DependencyInjectionConfig.cs`:
    ```csharp
    services.AddScoped<INomeRepository, NomeRepository>();
    services.AddScoped<INomeService, NomeService>();
    ```

---

## Configurações Especiais

### Nullable Reference Types

`<Nullable>enable</Nullable>` activo no Mvc.csproj. Implicação: propriedades `string` não-nullable em ViewModels recebem `[Required]` implícito. Campos que não são postados directamente (como `PhotoUrl` em uploads de ficheiro) devem ser removidos do ModelState antes da validação:

```csharp
ModelState.Remove(nameof(vm.PhotoUrl));
if (!ModelState.IsValid) return View(vm);
```

### Cloudinary

Configurado via `ICloudinaryService`. Métodos principais:
- `UploadImageAsync(IFormFile file, string folder)` → retorna URL pública
- `ExtractPublicId(string url)` → extrai public_id para deleção
- `DeleteImageAsync(string publicId)` → apaga imagem

### Autenticação / Roles

Roles disponíveis: `Administrador`, `Supervisor`, `Formando`.
```csharp
[Authorize(Roles = "Administrador,Supervisor")]  // acesso de gestão
[Authorize(Roles = "Formando")]                  // área do aluno
[Authorize]                                       // qualquer utilizador autenticado
```
