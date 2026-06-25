# SPEC/04-content.md — Conteúdo Editorial

Módulos: Testemunhos, Artigos.

---

## 9. Testemunhos (`E_Testimonials`)

Depoimentos de formandos exibidos na landing page.
- Campos: AuthorName, Role, City, Country, PhotoUrl (Cloudinary), Quote, Rating (1-5), LinkedinUrl, Featured, DisplayOrder, IsActived
- Foto: upload opcional via Cloudinary; Edit mantém foto existente se não for substituída

### Fluxo de Gestão

```
Index → lista todos → botões Editar / Eliminar
Create → formulário horizontal → upload foto (opcional) → Cloudinary
Edit   → formulário horizontal → foto existente em preview
         → nova foto substitui (apaga antiga do Cloudinary)
         → sem nova foto → mantém PhotoUrl existente
```

### Regras de Negócio

| Regra | Detalhe |
|-------|---------|
| Foto de testemunho: opcional | `PhotoUrl` pode ser `""` (sem foto) |
| Rating de testemunho: 1 a 5 | Sem validação server-side estrita (apenas UI min/max) |

---

## 10. Artigos (`E_Articles`)

Blog/conteúdo informativo da plataforma.
- Campos: Title, Slug, Content, CoverImage, Subject, IsActived
- `CoverImage` → URL Cloudinary
- `Slug` → URL amigável (gerado a partir do título)
- `ReadingTime` → minutos estimados de leitura (campo manual)
