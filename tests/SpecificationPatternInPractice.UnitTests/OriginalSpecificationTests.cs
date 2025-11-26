using SpecificationPatternInPractice.Specification;
using System.Linq.Expressions;

namespace SpecificationPatternInPractice.UnitTests;

public class OriginalSpecificationTests
{
    [Fact]
    public void AndSpecification_ShouldEvaluateCorrectly()
    {
        // Arrange
        var spec1 = new TestSpecification(e => e.Value > 5);
        var spec2 = new TestSpecification(e => e.Name != null);
        var andSpec = new AndSpecification<TestEntity>(spec1, spec2);
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
        var spec1 = new TestSpecification(e => e.Value > 5);
        var spec2 = new TestSpecification(e => e.Name != null);
        var andSpec = new AndSpecification<TestEntity>(spec1, spec2);
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
        var spec1 = new TestSpecification(e => e.Value > 5);
        var spec2 = new TestSpecification(e => e.Name == "Test");
        var orSpec = new OrSpecification<TestEntity>(spec1, spec2);
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
        var spec1 = new TestSpecification(e => e.Value > 5);
        var spec2 = new TestSpecification(e => e.Name == "Test");
        var orSpec = new OrSpecification<TestEntity>(spec1, spec2);
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
        var baseSpec = new TestSpecification(e => e.Value > 5);
        var notSpec = new NotSpecification<TestEntity>(baseSpec);
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
        var baseSpec = new TestSpecification(e => e.Value > 5);
        var notSpec = new NotSpecification<TestEntity>(baseSpec);
        var entity = new TestEntity { Value = 3 };

        // Act
        var result = await notSpec.IsSatisfiedByAsync(entity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void SpecificationExtensions_ShouldWork()
    {
        // Arrange
        var spec1 = new TestSpecification(e => e.Value > 5);
        var spec2 = new TestSpecification(e => e.Name != null);

        // Act
        var andSpec = spec1.And(spec2);
        var orSpec = spec1.Or(spec2);
        var notSpec = spec1.Not();
        
        var entity = new TestEntity { Value = 10, Name = "Test" };

        // Assert
        Assert.True(andSpec.IsSatisfiedBy(entity));
        Assert.True(orSpec.IsSatisfiedBy(entity));
        Assert.False(notSpec.IsSatisfiedBy(entity));
    }

    private class TestSpecification(
        Expression<Func<TestEntity, bool>> expression
    ) : SpecificationBase<TestEntity>
    {
        public override Expression<Func<TestEntity, bool>> AsExpression()
        {
            return expression;
        }
    }

    private class TestEntity
    {
        public int Value { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }
}
