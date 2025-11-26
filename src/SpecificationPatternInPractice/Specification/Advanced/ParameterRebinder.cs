using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification.Advanced;

/// <summary>
/// Utility class for rebinding parameters in expression trees
/// </summary>
internal sealed class ParameterRebinder : ExpressionVisitor
{
    private readonly ParameterExpression _oldParameter;
    private readonly ParameterExpression _newParameter;

    private ParameterRebinder(ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        _oldParameter = oldParameter;
        _newParameter = newParameter;
    }

    /// <summary>
    /// Replaces parameters in an expression tree
    /// </summary>
    /// <param name="oldParameter">The parameter to replace</param>
    /// <param name="newParameter">The replacement parameter</param>
    /// <param name="expression">The expression tree</param>
    /// <returns>The expression tree with replaced parameters</returns>
    public static Expression ReplaceParameters(ParameterExpression oldParameter, ParameterExpression newParameter, Expression expression)
    {
        var rebinder = new ParameterRebinder(oldParameter, newParameter);
        return rebinder.Visit(expression)!;
    }

    /// <summary>
    /// Visits and replaces parameter expressions
    /// </summary>
    /// <param name="node">The parameter expression node</param>
    /// <returns>The replaced parameter expression</returns>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == _oldParameter ? _newParameter : base.VisitParameter(node);
    }
}
