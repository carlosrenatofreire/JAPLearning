# DATABASE/02-entities.md — Tabelas de Entidades (E_)

Tabelas principais de domínio. Prefixo `E_`.

---

## E_Users

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| RoleId | uniqueidentifier | ✓ | FK → P_Roles |
| TeamId | uniqueidentifier | ✓ | FK → P_Teams |
| FirstName | nvarchar | ✓ | |
| LastName | nvarchar | ✓ | |
| Email | nvarchar | ✓ | |
| Password | nvarchar | ✓ | hash BCrypt |
| PhoneNumber | nvarchar | | |
| PhotoUrl | nvarchar | | URL Cloudinary |
| Address | nvarchar | | |
| Number | nvarchar | | número de porta |
| City | nvarchar | | |
| State | nvarchar | | |
| Country | nvarchar | | |
| ResetToken | nvarchar | | token de recuperação de senha |
| ResetTokenExpiry | datetime2 | | |
| CreatedDate | datetime2 | ✓ | |
| ChangedDate | datetime2 | | |
| IsActived | bit | ✓ | default true |
| IsDeleted | bit | ✓ | soft delete |
| LoginCount | int | ✓ | default 0 — contador de acessos (apenas Alunos) |
| LastLoginDate | datetime2 | | data/hora do último login |
| MustChangePassword | bit | ✓ | default false — true = obriga troca de senha no próximo login |

---

## E_Courses

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| CategoryId | uniqueidentifier | ✓ | FK → P_Categories |
| TeacherId | uniqueidentifier | ✓ | FK → P_Teachers |
| LevelId | uniqueidentifier | ✓ | FK → P_Levels |
| Title | varchar(150) | ✓ | |
| Subtitle | varchar(255) | | |
| Description | varchar(2000) | | |
| Thumbnail | varchar(500) | | URL Cloudinary |
| SnapshotUrl | varchar(500) | | caminho relativo: `snapshots/{courseId}.zip` — ficheiro local em `PrivateFiles/` |
| PdfFileUrl | varchar(500) | | caminho relativo: `pdfs/{courseId}.pdf` — ficheiro local em `PrivateFiles/` |
| PassingScore | int | ✓ | default 60 — limiar de aprovação (%) |
| IsBrief | bit | ✓ | formação "em breve" — alunos não acedem ao player |
| IsFree | bit | ✓ | acesso livre |
| CreatedDate | datetime2 | ✓ | |
| ChangedDate | datetime2 | | |
| IsActived | bit | ✓ | default true |
| IsDeleted | bit | ✓ | soft delete |

---

## E_Topics

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| CourseId | uniqueidentifier | ✓ | FK → E_Courses |
| Name | nvarchar | ✓ | |
| Description | nvarchar | | |
| Order | int | ✓ | ordenação dentro da formação |
| CreatedDate | datetime2 | ✓ | |
| ChangedDate | datetime2 | | |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

---

## E_Lessons

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| CourseId | uniqueidentifier | ✓ | FK → E_Courses |
| TopicId | uniqueidentifier | ✓ | FK → E_Topics |
| Order | int | ✓ | ordenação dentro do tópico |
| Name | nvarchar | ✓ | |
| Description | nvarchar | | |
| TimeLesson | time | | duração estimada |
| Video | nvarchar | | URL embed (YouTube/Vimeo) |
| IsTest | bit | ✓ | true = lição do tipo quiz |
| IsFreePreview | bit | ✓ | preview sem inscrição |
| CreatedDate | datetime2 | ✓ | |
| ChangedDate | datetime2 | | |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

> **Nota:** a detecção de "lição quiz" no código não usa `IsTest` mas sim
> `Questions.Any(q => q.Options.Any())` — ver `PlayerViewModel.IsQuizLesson`.

---

## E_Questions

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| LessonId | uniqueidentifier | ✓ | FK → E_Lessons |
| Name | nvarchar | ✓ | texto da pergunta |
| Description | nvarchar | | explicação pós-resposta |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

---

## E_QuestionOptions

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| QuestionId | uniqueidentifier | ✓ | FK → E_Questions |
| Name | nvarchar | ✓ | texto da opção de resposta |
| Description | nvarchar | | explicação se esta opção for escolhida |
| IsCorrect | bit | ✓ | true = resposta correcta |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

---

## E_Certificates

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| UserId | uniqueidentifier | ✓ | FK → E_Users |
| CourseId | uniqueidentifier | ✓ | FK → E_Courses |
| CertifiedFile | varchar(500) | ✓ | URL do ficheiro (vazio = gerado on-demand) |
| ValidationCode | varchar(100) | | 12 chars alfanumérico único; index UNIQUE |
| ScorePercent | int | ✓ | ⚠️ coluna adicionada manualmente — default 0 |
| CompletedDate | datetime2 | ✓ | data de emissão |

---

## E_Testimonials

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| UserId | uniqueidentifier | | FK → E_Users (nullable — pode ser externo) |
| AuthorName | nvarchar | ✓ | |
| Role | nvarchar | ✓ | cargo/função |
| City | nvarchar | ✓ | |
| Country | nvarchar | ✓ | |
| PhotoUrl | nvarchar | ✓ | URL Cloudinary (pode ser `""` se sem foto) |
| LinkedinUrl | nvarchar | | |
| Quote | nvarchar | ✓ | texto do depoimento |
| Rating | int | ✓ | 1 a 5; default 5 |
| DisplayOrder | int | ✓ | ordenação na landing page; default 0 |
| Featured | bit | ✓ | destaque na landing page |
| CreatedDate | datetime2 | ✓ | |
| ChangedDate | datetime2 | | |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |

---

## E_Articles

| Coluna | Tipo | Obrigatório | Notas |
|--------|------|-------------|-------|
| Id | uniqueidentifier | ✓ | PK |
| SubjectId | uniqueidentifier | ✓ | FK → P_Subjects |
| Name | nvarchar | ✓ | título |
| Description | nvarchar | | resumo |
| Content | nvarchar(max) | | corpo do artigo |
| Slug | nvarchar | | URL amigável |
| CoverImage | nvarchar | | URL Cloudinary |
| Author | nvarchar | | nome do autor |
| PublishDate | datetime2 | ✓ | |
| ReadingTime | int | | minutos estimados de leitura |
| CreatedDate | datetime2 | ✓ | |
| ChangedDate | datetime2 | | |
| IsActived | bit | ✓ | |
| IsDeleted | bit | ✓ | |
