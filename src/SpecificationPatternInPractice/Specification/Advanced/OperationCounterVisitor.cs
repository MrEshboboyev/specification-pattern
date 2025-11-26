using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification.Advanced;

/// <summary>
/// Visitor that counts operations in a specification
/// </summary>
public class OperationCounterVisitor : SpecificationVisitor
{
    /// <summary>
    /// Gets the count of AND operations
    /// </summary>
    public int AndCount { get; private set; }

    /// <summary>
    /// Gets the count of OR operations
    /// </summary>
    public int OrCount { get; private set; }

    /// <summary>
    /// Gets the count of NOT operations
    /// </summary>
    public int NotCount { get; private set; }

    /// <summary>
    /// Gets the total count of operations
    /// </summary>
    public int TotalCount => AndCount + OrCount + NotCount;

    /// <summary>
    /// Resets all counters
    /// </summary>
    public void Reset()
    {
        AndCount = 0;
        OrCount = 0;
        NotCount = 0;
    }

    /// <summary>
    /// Called when a logical operation (AND/OR) is visited
    /// </summary>
    /// <param name="operationType">The type of logical operation</param>
    /// <param name="left">The left operand</param>
    /// <param name="right">The right operand</param>
    protected override void OnVisitLogicalOperation(ExpressionType operationType, Expression left, Expression right)
    {
        if (operationType == ExpressionType.AndAlso)
        {
            AndCount++;
        }
        else if (operationType == ExpressionType.OrElse)
        {
            OrCount++;
        }
    }

    /// <summary>
    /// Called when a NOT operation is visited
    /// </summary>
    /// <param name="operand">The operand of the NOT operation</param>
    protected override void OnVisitNotOperation(Expression operand)
    {
        NotCount++;
    }
}
