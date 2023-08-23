using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIEquipmentRow : MonoBehaviour
    {
        [SerializeField] private RectTransform _downPoint;
        [SerializeField] private RectTransform _upPoint;
        [SerializeField] private UIEquipmentItem _button;

        private void OnEnable()
        {
            if (_button) _button.SelectedEvent += ButtonOnSelectedEvent;
        }

        private void OnDisable()
        {
            if (_button) _button.SelectedEvent -= ButtonOnSelectedEvent;
        }

        private void ButtonOnSelectedEvent(int index)
        {
            UITooltip
                .ShowTooltipEvent?
                .Invoke(_upPoint.position, _downPoint.position);
        }
    }
}