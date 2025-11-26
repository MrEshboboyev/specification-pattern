using SpecificationPatternInPractice.Specification.Advanced;
using System.Linq.Expressions;

namespace SpecificationPatternInPractice.UnitTests;

public class AdvancedSpecificationTests
{
    [Fact]
    public void ExpressionSpecification_ShouldEvaluateCorrectly()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expression = e => e.Value > 5;
        var specification = new ExpressionSpecification<TestEntity>(expression);
        var entity = new TestEntity { Value = 10 };

        // Act
        var result = specification.IsSatisfiedBy(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExpressionSpecification_ShouldEvaluateAsyncCorrectly()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expression = e => e.Value > 5;
        var specification = new ExpressionSpecification<TestEntity>(expression);
        var entity = new TestEntity { Value = 10 };

        // Act
        var result = await specification.IsSatisfiedByAsync(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AndSpecification_ShouldEvaluateCorrectly()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expr1 = e => e.Value > 5;
        Expression<Func<TestEntity, bool>> expr2 = e => e.Name != null;
        var spec1 = new ExpressionSpecification<TestEntity>(expr1);
        var spec2 = new ExpressionSpecification<TestEntity>(expr2);
        var andSpec = spec1.And(spec2);
        var entity = new TestEntity { Value = 10, Name = "Test" };

        // Act
        var result = andSpec.IsSatisfiedBy(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AndSpecification_ShouldEvaluateAsyncCorrectly()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expr1 = e => e.Value > 5;
        Expression<Func<TestEntity, bool>> expr2 = e => e.Name != null;
        var spec1 = new ExpressionSpecification<TestEntity>(expr1);
        var spec2 = new ExpressionSpecification<TestEntity>(expr2);
        var andSpec = spec1.And(spec2);
        var entity = new TestEntity { Value = 10, Name = "Test" };

        // Act
        var result = await andSpec.IsSatisfiedByAsync(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OrSpecification_ShouldEvaluateCorrectly()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expr1 = e => e.Value > 5;
        Expression<Func<TestEntity, bool>> expr2 = e => e.Name == "Test";
        var spec1 = new ExpressionSpecification<TestEntity>(expr1);
        var spec2 = new ExpressionSpecification<TestEntity>(expr2);
        var orSpec = spec1.Or(spec2);
        var entity = new TestEntity { Value = 3, Name = "Test" };

        // Act
        var result = orSpec.IsSatisfiedBy(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task OrSpecification_ShouldEvaluateAsyncCorrectly()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expr1 = e => e.Value > 5;
        Expression<Func<TestEntity, bool>> expr2 = e => e.Name == "Test";
        var spec1 = new ExpressionSpecification<TestEntity>(expr1);
        var spec2 = new ExpressionSpecification<TestEntity>(expr2);
        var orSpec = spec1.Or(spec2);
        var entity = new TestEntity { Value = 3, Name = "Test" };

        // Act
        var result = await orSpec.IsSatisfiedByAsync(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void NotSpecification_ShouldEvaluateCorrectly()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expression = e => e.Value > 5;
        var baseSpec = new ExpressionSpecification<TestEntity>(expression);
        var notSpec = baseSpec.Not();
        var entity = new TestEntity { Value = 3 };

        // Act
        var result = notSpec.IsSatisfiedBy(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task NotSpecification_ShouldEvaluateAsyncCorrectly()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expression = e => e.Value > 5;
        var baseSpec = new ExpressionSpecification<TestEntity>(expression);
        var notSpec = baseSpec.Not();
        var entity = new TestEntity { Value = 3 };

        // Act
        var result = await notSpec.IsSatisfiedByAsync(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void SpecificationBuilder_ShouldCreateComplexSpecification()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expr1 = e => e.Value > 5;
        Expression<Func<TestEntity, bool>> expr2 = e => e.Name != null;
        Expression<Func<TestEntity, bool>> expr3 = e => e.IsActive;

        // Act
        var specification = SpecificationBuilder<TestEntity>
            .Create(expr1)
            .And(new ExpressionSpecification<TestEntity>(expr2))
            .Or(new ExpressionSpecification<TestEntity>(expr3))
            .Build();

        var entity = new TestEntity { Value = 3, Name = "Test", IsActive = true };

        // Assert
        var result = specification.IsSatisfiedBy(entity);
        Assert.True(result);
    }

    [Fact]
    public void CachedSpecification_ShouldCacheResults()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expression = e => e.Value > 5;
        var baseSpec = new ExpressionSpecification<TestEntity>(expression);
        var cachedSpec = new CachedSpecification<TestEntity>(baseSpec, e => e.Value.GetHashCode());
        var entity = new TestEntity { Value = 10 };

        // Act
        var result1 = cachedSpec.IsSatisfiedBy(entity);
        var result2 = cachedSpec.IsSatisfiedBy(entity);

        // Assert
        Assert.True(result1);
        Assert.True(result2);
        Assert.Equal(1, cachedSpec.CacheCount);
    }

    [Fact]
    public void OperationCounterVisitor_ShouldCountOperations()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expr1 = e => e.Value > 5;
        Expression<Func<TestEntity, bool>> expr2 = e => e.Name != null;
        Expression<Func<TestEntity, bool>> expr3 = e => e.IsActive;
        
        var spec1 = new ExpressionSpecification<TestEntity>(expr1);
        var spec2 = new ExpressionSpecification<TestEntity>(expr2);
        var spec3 = new ExpressionSpecification<TestEntity>(expr3);
        
        var complexSpec = spec1.And(spec2).Or(spec3.Not());
        var visitor = new OperationCounterVisitor();

        // Act
        visitor.Visit(complexSpec);

        // Assert
        Assert.Equal(1, visitor.AndCount);
        Assert.Equal(1, visitor.OrCount);
        Assert.Equal(1, visitor.NotCount);
        Assert.Equal(3, visitor.TotalCount);
    }

    [Fact]
    public void CompositeValidationSpecification_ShouldCollectErrors()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expr1 = e => e.Value > 5;
        Expression<Func<TestEntity, bool>> expr2 = e => e.Name != null;
        
        var spec1 = new ValidationSpecification<TestEntity>(
            new ExpressionSpecification<TestEntity>(expr1), 
            "Value must be greater than 5");
        var spec2 = new ValidationSpecification<TestEntity>(
            new ExpressionSpecification<TestEntity>(expr2), 
            "Name is required");
        
        var compositeSpec = new CompositeValidationSpecification<TestEntity>(new[] { spec1, spec2 });
        var entity = new TestEntity { Value = 3, Name = null };

        // Act
        var errors = compositeSpec.Validate(entity);

        // Assert
        Assert.Equal(2, errors.Count);
        Assert.Contains("Value must be greater than 5", errors);
        Assert.Contains("Name is required", errors);
    }

    [Fact]
    public void SpecificationExtensions_ShouldWorkWithExpressions()
    {
        // Arrange
        Expression<Func<TestEntity, bool>> expr1 = e => e.Value > 5;
        Expression<Func<TestEntity, bool>> expr2 = e => e.Name != null;

        // Act
        var specification = expr1.ToSpecification().And(expr2.ToSpecification());
        var entity = new TestEntity { Value = 10, Name = "Test" };

        // Assert
        var result = specification.IsSatisfiedBy(entity);
        Assert.True(result);
    }

    private class TestEntity
    {
        public int Value { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }
}
