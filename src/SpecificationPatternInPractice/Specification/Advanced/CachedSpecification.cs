using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification.Advanced;

/// <summary>
/// Specification wrapper that adds caching for improved performance
/// </summary>
/// <typeparam name="T">The type of entity to which the specification applies</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="CachedSpecification{T}"/> class
/// </remarks>
/// <param name="specification">The specification to cache</param>
/// <param name="keySelector">Function to generate cache keys from entities</param>
public class CachedSpecification<T>(
    ISpecification<T> specification, 
    Func<T, int> keySelector
) : SpecificationBase<T>
{
    private readonly ISpecification<T> _specification = specification 
        ?? throw new ArgumentNullException(nameof(specification));
    private readonly ConcurrentDictionary<int, bool> _cache = new();
    private readonly Func<T, int> _keySelector = keySelector 
        ?? throw new ArgumentNullException(nameof(keySelector));

    /// <summary>
    /// Builds the expression for this specification
    /// </summary>
    /// <returns>The expression tree for this specification</returns>
    protected override Expression<Func<T, bool>> BuildExpression()
    {
        return _specification.AsExpression();
    }

    /// <summary>
    /// Evaluates whether the entity satisfies this specification with caching
    /// </summary>
    /// <param name="entity">The entity to evaluate</param>
    /// <returns>True if the entity satisfies the specification, false otherwise</returns>
    public override bool IsSatisfiedBy(T entity)
    {
        if (entity == null) 
            throw new ArgumentNullException(nameof(entity));

        var key = _keySelector(entity);
        
        if (_cache.TryGetValue(key, out var cachedResult))
        {
            return cachedResult;
        }

        var result = _specification.IsSatisfiedBy(entity);
        _cache.TryAdd(key, result);
        return result;
    }

    /// <summary>
    /// Clears the cache
    /// </summary>
    public void ClearCache()
    {
        _cache.Clear();
    }

    /// <summary>
    /// Gets the number of items in the cache
    /// </summary>
    public int CacheCount => _cache.Count;
}
