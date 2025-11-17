using SpecificationPatternInPractice.Specification;
using System.Linq.Expressions;

Console.WriteLine("Hello, World!");

var account = Account.Create(true, 4000);
var minAmount = 3000;

var spec = new ActiveAccountSpecification()
    .And(new AccountAmountSpecification(minAmount));

if (spec.IsSatisfiedBy(account)) Console.WriteLine("Valid");
else Console.WriteLine("Invalid");

class ActiveAccountSpecification : SpecificationBase<Account>
{
    public override Expression<Func<Account, bool>> AsExpression()
    {
        return a => a.IsActive;
    }
}

class AccountAmountSpecification(decimal amount) : SpecificationBase<Account>
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
