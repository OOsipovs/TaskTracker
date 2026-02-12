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

### Frontend
- Angular 17+
- TypeScript
- RxJS
- Standalone Components

## Prerequisites

### For Running with Docker
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (version 20.10 or higher)
- [Docker Compose](https://docs.docker.com/compose/install/) (usually included with Docker Desktop)

### For Running Locally (Without Docker)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (version 20 or higher) and npm
- Visual Studio 2022, VS Code, or Rider

## Running with Docker Compose (Recommended)

### 1. Clone the repository
git clone https://github.com/OOsipovs/TaskTracker.git
cd TaskTracker

### 2. Build and start all services
docker-compose up --build

### 3. Access the application

- **Angular UI**: http://localhost:4200
- **AuthService API**: http://localhost:5001
  - Swagger UI: http://localhost:5001/swagger
- **TaskService API**: http://localhost:5002
  - Swagger UI: http://localhost:5002/swagger

### 4. Stop the services
docker-compose down

### 5. Stop and remove containers AND volumes (removes all data)
docker-compose down -v

## Running Locally (Without Docker)

### Step 1: Clone the Repository
git clone https://github.com/OOsipovs/TaskTracker.git
cd TaskTracker

### Step 2: Run Backend Services

#### Option A: Using Visual Studio 2022

1. Open `TaskTracker.sln` in Visual Studio 2022
2. Right-click on the solution in Solution Explorer
3. Select **Configure Startup Projects**
4. Choose **Multiple startup projects**
5. Set both `AuthService.API` and `TaskService.API` to **Start**
6. Click **OK** and press **F5** to run

Both services will start automatically with Swagger UI.

#### Option B: Using Command Line

**Terminal 1 - AuthService:**
Open **TaskTracker/AuthService** folder in your terminal.
Run the following commands:

```
dotnet restore
dotnet run
```

- The service should now be running at `http://localhost:5001`
- Access Swagger UI at `http://localhost:5001/swagger`

**Terminal 2 - TaskService:**
Open **TaskTracker/TaskService** folder in your terminal.
Run the following commands:

```
dotnet restore
dotnet run
```

- The service should now be running at `http://localhost:5002`
- Access Swagger UI at `http://localhost:5002/swagger`

### Step 3: Run Angular Frontend

Open **TaskTracker/ClientApp** folder in your terminal.
Run the following commands:

```
npm install
ng serve
```

- The frontend should now be running at `http://localhost:4200`

### Step 4: Update API URLs in Frontend

Open **src/environments/environment.ts** in the Angular project.
Update the URLs for `apiAuthUrl` and `apiTasksUrl` to match your local AuthService and TaskService URLs, e.g.,

```typescript
export const environment = {
  production: false,
  apiAuthUrl: 'http://localhost:5001/api/auth/',
  apiTasksUrl: 'http://localhost:5002/api/tasks/'
};
```

### Step 5: Access the Application

- Open `http://localhost:4200` in your web browser
- The Angular UI should now be communicating with the local AuthService and TaskService


