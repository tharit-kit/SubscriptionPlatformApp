# 🗄️ Entity Framework Core Migrations Guide

This project follows a **Clean Architecture** structure:

- `SubscriptionPlatformApp.API` → Startup project (entry point)
- `SubscriptionPlatformApp.Infrastructure` → Contains `AppDbContext` and EF Core migrations
- `SubscriptionPlatformApp.Application` → Application logic
- `SubscriptionPlatformApp.Domain` → Domain models

---

## 📌 Migration Setup

EF Core migrations are stored in the **Infrastructure** project, while the **API** project is used as the startup project.

---

## 🚀 Add a New Migration

```bash
dotnet ef migrations add InitialCreate `
  --context AppDbContext `
  --project SubscriptionPlatformApp.Infrastructure `
  --startup-project SubscriptionPlatformApp.API `
  --output-dir Persistence/Migrations

dotnet ef database update `
  --project SubscriptionPlatformApp.Infrastructure `
  --startup-project SubscriptionPlatformApp.API

dotnet ef migrations remove `
  --project SubscriptionPlatformApp.Infrastructure `
  --startup-project SubscriptionPlatformApp.API