# SPEC.md — JAPLearning

Especificação de produto: módulos, regras de negócio e fluxos da plataforma.

---

## Visão do Produto

**JAPLearning** é uma plataforma de e-learning interna para a equipa **DMC-Developers**. Permite criar e gerir formações estruturadas com vídeo e testes de conhecimento, acompanhar o progresso dos formandos e emitir certificados automáticos.

---

## Actores / Roles

| Role | Descrição | Acesso |
|------|-----------|--------|
| **Administrador** | Gestão total da plataforma | Tudo |
| **Supervisor** | Gestão de conteúdo e formandos | CRUD de módulos, sem gestão de roles/permissões |
| **Formando** | Consumo das formações | Área do aluno (player, certificados, perfil) |

---

## Módulos da Plataforma

### 1. Equipas (`P_Teams`)
Agrupam formações. Cada formação pertence a uma equipa.
- Campos: Nome, Descrição, IsActived

### 2. Categorias (`P_Categories`)
Classificação de formações.
- Campos: Nome, Descrição, IsActived

### 3. Formações / Cursos (`E_Courses`)
Unidade principal de aprendizagem.
- Campos: Título, Descrição, Imagem de Capa, Nível, Categoria, Equipa, PassingScore (int, default 60), IsActived
- Uma formação tem vários **Tópicos**
- `PassingScore` define a nota mínima (%) para obter certificado

### 4. Tópicos (`P_Topics`)
Agrupam lições dentro de uma formação. Ordenados por `DisplayOrder`.

### 5. Lições (`E_Lessons`)
Unidade de conteúdo. Dois tipos:
- **Lição de Vídeo** → tem URL de vídeo (YouTube/Vimeo embed); botão "Concluir Aula"
- **Lição de Quiz** → tem questões com opções; sem vídeo; quiz interactivo
- **Detecção automática:** `IsQuizLesson = Questions.Any(q => q.Options.Any())`
- Campos: Título, Descrição, VideoUrl, DisplayOrder, IsActived

### 6. Questões (`E_Questions` / `E_Lessons`)
Perguntas de um quiz (ligadas a uma lição).
- Campos: QuestionText (textarea), Description/Explanation, LessonId, IsActived

### 7. Opções de Resposta (`E_QuestionOptions`)
Respostas possíveis de uma questão.
- Campos: Name (texto da opção), IsCorrect, Description (explicação pós-resposta), QuestionId, IsActived
- Selects em cascata na UI: Equipa → Formação → Tópico → Lição → Questão

### 8. Utilizadores (`E_Users`)
Formandos e administradores da plataforma.
- Integrado com ASP.NET Core Identity
- Campos extra: NomeCompleto, Foto (Cloudinary), Equipa, Role, IsActived

### 9. Testemunhos (`E_Testimonials`)
Depoimentos de formandos exibidos na landing page.
- Campos: AuthorName, Role, City, Country, PhotoUrl (Cloudinary), Quote, Rating (1-5), LinkedinUrl, Featured, DisplayOrder, IsActived
- Foto: upload opcional via Cloudinary; Edit mantém foto existente se não for substituída

### 10. Artigos (`E_Articles`)
Blog/conteúdo informativo da plataforma.
- Campos: Title, Slug, Content, CoverImage, Subject, IsActived

### 11. Certificados (`E_Certificates`)
Emitidos automaticamente ao concluir uma formação com sucesso.
- Campos: UserId, CourseId, CompletedDate, ValidationCode (12 chars alfanumérico), CertifiedFile, ScorePercent (int)
- Visualização: página standalone imprimível (`Student/CertificateView.cshtml`)

---

## Fluxos Principais

### Fluxo do Formando — Consumir Formação

```
Dashboard → Seleccionar Formação → Player

No Player:
├── Lição de Vídeo
│   ├── Mostra iframe do vídeo
│   ├── Mostra conteúdo/descrição da lição
│   └── Botão "Concluir Aula" → POST CompleteLesson → verifica certificado
│
└── Lição de Quiz
    ├── SEM área de vídeo
    ├── Badge de estado: "Teste a Fazer" / "Tentativa: X%" / "Teste Concluído"
    ├── Quiz interactivo (questões com opções A/B/C...)
    ├── Resultado: percentagem de acertos vs PassingScore
    ├── AJAX SaveQuizResult → grava em R_UserLessonTests
    └── Botão "Concluir" (após quiz) → POST CompleteLesson → verifica certificado
```

### Fluxo de Emissão de Certificado

Disparado em `StudentController.CompleteLesson`:

```
1. Marcar lição como concluída em R_UserCourseLessons
2. Verificar se TODAS as lições da formação estão concluídas
   └─ Se não → terminar (sem certificado)
3. Verificar se já existe certificado para este utilizador/formação
   └─ Se sim → terminar (certificado já emitido)
4. Calcular nota média:
   a. Para cada lição com quiz: obter MELHOR resultado em R_UserLessonTests
   b. Lições sem quiz: contam como 100%
   c. Média = soma das melhores notas / total de lições
5. Verificar se média ≥ Course.PassingScore
   └─ Se não → sem certificado (notificar formando)
6. Emitir certificado:
   - IssueCertificateAsync(userId, courseId, scorePercent)
   - Grava em E_Certificates com ValidationCode aleatório (12 chars)
```

### Fluxo de Quiz (JavaScript no Player)

```
selectOption(questionIndex, optionIndex)
    └─ marca opção visualmente, activa botão "Próxima" / "Terminar"

nextQuestion()
    └─ avança para próxima questão (com animação de transição)

finishQuiz()
    └─ calcula % de respostas correctas
    └─ AJAX POST → /Student/SaveQuizResult (JSON, sem antiforgery)
    └─ mostra ecrã de resultado:
        ├─ ≥ PassingScore → "Parabéns!" (verde) + botão "Concluir"
        └─ < PassingScore → "Continua a tentar!" (laranja) + botão "Tentar Novamente"

retryQuiz()
    └─ reinicia quiz (volta à questão 1, limpa selecções)

completeFromQuiz()
    └─ POST para CompleteLesson → desencadeia lógica de certificado
```

### Gestão de Testemunhos

```
Index → lista todos → botões Editar / Eliminar
Create → formulário horizontal → upload foto (opcional) → Cloudinary
Edit   → formulário horizontal → foto existente em preview
         → nova foto substitui (apaga antiga do Cloudinary)
         → sem nova foto → mantém PhotoUrl existente
```

---

## Regras de Negócio

| Regra | Detalhe |
|-------|---------|
| Certificado só se todas as lições concluídas | Verificado em `CompleteLesson` |
| Certificado só se média ≥ `PassingScore` | Default: 60% |
| Nota do quiz: melhor tentativa por lição | Não a média de tentativas |
| Lições sem quiz contam como 100% para a média | Garante que só a componente de quiz é avaliada |
| Um certificado por utilizador por formação | `HasCertificateAsync` verifica antes de emitir |
| ValidationCode único de 12 chars | `Guid.NewGuid().ToString("N").ToUpper()[..12]` |
| Foto de testemunho: opcional | `PhotoUrl` pode ser `""` (sem foto) |
| Rating de testemunho: 1 a 5 | Sem validação server-side estrita (apenas UI min/max) |
| `IsDeleted` (soft delete) | Presente em entidades principais; não implementado na UI ainda |

---

## Páginas da Área do Formando (`StudentController`)

| Rota | View | Descrição |
|------|------|-----------|
| `/Student/Dashboard` | `Dashboard.cshtml` | Visão geral das formações inscritas |
| `/Student/Player/{lessonId}` | `Player.cshtml` | Player de lição (vídeo ou quiz) |
| `/Student/MyCertificates` | `MyCertificates.cshtml` | Lista de certificados obtidos |
| `/Student/CertificateView/{id}` | `CertificateView.cshtml` | Certificado imprimível (sem layout) |
| `/Student/CompleteLesson` | — | POST: marcar lição como concluída |
| `/Student/SaveQuizResult` | — | POST JSON: gravar resultado do quiz |

---

## Páginas de Administração

| Módulo | Controller | Views |
|--------|-----------|-------|
| Equipas | `TeamsController` | Index, Create, Edit |
| Categorias | `CategoriesController` | Index, Create, Edit |
| Formações | `CoursesController` | Index, Create, Edit, Details |
| Tópicos | `TopicsController` | Index, Create, Edit |
| Lições | `LessonsController` | Index, Create, Edit |
| Questões | `QuestionsController` | Index, Create, Edit |
| Opções de Resposta | `QuestionOptionsController` | Index, Create, Edit |
| Utilizadores | `UsersController` | Index, Create, Edit |
| Testemunhos | `TestimonialsController` | Index, Create, Edit |
| Artigos | `ArticlesController` | Index, Create, Edit |
| Certificados | `CertificatesController` | Index, Details |
| Roles | `RolesController` | Index, Create, Edit |

---

## Dados de Referência (Seeders)

A plataforma inclui seeders para dados iniciais:
- Roles de sistema (Administrador, Supervisor, Formando)
- Utilizador administrador padrão
- Níveis de dificuldade (Iniciante, Intermédio, Avançado)

---

## Integrações Externas

| Serviço | Uso | Interface |
|---------|-----|-----------|
| **Cloudinary** | Upload/gestão de imagens (fotos de utilizadores, testemunhos, capas de artigos) | `ICloudinaryService` |
| **Email** | Notificações (não implementado na UI ainda) | `IEmailService` |
| **Doppler** | Gestão de secrets/configurações em produção | `Doppler.Extensions.Configuration` |
