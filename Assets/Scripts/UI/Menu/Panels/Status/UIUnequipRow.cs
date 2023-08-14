using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UIUnequipRow : MultiInputButton
    {
        public override void OnSelect(BaseEventData eventData)
        {
            UITooltip.HideTooltipEvent?.Invoke();
            base.OnSelect(eventData);
        }

        public void OnPressed()
        {
            Debug.Log($"Unequip pressed");
        }
    }
}