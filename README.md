# Specification Pattern in Practice

This project demonstrates an advanced implementation of the Specification Pattern in C#, showcasing both a basic implementation and an enhanced version with enterprise-grade features.

## Overview

The Specification Pattern is a behavioral design pattern that allows you to define business rules or criteria that can be combined using boolean operators (AND, OR, NOT) to create complex business logic. This implementation provides both a simple version and an advanced version with additional features.

## Features

### Basic Specification Pattern
- Core interfaces and base classes
- AND, OR, and NOT composition
- Expression-based specifications
- Extension methods for fluent API

### Advanced Specification Pattern
- **Performance Optimizations**: Parameter rebinding and expression caching
- **Asynchronous Evaluation**: Non-blocking specification evaluation
- **Specification Builder**: Fluent API for creating complex specifications
- **Specification Visitor**: Pattern for analyzing and processing specifications
- **Validation Framework**: Built-in validation with error collection
- **Caching Mechanism**: Performance improvement through result caching
- **Operation Analysis**: Count and analyze operations in complex specifications

## Key Components

### Core Interfaces
- `ISpecification<T>`: Base interface for all specifications

### Basic Implementation
- `SpecificationBase<T>`: Abstract base class for specifications
- `AndSpecification<T>`: Combines two specifications with AND logic
- `OrSpecification<T>`: Combines two specifications with OR logic
- `NotSpecification<T>`: Negates a specification

### Advanced Implementation
- `SpecificationBase<T>`: Enhanced base class with caching and async support
- `AndSpecification<T>`: Optimized AND implementation with parameter rebinding
- `OrSpecification<T>`: Optimized OR implementation with parameter rebinding
- `NotSpecification<T>`: NOT implementation
- `CachedSpecification<T>`: Wrapper for caching specification results
- `SpecificationBuilder<T>`: Fluent API for building complex specifications
- `SpecificationVisitor`: Base class for visiting and analyzing specifications
- `OperationCounterVisitor`: Example visitor that counts operations
- `ValidationSpecification<T>`: Specification with validation capabilities
- `CompositeValidationSpecification<T>`: Validates multiple specifications

## Usage Examples

### Basic Usage
```csharp
var activeAccountSpec = new ActiveAccountSpecification();
var highBalanceSpec = new AccountAmountSpecification(1000);
var compositeSpec = activeAccountSpec.And(highBalanceSpec);

if (compositeSpec.IsSatisfiedBy(account))
{
    // Account is active and has high balance
}
```

### Advanced Usage
```csharp
// Using the specification builder
var spec = SpecificationBuilder<Account>
    .Create(a => a.IsActive)
    .And(a => a.Amount > 1000)
    .Build();

// Async evaluation
var result = await spec.IsSatisfiedByAsync(account);

// Operation analysis
var visitor = new OperationCounterVisitor();
visitor.Visit(spec);
Console.WriteLine($"AND operations: {visitor.AndCount}");
```

## Running the Project

To run the demonstration:

```bash
dotnet run
```

This will show examples of both the basic and advanced specification patterns in action.

## Running Tests

To run the unit tests:

```bash
dotnet test
```

## Benefits of the Advanced Implementation

1. **Performance**: Optimized expression building and caching mechanisms
2. **Flexibility**: Rich API for composing complex business rules
3. **Maintainability**: Clean separation of concerns and reusable components
4. **Extensibility**: Easy to add new operations and analysis tools
5. **Enterprise Ready**: Includes validation, async support, and diagnostic capabilities

## License

This project is open source and available under the MIT License.