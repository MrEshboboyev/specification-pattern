using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification.Advanced;

/// <summary>
/// Specification that validates entities and collects validation errors
/// </summary>
/// <typeparam name="T">The type of entity to which the specification applies</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="ValidationSpecification{T}"/> class
/// </remarks>
/// <param name="specification">The specification to validate</param>
/// <param name="errorMessage">The error message when validation fails</param>
public class ValidationSpecification<T>(
    ISpecification<T> specification,
    string errorMessage
) : SpecificationBase<T>
{
    private readonly ISpecification<T> _specification = specification 
        ?? throw new ArgumentNullException(nameof(specification));
    private readonly string _errorMessage = errorMessage 
        ?? throw new ArgumentNullException(nameof(errorMessage));

    /// <summary>
    /// Builds the expression for this specification
    /// </summary>
    /// <returns>The expression tree for this specification</returns>
    protected override Expression<Func<T, bool>> BuildExpression()
    {
        return _specification.AsExpression();
    }

    /// <summary>
    /// Validates an entity and collects validation results
    /// </summary>
    /// <param name="entity">The entity to validate</param>
    /// <param name="errors">Collection to store validation errors</param>
    /// <returns>True if the entity satisfies the specification, false otherwise</returns>
    public bool Validate(T entity, ICollection<string> errors)
    {
        if (entity == null) 
            throw new ArgumentNullException(nameof(entity));

        ArgumentNullException.ThrowIfNull(errors);

        var result = IsSatisfiedBy(entity);
        if (!result)
        {
            errors.Add(_errorMessage);
        }
        return result;
    }
}
