# SPEC/01-learning-core.md — Núcleo de Aprendizagem

Módulos: Formações, Tópicos, Lições, Questões, Opções de Resposta.

---

## 3. Formações / Cursos (`E_Courses`)

Unidade principal de aprendizagem.
- Campos: Título, Descrição, Imagem de Capa, Nível, Categoria, Equipa, PassingScore (int, default 60), IsActived
- Uma formação tem vários **Tópicos**
- `PassingScore` define a nota mínima (%) para obter certificado

---

## 4. Tópicos (`P_Topics`)

Agrupam lições dentro de uma formação. Ordenados por `DisplayOrder`.

---

## 5. Lições (`E_Lessons`)

Unidade de conteúdo. Dois tipos:
- **Lição de Vídeo** → tem URL de vídeo (YouTube/Vimeo embed); botão "Concluir Aula"
- **Lição de Quiz** → tem questões com opções; sem vídeo; quiz interactivo
- **Detecção automática:** `IsQuizLesson = Questions.Any(q => q.Options.Any())`
- Campos: Título, Descrição, VideoUrl, DisplayOrder, IsActived

---

## 6. Questões (`E_Questions`)

Perguntas de um quiz (ligadas a uma lição).
- Campos: QuestionText (textarea), Description/Explanation, LessonId, IsActived

---

## 7. Opções de Resposta (`E_QuestionOptions`)

Respostas possíveis de uma questão.
- Campos: Name (texto da opção), IsCorrect, Description (explicação pós-resposta), QuestionId, IsActived
- Selects em cascata na UI: Equipa → Formação → Tópico → Lição → Questão

---

## Regras de Negócio — Núcleo

| Regra | Detalhe |
|-------|---------|
| Certificado só se todas as lições concluídas | Verificado em `CompleteLesson` |
| Certificado só se média ≥ `PassingScore` | Default: 60% |
| Nota do quiz: melhor tentativa por lição | Não a média de tentativas |
| Lições sem quiz contam como 100% para a média | Garante que só a componente de quiz é avaliada |
