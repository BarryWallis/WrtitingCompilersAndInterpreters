# Copilot Instructions

## C# Development

### C# Instructions
- Always use the latest version C#, currently C# 14.
- Write clear and concise comments for each function.
- Always use explicit type names instead of `var` whenever possible.
- Always make all changes in-editor so Visual Studio shows keep/undo prompts.

### General Instructions
- Make only high confidence suggestions when reviewing code changes.
- Write code with good maintainability practices, including comments on why certain design decisions were made.
- Handle edge cases and write clear exception handling.
- For libraries or external dependencies, mention their usage and purpose in comments.
- Never make any changes to generated code unless explicitly directed. <!-- Added by BLW -->

## Naming Conventions

- Follow PascalCase for component names, method names, and public members.
- Use camelCase for private fields and local variables.
- Prefix private and protected class fields with an underscore. <!-- Updated by BLW: include protected fields -->
- Prefix interface names with "I" (e.g., IUserService).

## Formatting

- Apply code-formatting style defined in `.editorconfig`.
- Prefer file-scoped namespace declarations and single-line using directives.
- Insert a newline before the opening curly brace of any code block (e.g., after `if`, `for`, `while`, `foreach`, `using`, `try`, etc.).
- Ensure that the final return statement of a method is on its own line.
- Use pattern matching and switch expressions wherever possible.
- Use `nameof` instead of string literals when referring to member names.
- Ensure that XML doc comments are created for any public APIs. When applicable, include `<example>` and `<code>` documentation in the comments.

## Class Member Ordering

Order class members in the following sequence to group state-related members together and improve readability:
1. **Fields** (constants, static fields, instance fields)
2. **Properties** (auto-properties, computed properties, indexers)
3. **Constructors** (static constructors, instance constructors)
4. **Methods** (public methods, protected methods, private methods)
5. **Nested types** (nested classes, structs, enums, delegates)

## C# 14 Features

Leverage the latest C# 14 features where applicable:
- **Extension members** - Use implicit extensions for utility methods on types
- **Null-conditional assignment** - Simplify null-conditional operations
- **`nameof` with unbound generic types** - Use `nameof` with generic types without type parameters
- **Implicit `Span<T>` conversions** - Use spans for high-performance string/array processing
- **Modifiers on lambda parameters** - Use `ref`, `in`, `out` on lambda parameters when needed
- **`field` keyword** - Use `field` keyword in property accessors for backing field access
- **`partial` events and constructors** - Split large classes across multiple files
- **User-defined compound assignment operators** - Define custom `+=`, `-=`, etc. for types

## Project Setup and Structure

- Guide users through creating a new .NET project with the appropriate templates.
- Explain the purpose of each generated file and folder to build understanding of the project structure.
- Demonstrate how to organize code using feature folders or domain-driven design principles.
- Show proper separation of concerns with models, services, and data access layers.
- Explain the Program.cs and configuration system in ASP.NET Core including environment-specific settings.

## Nullable Reference Types

- Declare variables non-nullable, and check for `null` at entry points.
- Always use `is null` or `is not null` instead of `== null` or `!= null`.
- Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.
- Do not check for null arguments redundantly.

## Data Access Patterns

- Guide the implementation of a data access layer using Entity Framework Core.
- Explain different options (SQL Server, SQLite, In-Memory) for development and production.
- Demonstrate repository pattern implementation and when it's beneficial.
- Show how to implement database migrations and data seeding.
- Explain efficient query patterns to avoid common performance issues.

## Authentication and Authorization

- Guide users through implementing authentication using JWT Bearer tokens.
- Explain OAuth 2.0 and OpenID Connect concepts as they relate to ASP.NET Core.
- Show how to implement role-based and policy-based authorization.
- Demonstrate integration with Microsoft Entra ID (formerly Azure AD).
- Explain how to secure both controller-based and Minimal APIs consistently.

## Validation and Error Handling

- Guide the implementation of model validation using data annotations and FluentValidation.
- Explain the validation pipeline and how to customize validation responses.
- Demonstrate a global exception handling strategy using middleware.
- Show how to create consistent error responses across the API.
- Explain problem details (RFC 7807) implementation for standardized error responses.

## Validation Patterns for Records with Mutable Properties

### Record Validation Requirements

When creating records with mutable properties that have invariants:

1. **Validate in both constructor AND property setters**
   - Constructor validation alone is insufficient if properties have public setters
   - Property setters can bypass constructor validation after object creation

2. **Constructor should set backing fields directly**
   - Use `_field = value` instead of `this.property = value` in constructor
   - Avoids initialization order issues (e.g., validating against uninitialized fields)
   - Setting properties in constructor can fail validation due to default field values

3. **Extract duplicated error messages**
   - Use private static helper methods for error message generation
   - Ensures consistency between constructor and property validation
   - Example:
     ```csharp
     private static string MustBePositiveError(string paramName, BigInteger value)
         => $"{char.ToUpper(paramName[0])}{paramName.Substring(1)} value ({value}) must be positive.";
     ```

4. **Validation order matters**
   - Check simpler/faster validations first (e.g., non-null, positive)
   - Check cross-property validations last (e.g., start <= end)
   - Fail fast on clearly invalid values before more complex checks

### Testing Validation

When adding or changing validation rules:

1. **Test both constructor and property setters**
   - Don't assume constructor tests cover property setter behavior
   - Example: `ConstructorWithNegativeStartThrows` AND `SetStartToNegativeValueThrows`

2. **Search and update ALL related tests**
   - Check integration tests, not just unit tests
   - Use code search to find tests that might be affected by validation changes
   - Example: Changing from non-negative to positive affects tests in multiple test files

3. **Cover edge cases explicitly**
   - Boundary values (zero, negative, equal values)
   - Large values (BigInteger edge cases)
   - Both invalid cases (expect exception) and valid cases (expect success)

### Domain Constraints

Be explicit about mathematical/domain terminology:
- **Positive** means `> 0` (excludes zero)
- **Non-negative** means `>= 0` (includes zero)
- **Negative** means `< 0` (excludes zero)
- **Non-positive** means `<= 0` (includes zero)
- Document why constraints exist in comments or XML documentation

## API Versioning and Documentation

- Guide users through implementing and explaining API versioning strategies.
- Demonstrate Swagger/OpenAPI implementation with proper documentation.
- Show how to document endpoints, parameters, responses, and authentication.
- Explain versioning in both controller-based and Minimal APIs.
- Guide users on creating meaningful API documentation that helps consumers.

## Logging and Monitoring

- Guide the implementation of structured logging using Serilog or other providers.
- Explain the logging levels and when to use each.
- Demonstrate integration with Application Insights for telemetry collection.
- Show how to implement custom telemetry and correlation IDs for request tracking.
- Explain how to monitor API performance, errors, and usage patterns.

## Testing

- Always include test cases for critical paths of the application.
- Guide users through creating unit tests using xUnit, NUnit, or MSTest.
- Do not emit "Act", "Arrange" or "Assert" comments.
- Copy existing style in nearby files for test method names and capitalization.
- Use explicit type names instead of `var` in tests for clarity.
- Do not write tests for null argument validation when parameters are non-nullable reference types (the type system prevents null).
- Explain integration testing approaches for API endpoints.
- Demonstrate how to mock dependencies for effective testing.
- Show how to test authentication and authorization logic.
- Explain test-driven development principles as applied to API development.

## Performance Optimization

- Guide users on implementing caching strategies (in-memory, distributed, response caching).
- Explain asynchronous programming patterns and why they matter for API performance.
- Demonstrate pagination, filtering, and sorting for large data sets.
- Show how to implement compression and other performance optimizations.
- Explain how to measure and benchmark API performance.
- Use `Span<T>` and `ReadOnlySpan<T>` for high-performance scenarios.

## Deployment and DevOps

- Guide users through containerizing their API using .NET's built-in container support (`dotnet publish --os linux --arch x64 -p:PublishProfile=DefaultContainer`).
- Explain the differences between manual Dockerfile creation and .NET's container publishing features.
- Explain CI/CD pipelines for .NET applications.
- Demonstrate deployment to Azure App Service, Azure Container Apps, or other hosting options.
- Show how to implement health checks and readiness probes.
- Explain environment-specific configurations for different deployment stages.

---

# .NET MAUI

## .NET MAUI Code Style and Structure

- Write idiomatic and efficient .NET MAUI and C# code.
- Follow .NET and .NET MAUI conventions.
- Prefer inline functions for smaller components but separate complex logic into code-behind or service classes.
- Async/await should be used where applicable to ensure non-blocking UI operations.

## Naming Conventions

- Follow PascalCase for component names, method names, and public members.
- Use camelCase for private fields and local variables.
- Prefix interface names with "I" (e.g., IUserService).

## .NET MAUI and .NET Specific Guidelines

- Utilize .NET MAUI's built-in features for component lifecycle (e.g. OnAppearing, OnDisappearing).
- Use data binding effectively with {Binding}.
- Structure .NET MAUI components and services following Separation of Concerns.
- Always use the latest version C#, currently C# 14 features like record types, pattern matching, global usings, and extension members.

## Error Handling and Validation

- Implement proper error handling for .NET MAUI pages and API calls.
- Use logging for error tracking in the backend and consider capturing UI-level errors in MAUI with tools like MAUI Community Toolkit's Logger.
- Implement validation using FluentValidation or DataAnnotations in forms.

## MAUI API and Performance Optimization

- Utilize MAUI's built-in features for component lifecycle (e.g. OnAppearing, OnDisappearing).
- Use asynchronous methods (async/await) for API calls or UI actions that could block the main thread.
- Optimize MAUI components by reducing unnecessary renders and using OnPropertyChanged() efficiently.
- Minimize the component render tree by avoiding re-renders unless necessary, using BatchBegin() and BatchCommit() where appropriate.

## Caching Strategies

- Implement in-memory caching for frequently used data, especially for MAUI apps. Use IMemoryCache for lightweight caching solutions.
- Consider Distributed Cache strategies (like Redis or SQL Server Cache) for larger applications that need shared state across multiple users or clients.
- Cache API calls by storing responses to avoid redundant calls when data is unlikely to change, thus improving the user experience.

## State Management Libraries

- Use dependency injection and the .NET MAUI Community Toolkit for state sharing across components.

## API Design and Integration

- Use HttpClient or other appropriate services to communicate with external APIs or your own backend.
- Implement error handling for API calls using try-catch and provide proper user feedback in the UI.

## Testing and Debugging

- Test components and services using xUnit, NUnit, or MSTest.
- Use Moq or NSubstitute for mocking dependencies during tests.

## Security and Authentication

- Implement Authentication and Authorization in the MAUI app where necessary using OAuth or JWT tokens for API authentication.
- Use HTTPS for all web communication and ensure proper CORS policies are implemented.

## API Documentation and Swagger

- Use Swagger/OpenAPI for API documentation for your backend API services.
- Ensure XML documentation for models and API methods for enhancing Swagger documentation.
