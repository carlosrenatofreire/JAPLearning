# SPEC/00-overview.md — Visão Geral

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

## Integrações Externas

| Serviço | Uso | Interface |
|---------|-----|-----------|
| **Cloudinary** | Upload/gestão de imagens (fotos de utilizadores, testemunhos, capas de artigos) | `ICloudinaryService` |
| **Email** | Notificações (não implementado na UI ainda) | `IEmailService` |
| **Doppler** | Gestão de secrets/configurações em produção | `Doppler.Extensions.Configuration` |

---

## Dados de Referência (Seeders)

A plataforma inclui seeders para dados iniciais:
- Roles de sistema (Administrador, Supervisor, Formando)
- Utilizador administrador padrão
- Níveis de dificuldade (Iniciante, Intermédio, Avançado)
