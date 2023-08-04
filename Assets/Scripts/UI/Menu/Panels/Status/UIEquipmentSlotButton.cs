using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UIEquipmentSlotButton : MultiInputButton
    {
        [SerializeField] private GameObject _selectEffect;

        public enum EEquipmentType
        {
            None = -1,
            Weapon = 0,
            Shield = 1,
            Head = 2,
            Body = 3,
            Leg = 4,
            Foot = 5,
            Accessory1 = 6,
            Accessory2 = 7,
        }

        public event UnityAction<EEquipmentType> Pressed;
        [SerializeField] private EEquipmentType _equipmentType;

        public void OnPressed()
        {
            Pressed?.Invoke(_equipmentType);
        }

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