using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
    Expression<Func<T, bool>> AsExpression();
}
