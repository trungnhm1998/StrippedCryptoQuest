using CryptoQuest.Beast;

namespace CryptoQuest.Ranch.Upgrade.UI
{
    public interface IBeastUpgradeValidator
    {
        bool CanUpgrade(IBeast beast, int toLevel);
        bool IsEnoughGoldToUpgrade(float goldOwned, float cost);
    }

    public class BeastUpgradeValidator : IBeastUpgradeValidator
    {
        public bool CanUpgrade(IBeast beast, int toLevel) => toLevel > beast.Level && toLevel <= beast.MaxLevel;

        public bool IsEnoughGoldToUpgrade(float goldOwned, float cost) => goldOwned >= cost;
    }
}