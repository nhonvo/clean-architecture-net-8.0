# Clean Architecture Web API Project

This repository serves as a template and guide for building a Clean Architecture project in C# using ASP.NET Web API. The project structure adheres to the principles of Clean Architecture, separating concerns into distinct layers: Domain, Application, Web API, and Infrastructure.

## Project Structure

### 1. Domain

This layer contains the core business logic and entities of the application. It is independent of any infrastructure or application details. Keep it clean and focus on representing the business rules and behaviors.

### 2. Application

The Application layer orchestrates the flow of data between the Domain layer and the Infrastructure layer. It contains use cases, business logic, and application-specific rules. Use the Repository pattern and Unit of Work to interact with the database.

### 3. Web API

The Web API layer exposes endpoints to interact with the application. It handles incoming HTTP requests, maps them to application use cases, and returns appropriate responses. It uses ASP.NET Web API to provide a RESTful interface.

### 4. Infrastructure

Infrastructure contains implementation details like databases, external services, and other tools required for the application to function. Use Entity Framework (EF) Core as the ORM for database interactions, SQL Server as the database, and Docker for containerization.

## Dependencies

- ASP.NET Core 8.0
- Entity Framework Core
- Docker
- SQL Server
- Xunit (for unit testing)
- JWT for token-based authentication

## Features

### Sample API

The project includes a sample API to demonstrate the basic structure and functionality. It can be found in the Web API layer.

### Seeding Data

Utilize EF Core Migrations to create and update the database schema. Seed the database with initial data using data seeding techniques.

```bash
dotnet ef migrations add init --output-dir Infrastructure/Migrations
dotnet ef database update
```

### Logging Request-Response

Implement logging to capture requests and responses for better traceability and debugging.

### Health Check

Include a health check endpoint to monitor the application's overall health and status.

### Exception Middleware

Implement middleware to handle exceptions gracefully, providing meaningful responses to clients.

### Validation Middleware

Use middleware for request validation to ensure that incoming data meets the expected criteria.

### Basic Authentication (Login, Register)

Implement basic authentication for user login and registration.

### JWT Token Authentication

Secure your API with JWT token-based authentication to control access and maintain user sessions.

### Unit Tests

Write unit tests to ensure the correctness of the application's components.

## Getting Started

1. Clone the repository

```bash
git clone https://github.com/nhonvo/clean-architecture-net-8.0
```

2. Build and run the Docker container

```bash
docker-compose up --build
```

3. Access the API

```bash
http://localhost:3001/api/{your-endpoints}
```

## Contributing

Feel free to contribute to this project by submitting issues, feature requests, or pull requests.

## License

This project is licensed under the MIT License.

## For development

pack a project command 

```bash
dotnet new install ./ --force
dotnet pack -o nupkg
dotnet new install ./nupkg/CleanArchitecture.1.0.0.nupkg
dotnet new cleanarch -n MyFirstProject
```
