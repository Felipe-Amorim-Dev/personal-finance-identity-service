<h1 align="center">🔐 Personal Finance — Identity Service</h1>

<p align="center"><em>Identity and authentication for the personal finance platform.</em></p>

<p align="center">🇧🇷 <a href="README.md">Português</a> | 🇺🇸 English</p>

<p align="center"><img src="https://img.shields.io/badge/license-MIT-006bed?style=flat-square" alt="MIT" /> <img src="https://img.shields.io/github/last-commit/Felipe-Amorim-Dev/personal-finance-identity-service?style=flat-square&color=006bed" alt="Last commit" /></p>

<p align="center"><a href="#sobre">About</a> • <a href="#tecnologias">Technologies</a> • <a href="#como-rodar">Running locally</a> • <a href="#licenca">License</a> • <a href="#autor">Author</a></p>

---

<a id="sobre"></a>
## 📖 About

Identity service using .NET 9 and separate Host, API, Application, Domain and Infrastructure projects. Integrates JWT authentication, MySQL with Pomelo EF Core, RabbitMQ and outbox processing.

## ✨ Features

- JWT authentication and validation
- MySQL persistence
- Outbox message processing
- Unit and integration test projects

## 🛠️ Technologies

![C#](https://img.shields.io/badge/C%23-006bed?style=for-the-badge) ![.NET 9](https://img.shields.io/badge/.NET_9-006bed?style=for-the-badge) ![MySQL](https://img.shields.io/badge/MySQL-006bed?style=for-the-badge) ![EF Core 9.0.20](https://img.shields.io/badge/EF_Core_9.0.20-006bed?style=for-the-badge) ![Pomelo 9](https://img.shields.io/badge/Pomelo_9-006bed?style=for-the-badge) ![RabbitMQ](https://img.shields.io/badge/RabbitMQ-006bed?style=for-the-badge)

## 📦 Prerequisites

.NET 9 SDK and locally configured MySQL, JWT, RabbitMQ and email settings.

<a id="como-rodar"></a>
## 🚀 Running locally

```bash
git clone https://github.com/Felipe-Amorim-Dev/personal-finance-identity-service.git
cd personal-finance-identity-service
dotnet restore PersonalFinance.Identity.sln
dotnet build PersonalFinance.Identity.sln
dotnet run --project PersonalFinance.Identity.Host
```

## 🧪 Tests

```bash
dotnet test PersonalFinance.Identity.sln
```

## 🔗 Related repositories

- [personal-finance-platform](https://github.com/Felipe-Amorim-Dev/personal-finance-platform) — Plataforma financeira principal

<a id="licenca"></a>
## 📄 License

Distributed under the [MIT license](LICENSE).

<a id="autor"></a>
## 👤 Author

**Felipe Figueiredo Amorim** — Full Stack Developer.

[![LinkedIn](https://img.shields.io/badge/LinkedIn-Felipe%20Amorim-0A66C2?style=for-the-badge&logo=linkedin)](https://www.linkedin.com/in/felipe-amorim-dev/) [![E-mail](https://img.shields.io/badge/E--mail-contato-006bed?style=for-the-badge)](mailto:felipe.f.amorim@outlook.com)

<p align="center"><strong>Obrigado por visitar! / Thanks for visiting!</strong></p>
