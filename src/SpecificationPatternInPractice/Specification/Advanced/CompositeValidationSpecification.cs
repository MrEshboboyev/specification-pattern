using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification.Advanced;

/// <summary>
/// Composite specification that validates multiple specifications and collects all errors
/// </summary>
/// <typeparam name="T">The type of entity to which the specification applies</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="CompositeValidationSpecification{T}"/> class
/// </remarks>
/// <param name="specifications">The specifications to validate</param>
public class CompositeValidationSpecification<T>(
    IEnumerable<ISpecification<T>> specifications
) : SpecificationBase<T>
{
    private readonly IEnumerable<ISpecification<T>> _specifications = specifications 
        ?? throw new ArgumentNullException(nameof(specifications));

    /// <summary>
    /// Builds the expression for this specification
    /// </summary>
    /// <returns>The expression tree for this specification</returns>
    protected override Expression<Func<T, bool>> BuildExpression()
    {
        // This is a composite validation specification, so we don't have a single expression
        // Instead, we'll create an expression that always returns true
        return entity => true;
    }

    /// <summary>
    /// Validates an entity against all specifications and collects validation results
    /// </summary>
    /// <param name="entity">The entity to validate</param>
    /// <returns>Collection of validation errors</returns>
    public ICollection<string> Validate(T entity)
    {
        if (entity == null) 
            throw new ArgumentNullException(nameof(entity));

        var errors = new List<string>();
        
        foreach (var specification in _specifications)
        {
            // If it's a validation specification, use its Validate method
            if (specification is ValidationSpecification<T> validationSpec)
            {
                validationSpec.Validate(entity, errors);
            }
            else
            {
                // For regular specifications, just check if satisfied
                if (!specification.IsSatisfiedBy(entity))
                {
                    errors.Add($"Specification {specification.GetType().Name} was not satisfied");
                }
            }
        }
        
        return errors;
    }

    /// <summary>
    /// Evaluates whether the entity satisfies all specifications
    /// </summary>
    /// <param name="entity">The entity to evaluate</param>
    /// <returns>True if the entity satisfies all specifications, false otherwise</returns>
    public override bool IsSatisfiedBy(T entity)
    {
        if (entity == null) 
            throw new ArgumentNullException(nameof(entity));

        return _specifications.All(spec => spec.IsSatisfiedBy(entity));
    }
}
