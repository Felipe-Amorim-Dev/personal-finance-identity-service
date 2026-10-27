<h1 align="center">🔐 Personal Finance — Identity Service</h1>

<p align="center"><em>Identidade e autenticação para a plataforma financeira.</em></p>

<p align="center">🇧🇷 Português | 🇺🇸 <a href="README.en.md">English</a></p>

<p align="center"><img src="https://img.shields.io/badge/licen%C3%A7a-MIT-006bed?style=flat-square" alt="MIT" /> <img src="https://img.shields.io/github/last-commit/Felipe-Amorim-Dev/personal-finance-identity-service?style=flat-square&color=006bed" alt="Last commit" /></p>

<p align="center"><a href="#sobre">Sobre</a> • <a href="#tecnologias">Tecnologias</a> • <a href="#como-rodar">Como rodar</a> • <a href="#licenca">Licença</a> • <a href="#autor">Autor</a></p>

---

<a id="sobre"></a>
## 📖 Sobre

Serviço de identidade em .NET 9 dividido em Host, API, Application, Domain e Infrastructure. Integra autenticação JWT, persistência MySQL com Pomelo EF Core, mensageria RabbitMQ e processamento de outbox.

## ✨ Funcionalidades

- Autenticação e validação JWT
- Persistência MySQL
- Processamento de mensagens com outbox
- Projetos de testes unitários e de integração

## 🛠️ Tecnologias

![C#](https://img.shields.io/badge/C%23-006bed?style=for-the-badge) ![.NET 9](https://img.shields.io/badge/.NET_9-006bed?style=for-the-badge) ![MySQL](https://img.shields.io/badge/MySQL-006bed?style=for-the-badge) ![EF Core 9.0.20](https://img.shields.io/badge/EF_Core_9.0.20-006bed?style=for-the-badge) ![Pomelo 9](https://img.shields.io/badge/Pomelo_9-006bed?style=for-the-badge) ![RabbitMQ](https://img.shields.io/badge/RabbitMQ-006bed?style=for-the-badge)

## 📦 Pré-requisitos

SDK .NET 9 e configurações locais de MySQL, JWT, RabbitMQ e e-mail.

<a id="como-rodar"></a>
## 🚀 Como rodar

```bash
git clone https://github.com/Felipe-Amorim-Dev/personal-finance-identity-service.git
cd personal-finance-identity-service
dotnet restore PersonalFinance.Identity.sln
dotnet build PersonalFinance.Identity.sln
dotnet run --project PersonalFinance.Identity.Host
```

## 🧪 Testes

```bash
dotnet test PersonalFinance.Identity.sln
```

## 🔗 Repositórios relacionados

- [personal-finance-platform](https://github.com/Felipe-Amorim-Dev/personal-finance-platform) — Plataforma financeira principal

<a id="licenca"></a>
## 📄 Licença

Distribuído sob a [licença MIT](LICENSE).

<a id="autor"></a>
## 👤 Autor

**Felipe Figueiredo Amorim** — Desenvolvedor Full Stack.

[![LinkedIn](https://img.shields.io/badge/LinkedIn-Felipe%20Amorim-0A66C2?style=for-the-badge&logo=linkedin)](https://www.linkedin.com/in/felipe-amorim-dev/) [![E-mail](https://img.shields.io/badge/E--mail-contato-006bed?style=for-the-badge)](mailto:felipe.f.amorim@outlook.com)

<p align="center"><strong>Obrigado por visitar! / Thanks for visiting!</strong></p>
