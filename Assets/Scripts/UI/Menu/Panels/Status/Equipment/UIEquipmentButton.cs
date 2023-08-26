using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIEquipmentButton : MultiInputButton
    {
        public static event UnityAction<Button> InspectingRow;

        [SerializeField] private GameObject _selectEffect;

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _selectEffect.SetActive(true);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            _selectEffect.SetActive(false);
        }
    }
}