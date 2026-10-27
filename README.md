# Personal Finance — Identity Service

**Status: in active development / Em desenvolvimento.** An evolving .NET identity/authentication service for the [Personal Finance Platform](https://github.com/Felipe-Amorim-Dev/personal-finance-platform). Not production-certified.

## Implemented components visible in the repository
- Separate `Api`, `Application`, `Domain`, `Infrastructure` and `Host` projects.
- JWT validation and authentication middleware in the host.
- MySQL-backed persistence configuration, rate limiting and health-check registration.
- Email-related configuration and an outbox processing component.
- Unit and integration **test projects** are present; existence of projects alone does not establish passing tests or meaningful coverage.

## Architecture

```text
PersonalFinance.Identity.Host    application startup, middleware, composition
PersonalFinance.Identity.Api     HTTP interface
PersonalFinance.Identity.Application  use cases, application services, contracts
PersonalFinance.Identity.Domain       domain entities and abstractions
PersonalFinance.Identity.Infrastructure persistence, messaging, integration
PersonalFinance.Identity.UnitTests / IntegrationTests  test projects
```

## Local development

**Prerequisites:** compatible .NET SDK, MySQL and RabbitMQ for the configured integration paths, plus email infrastructure or suitable development configuration.

1. Clone the repository.
2. Provide **local, noncommitted** configuration for `ConnectionStrings:DefaultConnection`, `ConnectionStrings:RabbitMq`, `Jwt:SecretKey`, `Jwt:Issuer`, `Jwt:Audience`, and `Email` settings. `Program.cs` checks required settings at startup.
3. Review the application settings and connection strings before running migrations.
4. Run `dotnet restore PersonalFinance.Identity.sln` and `dotnet build PersonalFinance.Identity.sln`.
5. Run `dotnet test PersonalFinance.Identity.sln`. Confirm actual test discovery/results before reporting coverage.
6. Start the host with `dotnet run --project PersonalFinance.Identity.Host` only after configuring required services.

**Security:** never commit credentials or reuse example keys in a deployed environment. JWT, outbox processing and health checks require integration and operational testing before production.

## Roadmap
- Add and run meaningful application/domain/integration tests.
- Validate migrations and deployment configuration end to end.
- Evaluate concurrency, idempotency and retry handling for the outbox.
- Configure CI and review results.

---

**PT-BR:** Microsserviço de identidade em evolução, com projetos separados por responsabilidade e infraestrutura de persistência/mensageria. As funcionalidades presentes no código não significam validação de produção; testes e integração continuam necessários.
