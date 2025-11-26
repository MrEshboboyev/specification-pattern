using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification.Advanced;

/// <summary>
/// Base class for visiting and analyzing specification expressions
/// </summary>
public abstract class SpecificationVisitor : ExpressionVisitor
{
    /// <summary>
    /// Visits a specification's expression
    /// </summary>
    /// <typeparam name="T">The type of entity to which the specification applies</typeparam>
    /// <param name="specification">The specification to visit</param>
    public virtual void Visit<T>(ISpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(specification);

        Visit(specification.AsExpression());
    }

    /// <summary>
    /// Visits a binary expression in a specification
    /// </summary>
    /// <param name="node">The binary expression node</param>
    /// <returns>The visited expression</returns>
    protected override Expression VisitBinary(BinaryExpression node)
    {
        // Handle AndAlso and OrElse operations
        if (node.NodeType == ExpressionType.AndAlso || node.NodeType == ExpressionType.OrElse)
        {
            OnVisitLogicalOperation(node.NodeType, node.Left, node.Right);
        }

        return base.VisitBinary(node);
    }

    /// <summary>
    /// Visits a unary expression in a specification
    /// </summary>
    /// <param name="node">The unary expression node</param>
    /// <returns>The visited expression</returns>
    protected override Expression VisitUnary(UnaryExpression node)
    {
        // Handle Not operations
        if (node.NodeType == ExpressionType.Not)
        {
            OnVisitNotOperation(node.Operand);
        }

        return base.VisitUnary(node);
    }

    /// <summary>
    /// Called when a logical operation (AND/OR) is visited
    /// </summary>
    /// <param name="operationType">The type of logical operation</param>
    /// <param name="left">The left operand</param>
    /// <param name="right">The right operand</param>
    protected virtual void OnVisitLogicalOperation(
        ExpressionType operationType, 
        Expression left, 
        Expression right)
    {
        // Override in derived classes to handle logical operations
    }

    /// <summary>
    /// Called when a NOT operation is visited
    /// </summary>
    /// <param name="operand">The operand of the NOT operation</param>
    protected virtual void OnVisitNotOperation(Expression operand)
    {
        // Override in derived classes to handle NOT operations
    }
}
