public interface IUpgradeEffect
{
    void Apply(ICanUpgrade target);
    int GetCost();
    bool CanUpgrade();
}