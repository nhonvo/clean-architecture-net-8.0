# Clean Architecture Web API Project

## About the Project

This project serves as a template for building a Clean Architecture Web API in ASP.NET Core. It focuses on separation of concerns by dividing the application into distinct layers: Domain, Application, Web API, and Infrastructure.

## Main Features

- [x] Clean Architecture structure (Domain, Application, Web API, Infrastructure)
- [x] ASP.NET Core 8.0 with Entity Framework Core
- [x] Docker support with SQL Server integration
- [x] JWT Token & Authentication by identity
- [x] Health Check and Logging
- [x] Unit testing
- [x] Middleware for Exception Handling and Validation
- [ ] Integration test, GA pipeline
- [ ] Unit test for auth identity code
- [ ] fix: scope assign

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Docker
- SQL Server

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/nhonvo/clean-architecture-net-8.0
   ```

2. Build and run:

   - Docker:

     ```bash
     docker-compose up --build
     ```

   - Local: Update the connection string in `appsettings.Development.json`

     and run:

     ```bash
     dotnet run ./src/CleanArchitecture/CleanArchitecture.csproj
     ```

3. Package project (Optional):

```bash
dotnet pack -o nupkg
dotnet new install ./ --force
# dotnet new install ./nupkg/CleanArchitecture.1.0.0.nupkg
dotnet new cleanarch -n template-project
```

### Usage

Access the API via:

- Docker: `http://localhost:3001/swagger/index.html`
  - Health check: `http://localhost:3001/health` & `http://localhost:3001/healthcheck-ui`
- Local: `http://localhost:5240/swagger/index.html`
  - Health check: `http://localhost:5240/health` & `http://localhost:5240/healthcheck-ui`

## Roadmap

- Issue tracking and planned features can be found [here](https://github.com/nhonvo/clean-architecture-net-8.0/issues).

## Contributing

Feel free to contribute by submitting issues or pull requests.

## License

This project is licensed under the MIT License.

## Contact

For any inquiries, contact the repository owner [here](https://github.com/nhonvo).
