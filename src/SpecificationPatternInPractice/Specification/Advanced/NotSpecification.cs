using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification.Advanced;

/// <summary>
/// Represents the logical negation of a specification
/// </summary>
/// <typeparam name="T">The type of entity to which the specification applies</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="NotSpecification{T}"/> class
/// </remarks>
/// <param name="specification">The specification to negate</param>
public class NotSpecification<T>(ISpecification<T> specification) : SpecificationBase<T>
{
    private readonly ISpecification<T> _specification = specification 
        ?? throw new ArgumentNullException(nameof(specification));

    /// <summary>
    /// Builds the expression for this specification
    /// </summary>
    /// <returns>The expression tree for this specification</returns>
    protected override Expression<Func<T, bool>> BuildExpression()
    {
        var expression = _specification.AsExpression();
        var parameter = expression.Parameters[0];
        var body = Expression.Not(expression.Body);
        
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
