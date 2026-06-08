# 🏢 TradeOffStack API
> **PRIVATE REPOSITORY** – Internal IT Asset Management System

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?logo=postgresql)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)
![CI/CD](https://img.shields.io/badge/CI%2FCD-Active-brightgreen?logo=github-actions)
![Status](https://img.shields.io/badge/Status-Production%20Ready-success)

## 🎯 Executive Summary
The **TradeOffStack API** is an enterprise-grade backend application designed exclusively for internal IT asset management. It provides a robust, scalable, and highly secure centralized system to track, assign, and maintain the company's hardware fleet (laptops, peripherals, servers, etc.).

Built with a stateless architecture, it is fully optimized for cloud deployment (VPS/Cloud Native), multi-instance load balancing, and containerized environments.

---

## 🏗 Architecture & Engineering Standards
This API adheres strictly to modern software engineering best practices:
- **N-Tier Architecture**: Clear separation of concerns (Controllers, Services, Repositories).
- **Generic Repository Pattern**: DRY compliance ensuring high maintainability and rapid integration of future domain entities.
- **Stateless & Cloud-Ready**: Session management via JWT, file storage delegated to Cloudflare R2, enabling horizontal scalability.
- **Fail-Fast Initialization**: Safe automated Entity Framework Core database migrations upon startup.
- **Comprehensive Logging & Auditing**: Strict tracking of every action performed on critical assets.

---

## ⚙️ Core Modules & Capabilities

### 🔐 1. Identity & Access Management (IAM)
- **Role-Based Access Control (RBAC)**: Secure access using JWT (JSON Web Tokens) with distinct roles (`Admin`, `IT_Support`, `Employee`).
- **Endpoints**: `POST /api/auth/login`, `POST /api/auth/register`, `GET /api/auth/me`.

### 💻 2. Asset & Equipment Management
- **Lifecycle Tracking**: Full CRUD operations for IT hardware, including status (`Available`, `InUse`, `InMaintenance`, `Retired`).
- **Endpoints**: `GET /api/equipment`, `POST /api/equipment`, `PUT /api/equipment/{id}`, `GET /api/equipment/category/{category}`.

### 📅 3. Reservations & Assignments
- **Concurrency Protection**: Strict rules to prevent double-booking or assigning unavailable equipment.
- **Endpoints**: `POST /api/reservation`, `GET /api/reservation/active`, `PUT /api/reservation/{id}/complete`.

### 🔧 4. Maintenance Requests
- **Ticketing Workflow**: Employees can report issues. IT Support can track repair statuses (`Pending`, `InProgress`, `Resolved`).
- **Endpoints**: `GET /api/maintenance`, `POST /api/maintenance`, `PUT /api/maintenance/{id}/status`.

### 🏢 5. Department Management
- **Logical Grouping**: Organizes users and tracks assets distributed across different company divisions.
- **Endpoints**: `GET /api/department`, `POST /api/department`.

### 📜 6. Security Audit Logs
- **Traceability**: Immutable logs of critical system events (e.g., who deleted a server, who updated a user).
- **Endpoints**: `GET /api/auditlog/{entityType}/{entityId}`.

---

## 🚀 Getting Started (Local Development)

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Quick Start via Docker Compose
The easiest way to boot the entire stack (PostgreSQL Database + .NET API) is through Docker.

```bash
# 1. Clone the repository
git clone https://github.com/YourEnterprise/TradeOffStackAPI.git
cd TradeOffStackAPI

# 2. Boot the infrastructure
docker-compose up -d --build

# 3. Check logs
docker-compose logs -f api
```

The API will be available at: `http://localhost:5000` (or the configured port).

---

## 📚 API Documentation (Swagger)
The API is self-documented using OpenAPI (Swagger). When running in development mode, you can access the visual API explorer and test endpoints directly:

👉 **Swagger UI**: `http://localhost:5000/swagger/index.html`

*Note: For secured endpoints, authenticate via `/api/auth/login` and paste the generated JWT in the `Authorize` (Bearer) menu at the top of the Swagger interface.*

---

## 🛡️ CI/CD Pipeline
Continuous Integration is configured via **GitHub Actions** (`.github/workflows/ci.yml`). 
On every `Push` or `Pull Request` to the `main` branch, the pipeline automatically:
1. Restores NuGet dependencies.
2. Compiles the solution (`Release` mode).
3. Executes all Unit & Integration Tests.

Deployments to pre-production/production are blocked if heuristics or tests fail.

---
*Confidentiality Notice: This repository contains proprietary source code belonging to the organization. Unauthorized copying, distribution, or external deployment is strictly prohibited.*
