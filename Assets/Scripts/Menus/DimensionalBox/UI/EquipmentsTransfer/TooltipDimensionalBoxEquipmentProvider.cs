using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Tooltips.Equipment;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.UI.EquipmentsTransfer
{
    public class TooltipDimensionalBoxEquipmentProvider : MonoBehaviour, ITooltipEquipmentProvider
    {
        [SerializeField] private UIEquipment _uiEquipment;
        public IEquipment Equipment => _uiEquipment.Equipment;
    }
}