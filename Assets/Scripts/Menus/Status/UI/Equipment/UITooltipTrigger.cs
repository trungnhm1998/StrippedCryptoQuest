using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    public class UITooltipTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent<bool> _onTooltipShow;
        [SerializeField] private MultiInputButton _button;

        private void OnEnable()
        {
            _button.Selected += ShowTooltip;
            _button.DeSelected += HideTooltip;
        }

        private void OnDisable()
        {
            _button.Selected -= ShowTooltip;
            _button.DeSelected -= HideTooltip;
        }

        private void ShowTooltip()
        {
            _onTooltipShow?.Invoke(true);
        }

        private void HideTooltip()
        {
            _onTooltipShow?.Invoke(false);
        }
    }
}