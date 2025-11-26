using SpecificationPatternInPractice.Specification.Advanced;
using System.Linq.Expressions;

// Demonstrate the original specification pattern
Console.WriteLine("=== Original Specification Pattern ===");
var account = Account.Create(true, 4000);
var minAmount = 3000;

var spec = SpecificationPatternInPractice.Specification.SpecificationExtensions.And(
    new ActiveAccountSpecification(), 
    new AccountAmountSpecification(minAmount));

if (spec.IsSatisfiedBy(account)) Console.WriteLine("Valid");
else Console.WriteLine("Invalid");

// Demonstrate the enhanced specification pattern
Console.WriteLine("\n=== Enhanced Specification Pattern ===");

// Using specification builder
Expression<Func<Account, bool>> activeExpr = a => a.IsActive;
Expression<Func<Account, bool>> amountExpr = a => a.Amount > minAmount;

var enhancedSpec = SpecificationBuilder<Account>
    .Create(activeExpr)
    .And(amountExpr.ToSpecification())
    .Build();

if (enhancedSpec.IsSatisfiedBy(account)) Console.WriteLine("Enhanced Valid");
else Console.WriteLine("Enhanced Invalid");

// Demonstrate async evaluation
Console.WriteLine("\n=== Async Evaluation ===");
var asyncResult = await enhancedSpec.IsSatisfiedByAsync(account);
Console.WriteLine($"Async result: {asyncResult}");

// Demonstrate specification composition with Not
Console.WriteLine("\n=== Specification Composition ===");
var notActiveSpec = SpecificationPatternInPractice.Specification.SpecificationExtensions.Not(
    new ActiveAccountSpecification());
var account2 = Account.Create(false, 4000);

if (notActiveSpec.IsSatisfiedBy(account2)) Console.WriteLine("Account is inactive");
else Console.WriteLine("Account is active");

// Demonstrate operation counting
Console.WriteLine("\n=== Operation Analysis ===");
var complexSpec = SpecificationPatternInPractice.Specification.SpecificationExtensions.Or(
    SpecificationPatternInPractice.Specification.SpecificationExtensions.And(
        new ActiveAccountSpecification(), 
        new AccountAmountSpecification(minAmount)), 
    notActiveSpec);

var visitor = new OperationCounterVisitor();
visitor.Visit(complexSpec);
Console.WriteLine($"AND operations: {visitor.AndCount}");
Console.WriteLine($"OR operations: {visitor.OrCount}");
Console.WriteLine($"NOT operations: {visitor.NotCount}");

// Demonstrate validation
Console.WriteLine("\n=== Validation ===");
Expression<Func<Account, bool>> validAmountExpr = a => a.Amount >= 100;
var validationSpec = new ValidationSpecification<Account>(
    validAmountExpr.ToSpecification(),
    "Account amount must be at least 100");

var invalidAccount = Account.Create(true, 50);
var errors = new List<string>();
validationSpec.Validate(invalidAccount, errors);

if (errors.Any())
{
    Console.WriteLine("Validation errors:");
    foreach (var error in errors)
    {
        Console.WriteLine($"  - {error}");
    }
}

class ActiveAccountSpecification : SpecificationPatternInPractice.Specification.SpecificationBase<Account>
{
    public override Expression<Func<Account, bool>> AsExpression()
    {
        return a => a.IsActive;
    }
}

class AccountAmountSpecification(decimal amount) : SpecificationPatternInPractice.Specification.SpecificationBase<Account>
{
    public override Expression<Func<Account, bool>> AsExpression()
    {
        return a => a.Amount > amount;
    }
}

class Account
{
    public static Account Create(
        bool isActive,
        decimal amount) => new() { IsActive = isActive, Amount = amount };

    public bool IsActive { get; set; }
    public decimal Amount { get; set; } 
}