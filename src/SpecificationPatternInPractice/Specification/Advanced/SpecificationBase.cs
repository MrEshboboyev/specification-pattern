using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification.Advanced;

/// <summary>
/// Advanced base implementation of the specification pattern with performance optimizations
/// </summary>
/// <typeparam name="T">The type of entity to which the specification applies</typeparam>
public abstract class SpecificationBase<T> : ISpecification<T>
{
    private Func<T, bool>? _compiledPredicate;
    private Expression<Func<T, bool>>? _expression;

    /// <summary>
    /// Gets the LINQ expression representing this specification
    /// </summary>
    /// <returns>The expression tree for this specification</returns>
    public virtual Expression<Func<T, bool>> AsExpression()
    {
        return _expression ??= BuildExpression();
    }

    /// <summary>
    /// Abstract method to build the expression for this specification
    /// </summary>
    /// <returns>The expression tree for this specification</returns>
    protected abstract Expression<Func<T, bool>> BuildExpression();

    /// <summary>
    /// Evaluates whether the entity satisfies this specification
    /// </summary>
    /// <param name="entity">The entity to evaluate</param>
    /// <returns>True if the entity satisfies the specification, false otherwise</returns>
    public virtual bool IsSatisfiedBy(T entity)
    {
        if (entity == null) 
            throw new ArgumentNullException(nameof(entity));

        return (_compiledPredicate ??= AsExpression().Compile())(entity);
    }

    /// <summary>
    /// Evaluates whether the entity satisfies this specification asynchronously
    /// </summary>
    /// <param name="entity">The entity to evaluate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the entity satisfies the specification, false otherwise</returns>
    public virtual async Task<bool> IsSatisfiedByAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null) 
            throw new ArgumentNullException(nameof(entity));

        return await Task.Run(() => IsSatisfiedBy(entity), cancellationToken);
    }

    /// <summary>
    /// Combines this specification with another using the AND operator
    /// </summary>
    /// <param name="other">The other specification</param>
    /// <returns>A new specification representing the conjunction</returns>
    public ISpecification<T> And(ISpecification<T> other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return new AndSpecification<T>(this, other);
    }

    /// <summary>
    /// Combines this specification with another using the OR operator
    /// </summary>
    /// <param name="other">The other specification</param>
    /// <returns>A new specification representing the disjunction</returns>
    public ISpecification<T> Or(ISpecification<T> other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return new OrSpecification<T>(this, other);
    }

    /// <summary>
    /// Negates this specification using the NOT operator
    /// </summary>
    /// <returns>A new specification representing the negation</returns>
    public ISpecification<T> Not()
    {
        return new NotSpecification<T>(this);
    }

    /// <summary>
    /// Checks if this specification is satisfied by all entities in a collection
    /// </summary>
    /// <param name="entities">The collection of entities</param>
    /// <returns>True if all entities satisfy the specification</returns>
    public bool All(IEnumerable<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        return entities.All(IsSatisfiedBy);
    }

    /// <summary>
    /// Checks if any entity in a collection satisfies this specification
    /// </summary>
    /// <param name="entities">The collection of entities</param>
    /// <returns>True if any entity satisfies the specification</returns>
    public bool Any(IEnumerable<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        return entities.Any(IsSatisfiedBy);
    }

    /// <summary>
    /// Filters a collection to only include entities that satisfy this specification
    /// </summary>
    /// <param name="entities">The collection of entities</param>
    /// <returns>A filtered collection</returns>
    public IEnumerable<T> Where(IEnumerable<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        return entities.Where(IsSatisfiedBy);
    }

    /// <summary>
    /// Gets the first entity that satisfies this specification
    /// </summary>
    /// <param name="entities">The collection of entities</param>
    /// <returns>The first entity that satisfies the specification</returns>
    public T? FirstOrDefault(IEnumerable<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        return entities.FirstOrDefault(IsSatisfiedBy);
    }

    /// <summary>
    /// Implicit conversion to Expression<Func<T, bool>>
    /// </summary>
    /// <param name="specification">The specification</param>
    public static implicit operator Expression<Func<T, bool>>(SpecificationBase<T> specification)
    {
        return specification.AsExpression();
    }
}
