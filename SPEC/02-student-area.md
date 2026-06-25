# SPEC/02-student-area.md — Área do Formando

Fluxos do formando: Player, Quiz, Certificados.

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

## Fluxo do Formando — Consumir Formação

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

---

## Fluxo de Quiz (JavaScript no Player)

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

---

## Fluxo de Emissão de Certificado

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

---

## Regras de Negócio — Certificados

| Regra | Detalhe |
|-------|---------|
| Um certificado por utilizador por formação | `HasCertificateAsync` verifica antes de emitir |
| ValidationCode único de 12 chars | `Guid.NewGuid().ToString("N").ToUpper()[..12]` |
