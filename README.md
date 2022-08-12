# Introduction 
 This project is a RESTful API following the principles of Clean Architecture, make part of a product called Orms,works with Entity Framework and Dapper together.

 ## Technologies
* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [Dapper](https://github.com/DapperLib/Dapper)
* [FluentValidation](https://fluentvalidation.net/)
* [xUnit](https://xunit.net/), [FluentAssertions](https://fluentassertions.com/), & [Moq](https://github.com/moq)  

## Getting Started

The easiest way to get started is open the solution Orms.Api.sln  in **Visual Studio 2022**:

1. Install the latest [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
2. Navigate to `src/Orms.Api` and launch the project using `dotnet run` in **VS Code**
3. This project was built in **VS 2022**, it is recommended that this is the IDE for running the project

### Database Configuration
The database used in the project is SQLServer, every data model is ready through EF Core migrations, to deploy the database with the initial data just run the Package Manager Console from Visual Studio 2022

```json
 update-database
```

Startup project will be **Orms.Api**, in the Package Manager Console the Default project will be  **Orms.Persistence**

Verify that the **DefaultConnection** connection string within **appsettings.json** points to a valid SQL Server instance. 

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.


### Tests
To run the tests, just open the solution in Visual Studio 2022 and open the Test Explorer tab

## Overview

### Orms.Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Orms.Persistence

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.


### Orms.Api

 This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Program.cs* should reference Infrastructure.


