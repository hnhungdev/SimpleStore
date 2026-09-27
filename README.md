# 🎬 Simple Store

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge\&logo=dotnet\&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-CC292B?style=for-the-badge\&logo=microsoftsqlserver\&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-6.2-DC382D?style=for-the-badge\&logo=redis)
![Docker](https://img.shields.io/badge/Docker-29-2496ED?style=for-the-badge\&logo=docker)

> A modular e-commerce backend built with **.NET 8**, **SQL Server**, **Redis**, and **RabbitMQ**, designed to demonstrate **Clean Architecture**, **Domain-Driven Design**, **CQRS**, **event-driven architecture**, and distributed checkout workflows.

---

## 📖 Table of Contents

* [📌 Overview](#-overview)
* [🚀 Key Features](#-key-features)
* [🛠 Tech Stack](#-tech-stack)
* [🏗 Architecture](#-architecture)
* [📂 Project Structure](#-project-structure)
* [⚙️ Getting Started](#️-getting-started)
* [📬 Contact](#-contact)
* [📄 License](#-license)

---

## 📌 Overview

SimpleStore is a modular e-commerce backend built with ASP.NET Core .NET 8, following Domain-Driven Design (DDD) and a modular architecture.

The project is designed to demonstrate how common e-commerce business flows can be separated into independent modules while communicating through asynchronous integration events.

---

## 🚀 Key Features

| Feature                                   | Description                                                                                                                                                   |
| ----------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Clean Architecture**                    | Separation of concerns across Domain, Application, Infrastructure, and API layers with dependency inversion and clear architectural boundaries.               |
| **Domain-Driven Design (DDD)**            | Domain-centric design with entities, value objects, domain rules, domain events, and business logic isolated from infrastructure concerns.                    |
| **Modular Architecture**                  | Business capabilities are separated into independent modules with their own Domain, Application, Infrastructure, and API layers.                              |
| **Authentication & Authorization**        | JWT-based authentication with ASP.NET Core Identity, refresh tokens, role-based authorization, and admin user management.                                     |
| **Product Catalog**                       | Product management with filtering, sorting, searching, pagination, product status management, and image support.                                              |
| **Shopping Cart**                         | User-specific shopping carts with product quantity management, unique products per cart, and Redis caching.                                                   |
| **Order Management**                      | Order creation and lifecycle management with customer and product snapshots, order status transitions, cancellation, and payment method handling.             |
| **Inventory Management**                  | Inventory tracking with on-hand, reserved, and available quantities, including inventory reservation and release during checkout.                             |
| **Checkout Saga**                         | Distributed checkout workflow using MassTransit Saga to coordinate Order, Inventory, and Payment modules with failure handling and compensation.              |
| **Online Payment Integration**            | VNPay and MoMo payment integration with payment transaction tracking, callback/IPN handling, payment expiration, verification, and payment status management. |
| **Cash on Delivery**                      | COD payment support with payment completion triggered when the order is successfully delivered.                                                               |
| **Messaging & Event-Driven Architecture** | RabbitMQ and MassTransit for asynchronous communication between modules using integration events.                                                             |
| **Transactional Outbox & Inbox**          | Reliable integration event publishing with the Outbox pattern and duplicate message prevention using Inbox processing.                                        |
| **Real-time Notifications**               | SignalR-based notification hub for delivering real-time order and payment notifications to connected clients.                                                 |
| **Caching**                               | Redis integration for caching frequently accessed data such as shopping carts with expiration and cache invalidation.                                         |
| **API Documentation**                     | Swagger/OpenAPI integration for exploring and testing REST API endpoints during development.                                                                  |
| **Validation**                            | FluentValidation-based request validation for application commands and queries.                                                                               |
---

## 🛠 Tech Stack

### Backend

| Technology                    | Purpose                                                                              |
| ----------------------------- | ------------------------------------------------------------------------------------ |
| **.NET / ASP.NET Core**       | Web framework — Minimal APIs with Carter for HTTP endpoints and application hosting. |
| **Entity Framework Core**     | ORM, database access, migrations, entity configuration, and persistence.             |
| **MediatR**                   | CQRS request/handler pipeline for application commands and queries.                  |
| **FluentValidation**          | Request and command validation.                                                      |
| **MassTransit**               | Message-based communication, transactional outbox/inbox, and Saga orchestration.     |
| **ASP.NET Core Identity**     | User management, authentication, roles, and authorization.                           |
| **JWT Bearer Authentication** | Stateless authentication using access tokens.                                        |
| **Carter**                    | Lightweight endpoint routing and Minimal API organization.                           |
| **SignalR**                   | Real-time communication for user notifications.                                      |
| **Swagger / OpenAPI**         | API documentation and interactive API testing.                                       |

### Database & Infrastructure

| Technology      | Purpose                                                                                                     |
| --------------- | ----------------------------------------------------------------------------------------------------------- |
| **SQL Server**  | Primary relational database for module-specific data and transactional operations.                          |
| **Redis**       | Distributed caching for shopping carts and frequently accessed application data.                            |
| **RabbitMQ**    | Message broker for asynchronous communication and integration events between modules.                       |
| **MinIO**       | S3-compatible object storage for product images and application files.                                      |
| **MailHog**     | Local email testing service for development and testing.                                                    |
| **Docker**      | Containerized infrastructure services for local development and deployment.                                 |
| **Docker Compose** | Infrastructure orchestration for local development and deployment.                                          |

## 🏗 Architecture

SimpleStore is organized into business modules instead of putting the entire application into a single large project.

                         ┌──────────────┐
                         │     API      │
                         └──────┬───────┘
                                │
             ┌──────────────────┼──────────────────┐
             │                  │                  │
             ▼                  ▼                  ▼
        ┌─────────┐        ┌─────────┐        ┌─────────┐
        │ Identity│        │ Catalog │        │  Cart   │
        └─────────┘        └─────────┘        └─────────┘
                                │
                                ▼
                         ┌─────────────┐
                         │    Order    │
                         └──────┬──────┘
                                │
                         Checkout Saga
                                │
                ┌───────────────┼───────────────┐
                ▼               ▼               ▼
          ┌───────────┐   ┌───────────┐   ┌───────────┐
          │ Inventory │   │  Payment  │   │Notification│
          └───────────┘   └───────────┘   └───────────┘

Modules communicate through integration events over RabbitMQ rather than directly depending on each other's implementation.

---

## 📁 Project Structure

SimpleStore follows a **Modular Architecture** combined with **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

Each business module is organized into separate layers to keep domain logic isolated from application, infrastructure, and API concerns.

```text
src/
│
├── Api
│   └── SimpleStore.Api
│
├── BuildingBlocks
│   ├── Application
│   ├── Domain 
│   ├── Infrastructure 
│   └── Contracts
│
└── Modules
   ├── Identity
   ├── Catalog
   ├── Cart
   ├── Inventory
   ├── Order
   ├── Payment
   └── Notification
```

### Module Structure

Each business module follows the same layered structure:

```text
Module/
├── Domain/
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Enums/
│   ├── Events/
│   ├── Constants/
│   └── Repositories/
│
├── Application/
│   ├── Features/
│   │   ├── Commands/
│   │   ├── Queries/
│   │   ├── Dtos/
│   │   ├── EventHandlers/
│   │   └── Services/
│   └── Abstractions/
│
├── Infrastructure/
│   ├── Persistence/
│   ├── Services/
│   └── Messaging/
│
└── Api/
    ├── Endpoints/
    └── Extensions/
```

### Layer Responsibilities

| Layer              | Responsibility                                                                                                                                      |
| ------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Domain**         | Contains core business rules, entities, value objects, domain events, and domain abstractions.                                                      |
| **Application**    | Implements use cases through commands and queries, validation, DTOs, and application services.                                                      |
| **Infrastructure** | Provides persistence, EF Core configurations, repositories, external service integrations, messaging, caching, and other technical implementations. |
| **API**            | Exposes HTTP endpoints and connects external requests to the Application layer.                                                                     |

### Dependency Direction

Dependencies follow the **Dependency Inversion Principle**:

```text
API
 ↓
Application
 ↓
Domain

Infrastructure
 ↓
Application
 ↓
Domain
```

The Domain layer remains independent of infrastructure and external frameworks wherever possible.

### Cross-Module Communication

Modules avoid direct dependencies on another module's internal implementation.

Communication between modules is primarily handled through **integration events**:

```text
┌──────────────┐
│    Order     │
└──────┬───────┘
       │ Integration Event
       ▼
┌──────────────┐
│   RabbitMQ   │
└──────┬───────┘
       │
       ▼
┌──────────────┐
│   Payment    │
└──────────────┘
```

This keeps modules loosely coupled while allowing business workflows such as checkout, payment, inventory reservation, and notifications to work together.

---

## ⚙️ Getting Started

### Prerequisites

Make sure the following are installed:

* [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* Git
* Entity Framework Core CLI

Install the EF Core CLI if it is not already installed:

```bash
dotnet tool install --global dotnet-ef
```

Payment gateway credentials are only required if you want to test online payments.

### 1. Clone the repository

```bash
git clone https://github.com/hnhungdev/SimpleStore.git

cd SimpleStore
```

### 2. Configure environment variables

Copy the example environment file for Docker Compose:

```bash
cp .env.example .env
```

The `.env` file contains the configuration used by Docker Compose for local infrastructure services such as SQL Server, Redis, MinIO, RedisInsight, RabbitMQ, and MailHog.

Create the development application settings file:

```bash
cp src/Api/appsettings.json src/Api/appsettings.Development.json
```

Then update `appsettings.Development.json` with your local configuration.

For VNPay or MoMo payment testing, configure the corresponding sandbox credentials and callback URLs.

### 3. Start infrastructure services

Start the required infrastructure services with Docker Compose:

```bash
docker compose up -d
```

### 4. Apply database migrations

Run the migration script from the repository root:

```powershell
.\migrate.ps1
```

The script updates all module databases:

* Identity
* Cart
* Catalog
* Inventory
* Payment
* Order
* Notification

EF Core automatically skips migrations that have already been applied and only executes pending migrations.

### 5. Start the application

Run the API:

```bash
dotnet run --project src/Api
```

Alternatively, the API can be started using the configured development profile.

### 6. Access the services

| Service                 | URL                                        |
| ----------------------- | ------------------------------------------ |
| **MinIO Console**       | http://localhost:9001                      |
| **RedisInsight**        | http://localhost:5540                      |
| **MailHog**             | http://localhost:8025                      |
| **RabbitMQ Management** | http://localhost:15672                     |
| **Swagger**             | http://localhost:8080/swagger              |

---

## 📬 Contact

* **Name:** Hoàng Ngọc Hưng
* **Email:** `hnhungdev@gmail.com`

---

## 📄 License

This project is licensed under the [MIT License](LICENSE.txt).