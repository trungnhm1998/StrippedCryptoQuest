using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIEquipment : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private RectTransform _downPoint;
        [SerializeField] private RectTransform _upPoint;
        private RectTransform _equipmentViewport;

        private void OnEnable()
        {
            _equipmentViewport = gameObject.GetComponent<RectTransform>();
        }


        private void ButtonOnSelectedEvent(int index)
        {
            UITooltip
                .ShowTooltipEvent?
                .Invoke(_upPoint.position, _downPoint.position, _equipmentViewport.rect.height);
        }

        public void Init(EquipmentInfo equipment)
        {
            var def = equipment.Data;
            _name.StringReference = def.DisplayName;
            _icon.sprite = def.EquipmentType.Icon;
        }
    }
}