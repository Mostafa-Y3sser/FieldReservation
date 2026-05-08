# FieldReservation: Football Field Reservation System

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-blue)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A robust, enterprise-grade booking and management system designed to streamline football field reservations. This system provides a real-time, self-service platform for players and a comprehensive management dashboard for field owners, eliminating the friction of manual booking processes.

## 🚀 Key Features

### For Players
- **Real-Time Booking**: Interactive slot selection with zero latency.
- **Secure Payments**: Integrated with **Stripe** for seamless checkout and automatic refunds.
- **Booking History**: Track upcoming and past reservations with payment receipts.
- **Self-Service Cancellations**: Automated refund logic for cancellations within 24 hours.

### For Field Owners (Admin)
- **Centralized Dashboard**: Manage all reservations and track revenue in one place.
- **Conflict Management**: Real-time conflict detection prevents double-bookings at the database level.
- **Maintenance Blocking**: Easily block specific time slots for maintenance or field repairs.
- **Player Management**: Searchable database of players with detailed booking history and spending metrics.

---

## 🏗️ Architecture & Technical Highlights

This project is built using **Clean Architecture** and **DDD (Domain-Driven Design)** principles, ensuring the system is maintainable, testable, and scalable.

### Patterns & Principles
- **CQRS (Command Query Responsibility Segregation)**: Decouples read and write operations using the **MediatR** library.
- **Atomic Operations**: Guarantees transaction integrity for reservations and payments.
- **Middleware Integration**: Custom middlewares for global error handling and request logging.
- **Scalability**: The data model supports multi-field configurations out of the box.

### Project Structure
- **FieldReservation.API**: The entry point, handling HTTP requests and hosting the OpenAPI documentation.
- **FieldReservation.Application**: Contains business logic, commands, queries, and interfaces.
- **FieldReservation.Domain**: Core entities, value objects, and domain exceptions (no dependencies).
- **FieldReservation.Infrastructure**: Implementation of external services (Stripe, Email, SQL Server Persistence).

---

## 🛠️ Technology Stack

- **Framework**: .NET 10 (C#)
- **API**: ASP.NET Core Web API
- **Persistence**: Entity Framework Core + SQL Server
- **Authentication**: JWT (JSON Web Tokens) with RBAC
- **Payments**: Stripe API (Checkout Sessions & Webhooks)
- **Communication**: SMTP for Email Notifications
- **Documentation**: Swagger/OpenAPI

---

## 🚦 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or LocalDB)
- [Stripe Account](https://stripe.com/) (for payment testing)

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Mostafa-Y3sser/FieldReservation.git
   cd FieldReservation/src
   ```

2. **Configure Environment Variables**:
   Create an `appsettings.json` or use User Secrets to set:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=...;Database=FieldReservation;..."
     },
     "Stripe": {
       "SecretKey": "sk_test_...",
       "WebhookSecret": "whsec_..."
     },
     "Jwt": {
       "Key": "your_secret_key",
       "Issuer": "FieldReservation"
     }
   }
   ```

3. **Apply Database Migrations**:
   ```bash
   dotnet ef database update --project FieldReservation.Infrastructure --startup-project FieldReservation.API
   ```

4. **Run the Application**:
   ```bash
   dotnet run --project FieldReservation.API
   ```

5. **Explore the API**:
   Open `https://sportify-m.runasp.net/swagger/index.html` to interact with the API via Swagger UI.

---

## 📄 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
