# Clean Architecture Web API Project

## About the Project

This project serves as a template for building a Clean Architecture Web API in ASP.NET Core. It focuses on separation of concerns by dividing the application into distinct layers: Domain, Application, Web API, and Infrastructure.

## Main Features

- Clean Architecture structure (Domain, Application, Web API, Infrastructure)
- ASP.NET Core 8.0 with Entity Framework Core
- Docker support with SQL Server integration
- JWT Token Authentication
- Health Check and Logging
- Unit testing with Xunit
- Middleware for Exception Handling and Validation

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

### Usage

Access the API via:

- Docker: `http://localhost:3001/swagger/index.html`
- Local: `http://localhost:5240/swagger/index.html`

## Roadmap

- Issue tracking and planned features can be found [here](https://github.com/nhonvo/clean-architecture-net-8.0/issues).

## Contributing

Feel free to contribute by submitting issues or pull requests.

## License

This project is licensed under the MIT License.

## Contact

For any inquiries, contact the repository owner [here](https://github.com/nhonvo).

Note: For development

pack a project command

```bash
dotnet pack -o nupkg
dotnet new install ./ --force
# dotnet new install ./nupkg/CleanArchitecture.1.0.0.nupkg
dotnet new cleanarch -n MyFirstProject
```
