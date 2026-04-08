namespace FareEngine.Domain.SoldProducts;

public sealed class SoldDailyPass : SoldProduct
{
    private SoldDailyPass() : base() { }
    
    internal SoldDailyPass(Guid id) : base(id, ProductType.DailyPass) { }
}