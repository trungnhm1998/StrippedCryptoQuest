using CryptoQuest.Battle.Components;
using CryptoQuest.BlackSmith.Upgrade.Sagas;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Events;
using ESlotType = CryptoQuest.Item.Equipment.ESlot;

namespace CryptoQuest.BlackSmith.Upgrade.Actions
{
    public class RequestUpgrade : ActionBase
    {
        public IEquipment EquipmentToUpgrade { get; private set; }
        public int UpgradeLevel { get; private set; }

        public RequestUpgrade(IEquipment equipmentToUpgrade, int toLevel)
        {
            EquipmentToUpgrade = equipmentToUpgrade;
            UpgradeLevel = toLevel;
        }
    }

    public class UpgradeResponsed : ActionBase
    {
        public UpgradeResponse Response { get; private set; }
        public UpgradeResponsed(UpgradeResponse response)
        {
            Response = response;
        }
    }

    public struct UpgradedEquipmentInfo
    {
        public IEquipment Equipment;
        public HeroBehaviour EquippedHero;
        public ESlotType Slot;
    }

    public class UpgradeSucceed : ActionBase
    {
        public UpgradedEquipmentInfo UpgradedEquipmentInfo { get; private set; }
        public int Level { get; private set; }
        public float GoldAfter { get; private set; }

        public UpgradeSucceed(UpgradedEquipmentInfo equipmentInfo, int level, float gold)
        {
            UpgradedEquipmentInfo = equipmentInfo;
            Level = level;
            GoldAfter = gold;
        }
    }

    public class UpgradeFailed : ActionBase { }
}