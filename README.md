# Job Application API

A backend service for managing job postings and candidate applications, built with ASP.NET Core following Clean Architecture and CQRS principles.

Recruiters can post and close job listings, while candidates can browse, apply, and cancel their applications — all backed by role-based authentication and background job processing for notifications.

## Features

- **Authentication & Authorization** — Register/Login with ASP.NET Core Identity, JWT-based auth, and role-based access control (`Candidate`, `Recruiter`)
- **Job Management** — Recruiters can create and close job postings; only the owning recruiter can close their own jobs
- **Applications** — Candidates can apply to open jobs, with protections against duplicate applications and applying to closed jobs
- **Cancellation** — Candidates can cancel their own applications, restricted to specific application statuses
- **Background Notifications** — Email notifications are dispatched asynchronously via Hangfire so core requests aren't blocked
- **Recurring Maintenance Jobs** — A daily Hangfire recurring job automatically closes stale job postings (active for 30+ days), visible and manually triggerable from the Hangfire Dashboard
- **API Documentation** — Swagger/OpenAPI with JWT bearer support and XML doc comments

## Tech Stack

| Layer | Technology |
|---|---|
| Language / Framework | C# / .NET 8, ASP.NET Core Web API |
| Database | SQL Server, Entity Framework Core (Code-First) |
| Auth | ASP.NET Core Identity, JWT Bearer |
| CQRS | MediatR |
| Background Jobs | Hangfire |
| API Docs | Swashbuckle (Swagger) |

## Architecture

The project follows **Clean Architecture**, split into four layers:

```
JobApplication.Domain          → Entities & Enums (no external dependencies)
JobApplication.Application     → DTOs, Interfaces, CQRS Commands & Handlers
JobApplication.Infrastructure  → EF Core, Identity, Repositories, external service implementations
JobApplication.API             → Controllers, configuration, composition root
```

Each layer only depends on the layers beneath it — the Domain layer has no knowledge of EF Core, Identity, or any framework-specific concerns.

Business logic is organized using **CQRS**: every write operation (Create Job, Close Job, Apply for Job, Cancel Application) is implemented as a MediatR `Command` + `Handler` pair, keeping controllers thin and each use case isolated and testable.

## Project Structure

```
Application/
└── Features/
    ├── Jobs/
    │   └── Commands/
    │       ├── CreateJob/
    │       ├── CloseJob/
    │       └── AutoCloseStaleJobs/
    └── Applications/
        └── Commands/
            ├── ApplyForJob/
            └── CancelApplication/

Infrastructure/
├── Identity/          (ApplicationUser, Register/Login handlers)
├── Persistence/        (ApplicationDbContext)
├── Repositories/        (generic Repository<T>)
└── Services/            (EmailNotificationService, HangfireBackgroundJobScheduler, JobMaintenanceService)
```

## API Endpoints

| Method | Endpoint | Role | Description |
|---|---|---|---|
| POST | `/api/auth/register` | — | Register as a Candidate or Recruiter |
| POST | `/api/auth/login` | — | Log in and receive a JWT |
| POST | `/api/jobs` | Recruiter | Create a new job posting |
| PUT | `/api/jobs/{id}/close` | Recruiter | Close a job you own |
| POST | `/api/applications` | Candidate | Apply for an open job |
| DELETE | `/api/applications/{id}` | Candidate | Cancel your own application |

## Key Design Decisions

- **Ownership from the token, not the request body** — Fields like `RecruiterId` and the applying candidate's identity are always derived from JWT claims (`ClaimTypes.NameIdentifier`), never trusted from client input, to prevent identity spoofing.
- **Abstracted background jobs** — Rather than calling Hangfire's static API directly from business logic, an `IBackgroundJobScheduler` interface keeps the Application layer decoupled from the specific background job library in use.
- **Status whitelisting** — Application cancellation explicitly allows only `Applied` and `UnderReview` statuses, rather than blacklisting disallowed ones, so newly added statuses default to being safe.
- **Bridging Hangfire and MediatR** — Since Hangfire can't invoke MediatR commands directly, a thin `IJobMaintenanceService` wrapper sits between the recurring job trigger and the `AutoCloseStaleJobsCommand`, keeping the scheduling mechanism decoupled from the CQRS pipeline.

## Roadmap

- [ ] GET endpoints for browsing jobs and applications
- [ ] Unit and integration test coverage

## Author

Built as part of a hands-on .NET Backend Development training program with **EraaSoft × iCareer**.
