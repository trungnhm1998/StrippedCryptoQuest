using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Tooltips.Equipment;
using UnityEngine;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    public class TooltipEquipmentProvider : MonoBehaviour, ITooltipEquipmentProvider
    {
        [SerializeField] private UIEquipment _uiEquipment;
        public IEquipment Equipment => _uiEquipment.Equipment;
    }
}