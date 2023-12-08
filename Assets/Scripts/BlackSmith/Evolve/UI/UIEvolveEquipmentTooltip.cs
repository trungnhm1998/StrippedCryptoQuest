using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Tooltips.Equipment;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIEvolveEquipmentTooltip : UIEquipmentTooltip
    {
        public void SetEquipment(IEquipment equipment) => _equipment = equipment;

        protected override bool CanShow()
        {
            if (_equipment == null || !_equipment.IsValid())
                return false;

            return true;
        }
    }
}