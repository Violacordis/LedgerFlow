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
| Auth | JWT (custom, no ASP.NET Identity) |
| Real-time | SignalR |
| Containerization | Docker Compose |
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
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Run Locally

```bash
# Clone the repo
git clone https://github.com/yourusername/LedgerFlow.git
cd LedgerFlow

# Start infrastructure (PostgreSQL + RabbitMQ)
docker-compose up -d

# Run the API
dotnet run --project src/LedgerFlow.Api

# Run the Worker (separate terminal)
dotnet run --project src/LedgerFlow.Worker
```

### Run Tests

```bash
dotnet test
```

## API Endpoints

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/v1/auth/register` | Register account | Public |
| POST | `/api/v1/auth/login` | Login, get JWT | Public |
| POST | `/api/v1/transactions` | Submit transaction | Customer |
| GET | `/api/v1/transactions` | List transactions (paginated) | Customer / Operator |
| GET | `/api/v1/transactions/{reference}` | Get transaction status | Customer |
| GET | `/api/v1/accounts/{id}/transactions` | Account transaction history | Customer |
| GET | `/health` | Health check | Public |

Full API docs available at `/swagger` when running locally.

## Project Status

🚧 In active development
