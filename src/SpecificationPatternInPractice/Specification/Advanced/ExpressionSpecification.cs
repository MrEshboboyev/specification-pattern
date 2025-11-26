using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification.Advanced;

/// <summary>
/// Specification implementation based on an expression
/// </summary>
/// <typeparam name="T">The type of entity to which the specification applies</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="ExpressionSpecification{T}"/> class
/// </remarks>
/// <param name="expression">The expression for this specification</param>
public class ExpressionSpecification<T>(
    Expression<Func<T, bool>> expression
) : SpecificationBase<T>
{
    private readonly Expression<Func<T, bool>> _expression = expression 
        ?? throw new ArgumentNullException(nameof(expression));

    /// <summary>
    /// Builds the expression for this specification
    /// </summary>
    /// <returns>The expression tree for this specification</returns>
    protected override Expression<Func<T, bool>> BuildExpression()
    {
        return _expression;
    }
}
