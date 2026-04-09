namespace FareEngine.Domain.FarePolicies;

public sealed record FarePolicyIdWithType(Guid FarePolicyId, FarePolicyType Type)
{
    public override string ToString()
    {
        return $"{{id: {FarePolicyId} type: ({Type})}}";
    }
}