using CryptoQuest.Core;
using CryptoQuest.UI.Tooltips;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    public class UITooltipTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private UIEquipment _equipmentUI;

        public void OnSelect(BaseEventData eventData)
        {
            if (_equipmentUI.Equipment.IsValid() == false) return;
            ActionDispatcher.Dispatch(new ShowEquipmentTooltip() { Equipment = _equipmentUI.Equipment });
        }

        public void OnDeselect(BaseEventData eventData)
        {
            // ActionDispatcher.Dispatch(new HideEquipmentTooltip());
        }
    }
}