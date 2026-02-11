# Task Tracker - Microservices Application

A task management application built with **ASP.NET Core microservices** and **Angular**.

## Architecture

This application follows a **microservices architecture** with two independent services:

- **AuthService** - User authentication and JWT token generation
- **TaskService** - Task CRUD operations with user authorization

Each service follows **Clean Architecture** principles with layers:
- **Domain** - Entities and interfaces
- **Application** - Business logic and DTOs
- **Infrastructure** - Data access and repositories
- **API** - REST controllers

## Technologies

### Backend
- ASP.NET Core 8.0 Web API
- Entity Framework Core 8.0
- SQLite (for simplicity)
- JWT Authentication
- BCrypt for password hashing
- Swagger/OpenAPI
- xUnit for testing

### Frontend (to be implemented)
- Angular 17+
- TypeScript
- RxJS

## Prerequisites

- .NET 8.0 SDK
- Docker Desktop (for containerized deployment)
- Visual Studio 2022 or VS Code

## Running Locally (Without Docker)

### 1. Clone the repository
