# Blog System - Hexagonal Architecture Sample

This project demonstrates a blog system implemented using Hexagonal Architecture (also known as Ports and Adapters). The system allows managing authors and their blog posts.

## Architecture Overview

The solution follows Hexagonal Architecture principles with the following structure:

```
├── BlogCore/                 # Core Domain and Application Logic
│   ├── Domain/              # Domain Entities and Business Rules
│   ├── Ports/              
│   │   ├── Primary/        # Input Ports (Use Cases)
│   │   └── Secondary/      # Output Ports (Repositories)
│   └── UseCases/           # Use Case Implementations
│
├── BlogInfra/               # Infrastructure Layer
│   ├── Data/               # Database Context and Migrations
│   └── Repositories/       # Repository Implementations
│
├── BlogApi/                 # API Layer
│   └── Controllers/        # API Controllers
│
└── Tests/                   # Test Projects
    ├── Tests/              # Unit Tests
    ├── Tests.Integration/  # Integration Tests
    └── Tests.Benchmark/    # Performance Benchmarks
```

## Key Features

- Hexagonal Architecture implementation
- Clean separation of concerns
- Domain-driven design principles
- Unit and integration testing
- Performance benchmarking
- Entity Framework Core with SQL Server
- RESTful API endpoints

## Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB is sufficient)
- Visual Studio 2022 or VS Code
- ReportGenerator (for coverage reports)

## Getting Started

1. Clone the repository
2. Open the solution in your preferred IDE
3. Update the connection string in `BlogApi/appsettings.json` if needed
4. Run the following commands in the terminal:

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the API
cd BlogApi
dotnet run
```

The API will be available at `https://localhost:5001` and `http://localhost:5000`.

## Testing Instructions

### Testing Strategy

The project uses different testing approaches for different purposes:

1. **Unit Tests** (`Tests/`)
   - Test individual components in isolation
   - Use mocks for dependencies
   - Fast execution
   - High coverage of business logic

2. **Integration Tests** (`Tests.Integration/`)
   - Test components working together
   - Use real SQL Server database (LocalDB)
   - Database transactions for test isolation
   - Test real database constraints and behavior
   - Clean database state between tests

3. **Benchmarks** (`Tests.Benchmark/`)
   - Measure performance of critical operations
   - Identify performance bottlenecks
   - Track performance changes over time

### Running Unit Tests

```bash
# Run all unit tests
dotnet test Tests/Tests.csproj

# Run with coverage report
dotnet test Tests/Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=lcov
```

### Running Integration Tests

```bash
# Run all integration tests
dotnet test Tests.Integration/Tests.Integration.csproj

# Run with coverage report
dotnet test Tests.Integration/Tests.Integration.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=lcov
```

Note: Integration tests require SQL Server LocalDB to be installed. The tests will:
- Create a separate test database
- Use transactions to isolate test data
- Clean up after each test run
- Test real database behavior and constraints

### Running Tests with Coverage

1. Install ReportGenerator (if not already installed):
```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

2. Run tests with coverage:
```bash
# Run all tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov

# Generate HTML report
reportgenerator -reports:"**/coverage.info" -targetdir:"coveragereport" -reporttypes:Html
```

3. View the coverage report:
- Open `coveragereport/index.html` in your browser
- The report shows:
  - Line coverage
  - Branch coverage
  - Method coverage
  - Coverage by project and file
  - Uncovered lines and branches

4. Coverage thresholds (optional):
```bash
# Run tests with minimum coverage threshold
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:Threshold=80
```

### Running Benchmarks

```bash
# Run all benchmarks
cd Tests.Benchmark
dotnet run -c Release
```

The benchmarks will measure the performance of:
- Social security number validation
- Various validation scenarios (valid, invalid, empty, null, etc.)
- Memory allocation during validation

### Testing the API

You can test the API endpoints using Swagger UI, available at:
- https://localhost:5001/swagger
- http://localhost:5000/swagger

Key endpoints:
- `GET /api/authors` - List all authors
- `POST /api/authors` - Create a new author
- `GET /api/posts` - List all posts
- `POST /api/posts` - Create a new post

## Project Structure Details

### BlogCore
- Contains the core business logic and domain entities
- Defines ports (interfaces) for both primary (use cases) and secondary (repositories) adapters
- Implements use cases that orchestrate the business logic

### BlogInfra
- Implements the secondary ports (repositories)
- Handles database operations using Entity Framework Core
- Contains database context and migrations

### BlogApi
- Implements the primary ports as REST API endpoints
- Handles HTTP requests and responses
- Provides Swagger documentation

### Tests
- Unit tests for domain entities and use cases
- Integration tests for the complete system
- Performance benchmarks for critical operations
- Uses in-memory database for testing

## Architecture Benefits

1. **Independence of Frameworks**: The core business logic is independent of any external frameworks
2. **Testability**: Business rules can be tested without UI, database, or external elements
3. **Independence of UI**: The UI can be changed without changing the business logic
4. **Independence of Database**: The database can be changed without affecting the business logic
5. **Independence of External Agency**: The business rules don't know about the outside world

## Notes for Recruiters

When evaluating this code:

1. Look at the clean separation of concerns in the architecture
2. Examine the test coverage and test organization
3. Check the domain model implementation and business rules
4. Review the repository pattern implementation
5. Consider the maintainability and extensibility of the code
6. Note the performance benchmarking of critical operations

The code demonstrates:
- SOLID principles
- Clean Architecture practices
- Test-Driven Development
- Domain-Driven Design concepts
- Performance optimization awareness
- Modern .NET development practices 