using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIUnequipRow : MultiInputButton
    {
        public override void OnSelect(BaseEventData eventData)
        {
            // TODO: REFACTOR TOOL TIP HIDE
            base.OnSelect(eventData);
        }

        public void OnPressed()
        {
            Debug.Log($"Unequip pressed");
        }
    }
}