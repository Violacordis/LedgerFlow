# LedgerFlow

A fintech backend system that simulates how real payment processors handle transactions internally. Built with .NET 8, RabbitMQ, PostgreSQL, and clean architecture principles.

## What It Does

- Receives transactions via a REST API and queues them for async processing
- Runs fraud/risk checks using a pluggable rule engine
- Posts double-entry ledger entries for every settled transaction
- Tracks transactions through a defined state machine
- Provides real-time monitoring via SignalR

## Architecture

```
Client
  ↓
REST API  (LedgerFlow.Api)
  ↓
RabbitMQ Queue
  ↓
Worker Service  (LedgerFlow.Worker)
  ↓
Risk Engine → Ledger Engine
  ↓
PostgreSQL  (via LedgerFlow.Infrastructure)
```

### Clean Architecture Layers

```
LedgerFlow.Domain          → Entities, enums, state machine, exceptions (no dependencies)
LedgerFlow.Application     → Use cases, interfaces, DTOs (depends on Domain only)
LedgerFlow.Infrastructure  → EF Core, RabbitMQ, JWT (implements Application interfaces)
LedgerFlow.Api             → HTTP controllers, middleware, SignalR hub
LedgerFlow.Worker          → Background queue consumer
```

## Tech Stack

| Concern | Technology |
|---------|------------|
| Backend | .NET 8 |
| Database | PostgreSQL |
| ORM | Entity Framework Core |
| Messaging | RabbitMQ |
| Auth | JWT (custom) |
| Real-time | SignalR |
| Containerization | Docker Compose (optional) |
| Currency | NGN (single currency) |

## Transaction States

```
RECEIVED → PROCESSING → RISK_CHECK → APPROVED / FLAGGED / DECLINED → SETTLED
```

## Risk Engine Rules

| Rule | Score Impact |
|------|-------------|
| Amount > ₦1,000,000 | +40 |
| More than 3 transactions from same sender in 30 seconds | +35 |
| Transaction between 12am – 4am WAT | +20 |

**Score outcomes:** 0–30 → APPROVED · 31–60 → FLAGGED · >60 → DECLINED

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL and RabbitMQ — run locally **or** via Docker

---

### Option A — Run Without Docker

1. Install [PostgreSQL](https://www.postgresql.org/download/) and [RabbitMQ](https://www.rabbitmq.com/download.html) locally
2. Create a PostgreSQL database (any name you prefer)
3. Update the connection strings in `src/LedgerFlow.Api/appsettings.Development.json` and `src/LedgerFlow.Worker/appsettings.Development.json` with your database name, PostgreSQL credentials, and RabbitMQ credentials.
4. Apply database migrations:
```bash
dotnet ef database update --project src/LedgerFlow.Infrastructure --startup-project src/LedgerFlow.Api
```
5. Run the API and Worker in separate terminals:
```bash
dotnet run --project src/LedgerFlow.Api
dotnet run --project src/LedgerFlow.Worker
```

---

### Option B — Run With Docker

1. Install [Docker Desktop](https://www.docker.com/products/docker-desktop)
2. Start everything (PostgreSQL, RabbitMQ, API, Worker):
```bash
docker-compose up -d
```

---

### Run Tests

```bash
dotnet test
```

## API Endpoints

Full API docs available at `/swagger` when running locally.

## Roles

| Role | Access |
|------|--------|
| `customer` | Submit and view own transactions |
| `admin` | Everything + reverse transactions, manage accounts |

## Project Status

🚧 In active development
