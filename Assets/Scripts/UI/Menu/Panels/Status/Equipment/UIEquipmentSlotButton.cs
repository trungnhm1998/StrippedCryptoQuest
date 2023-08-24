using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIEquipmentSlotButton : MultiInputButton
    {
        [SerializeField] private GameObject _selectEffect;
        [SerializeField] private EEquipmentCategory _equipmentCategory;

        public event UnityAction<EEquipmentCategory> Pressed;

        public void OnPressed()
        {
            Pressed?.Invoke(_equipmentCategory);
            _selectEffect.SetActive(false);
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