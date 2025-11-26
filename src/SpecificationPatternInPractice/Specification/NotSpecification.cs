using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification;

/// <summary>
/// Represents the logical negation of a specification
/// </summary>
/// <typeparam name="T">The type of entity to which the specification applies</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="NotSpecification{T}"/> class
/// </remarks>
/// <param name="specification">The specification to negate</param>
public class NotSpecification<T>(
    ISpecification<T> specification
) : SpecificationBase<T>
{
    private readonly ISpecification<T> _specification = specification 
        ?? throw new ArgumentNullException(nameof(specification));

    /// <summary>
    /// Gets the LINQ expression representing this specification
    /// </summary>
    /// <returns>The expression tree for this specification</returns>
    public override Expression<Func<T, bool>> AsExpression()
    {
        var expression = _specification.AsExpression();
        var parameter = expression.Parameters[0];
        var body = Expression.Not(expression.Body);
        
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    /// <summary>
    /// Evaluates whether the entity satisfies this specification asynchronously
    /// </summary>
    /// <param name="entity">The entity to evaluate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the entity satisfies the specification, false otherwise</returns>
    public override async Task<bool> IsSatisfiedByAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        var result = await _specification.IsSatisfiedByAsync(entity, cancellationToken);
        return !result;
    }
}
