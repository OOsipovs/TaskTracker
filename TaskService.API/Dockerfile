FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["TaskService.API/TaskService.API.csproj", "TaskService.API/"]
COPY ["TaskService.Application/TaskService.Application.csproj", "TaskService.Application/"]
COPY ["TaskService.Domain/TaskService.Domain.csproj", "TaskService.Domain/"]
COPY ["TaskService.Infrastructure/TaskService.Infrastructure.csproj", "TaskService.Infrastructure/"]
COPY ["TaskTracker.Shared/TaskTracker.Shared.csproj", "TaskTracker.Shared/"]

# Restore dependencies
RUN dotnet restore "TaskService.API/TaskService.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/TaskService.API"
RUN dotnet build "TaskService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskService.API.dll"]