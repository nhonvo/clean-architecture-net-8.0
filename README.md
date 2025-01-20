# Clean Architecture Web API Project

## About the Project

This project serves as a template for building a Clean Architecture Web API in ASP.NET Core. It focuses on separation of concerns by dividing the application into distinct layers: Domain, Application, Web API, and Infrastructure.

## Main Features

### **1. Core Features (Completed)**

- Clean Architecture structure (Domain, Application, Web API, Infrastructure)
- ASP.NET Core 8.0 with Entity Framework Core
- Docker support with SQL Server integration
- JWT Token & Authentication by Identity
- Health Check and Logging
- Middleware for Exception Handling and Validation
- Unit Testing

### **2. Testing and Quality Assurance**

#### **2.1 Unit Testing**

- Write unit tests for authentication and identity code.

#### **2.2 Integration Testing**

- Set up integration tests with `HttpClient`.
- Update GitHub Actions (GA) pipeline to include integration tests and capture Docker logs.

### **3. Infrastructure and Hosting**

- Host the application on AWS EC2.
- Write Terraform scripts to initialize AWS resources.
- Add background services.
- Configure Hangfire for scheduled tasks.
- Apply sidecar architecture for job execution.

### **4. Enhancements and Fixes**

#### **4.1 Bug Fixes**

- Fix scope assignment issues.
- Fix user flow: update and remove unused files (e.g., status, avatar).

#### **4.2 Code Enhancements**

- Rename all models to include `Request`/`Response` postfix.
- Remove redundant models containing only one field.

#### **4.3 Warning and Pipeline Improvements**

- Fix project warnings.
- Enhance pipeline by separating build, unit test, and integration test stages.

### **5. Advanced Features**

- Add support for advanced technologies:
- Redis.
- Aspire.
- Splunk.
- New Relic for logging.
- Elasticsearch (ELK stack).

### **6. Real-World Testing**

- Add a real external service for testing.

### **7. Miscellaneous**

- Configure integration tests to run in Docker.

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


Note shared project for integration test, and unit test project which need use same model