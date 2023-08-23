using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UIEquipmentRow : MonoBehaviour
    {
        private const int BOUNDARY = 2;
        [SerializeField] private RectTransform _downPoint;
        [SerializeField] private RectTransform _upPoint;
        [SerializeField] private RectTransform _selfViewport;
        [SerializeField] private UIStatusInventoryItemButton _button;

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