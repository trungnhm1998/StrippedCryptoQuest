using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Tooltips;
using CryptoQuest.UI.Tooltips.Equipment;
using UnityEngine;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    public class TooltipEquipmentProvider : MonoBehaviour, ITooltipEquipmentProvider
    {
        [SerializeField] private UIEquipment _uiEquipment;
        public EquipmentInfo Equipment => _uiEquipment.Equipment;
    }
}