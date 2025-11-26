using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification.Advanced;

/// <summary>
/// Represents the logical disjunction of two specifications
/// </summary>
/// <typeparam name="T">The type of entity to which the specification applies</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="OrSpecification{T}"/> class
/// </remarks>
/// <param name="left">The left specification</param>
/// <param name="right">The right specification</param>
public class OrSpecification<T>(
    ISpecification<T> left, 
    ISpecification<T> right
) : SpecificationBase<T>
{
    private readonly ISpecification<T> _left = left 
        ?? throw new ArgumentNullException(nameof(left));
    private readonly ISpecification<T> _right = right 
        ?? throw new ArgumentNullException(nameof(right));

    /// <summary>
    /// Builds the expression for this specification with parameter rebinding optimization
    /// </summary>
    /// <returns>The expression tree for this specification</returns>
    protected override Expression<Func<T, bool>> BuildExpression()
    {
        var leftExpression = _left.AsExpression();
        var rightExpression = _right.AsExpression();

        // Rebind parameters to ensure they are the same instance
        var parameter = leftExpression.Parameters[0];
        var rightBody = ParameterRebinder.ReplaceParameters(rightExpression.Parameters[0], parameter, rightExpression.Body);

        var body = Expression.OrElse(leftExpression.Body, rightBody);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
