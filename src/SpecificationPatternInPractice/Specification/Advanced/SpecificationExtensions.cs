using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification.Advanced;

/// <summary>
/// Extension methods for specifications
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// Combines two specifications using the AND operator
    /// </summary>
    /// <typeparam name="T">The type of entity to which the specifications apply</typeparam>
    /// <param name="left">The left specification</param>
    /// <param name="right">The right specification</param>
    /// <returns>A new specification representing the conjunction</returns>
    public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new AndSpecification<T>(left, right);
    }

    /// <summary>
    /// Combines two specifications using the OR operator
    /// </summary>
    /// <typeparam name="T">The type of entity to which the specifications apply</typeparam>
    /// <param name="left">The left specification</param>
    /// <param name="right">The right specification</param>
    /// <returns>A new specification representing the disjunction</returns>
    public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new OrSpecification<T>(left, right);
    }

    /// <summary>
    /// Negates a specification using the NOT operator
    /// </summary>
    /// <typeparam name="T">The type of entity to which the specification applies</typeparam>
    /// <param name="specification">The specification to negate</param>
    /// <returns>A new specification representing the negation</returns>
    public static ISpecification<T> Not<T>(this ISpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(specification);

        return new NotSpecification<T>(specification);
    }

    /// <summary>
    /// Creates a specification from an expression
    /// </summary>
    /// <typeparam name="T">The type of entity to which the specification applies</typeparam>
    /// <param name="expression">The expression for the specification</param>
    /// <returns>A new specification based on the expression</returns>
    public static ISpecification<T> ToSpecification<T>(this Expression<Func<T, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return new ExpressionSpecification<T>(expression);
    }

    /// <summary>
    /// Evaluates whether the entity satisfies the specification
    /// </summary>
    /// <typeparam name="T">The type of entity to which the specification applies</typeparam>
    /// <param name="specification">The specification to evaluate</param>
    /// <param name="entity">The entity to evaluate</param>
    /// <returns>True if the entity satisfies the specification, false otherwise</returns>
    public static bool IsSatisfiedBy<T>(this ISpecification<T> specification, T entity)
    {
        ArgumentNullException.ThrowIfNull(specification);

        return specification.IsSatisfiedBy(entity);
    }

    /// <summary>
    /// Evaluates whether the entity satisfies the specification asynchronously
    /// </summary>
    /// <typeparam name="T">The type of entity to which the specification applies</typeparam>
    /// <param name="specification">The specification to evaluate</param>
    /// <param name="entity">The entity to evaluate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the entity satisfies the specification, false otherwise</returns>
    public static Task<bool> IsSatisfiedByAsync<T>(this ISpecification<T> specification, T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(specification);

        return specification.IsSatisfiedByAsync(entity, cancellationToken);
    }
}
