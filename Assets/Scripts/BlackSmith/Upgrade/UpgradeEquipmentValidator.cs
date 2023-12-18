using CryptoQuest.Item.Equipment;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public interface IUpgradeEquipmentValidator
    {
        bool CanUpgrade(IEquipment equipment, int toLevel);
        bool IsEnoughGoldToUpgrade(float goldOwned, float cost);
    }

    public class UpgradeEquipmentValidator : IUpgradeEquipmentValidator
    {
        public bool CanUpgrade(IEquipment equipment, int toLevel)
            => toLevel > equipment.Level && toLevel <= equipment.Data.MaxLevel;

        public bool IsEnoughGoldToUpgrade(float goldOwned, float cost)
        {
            return goldOwned >= cost;
        }
    }
}