using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification.Advanced;

/// <summary>
/// Builder class for creating specifications with a fluent API
/// </summary>
/// <typeparam name="T">The type of entity to which the specification applies</typeparam>
public class SpecificationBuilder<T>
{
    private ISpecification<T>? _specification;

    /// <summary>
    /// Creates a specification from a predicate expression
    /// </summary>
    /// <param name="predicate">The predicate expression</param>
    /// <returns>A specification builder</returns>
    public static SpecificationBuilder<T> Create(Expression<Func<T, bool>> predicate)
    {
        var builder = new SpecificationBuilder<T>
        {
            _specification = new ExpressionSpecification<T>(predicate)
        };

        return builder;
    }

    /// <summary>
    /// Combines the current specification with another using the AND operator
    /// </summary>
    /// <param name="other">The other specification</param>
    /// <returns>This specification builder</returns>
    public SpecificationBuilder<T> And(ISpecification<T> other)
    {
        if (_specification == null)
            throw new InvalidOperationException("No specification has been created yet.");

        _specification = _specification.And(other);
        return this;
    }

    /// <summary>
    /// Combines the current specification with another using the OR operator
    /// </summary>
    /// <param name="other">The other specification</param>
    /// <returns>This specification builder</returns>
    public SpecificationBuilder<T> Or(ISpecification<T> other)
    {
        if (_specification == null)
            throw new InvalidOperationException("No specification has been created yet.");

        _specification = _specification.Or(other);
        return this;
    }

    /// <summary>
    /// Negates the current specification using the NOT operator
    /// </summary>
    /// <returns>This specification builder</returns>
    public SpecificationBuilder<T> Not()
    {
        if (_specification == null)
            throw new InvalidOperationException("No specification has been created yet.");

        _specification = _specification.Not();
        return this;
    }

    /// <summary>
    /// Builds the final specification
    /// </summary>
    /// <returns>The built specification</returns>
    public ISpecification<T> Build()
    {
        if (_specification == null)
            throw new InvalidOperationException("No specification has been created yet.");

        return _specification;
    }
}
