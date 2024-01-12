using CryptoQuest.UI.Tooltips.Equipment;
using CryptoQuest.UI.Tooltips.Events;
using UnityEngine;

namespace CryptoQuest.UI.Tooltips
{
    public class ShopEquipmentTooltipController : MonoBehaviour
    {
        [SerializeField] private ShowTooltipEvent _showTooltipEvent;
        [SerializeField] private GameObject _toolTipGameObject;

        private void OnEnable()
        {
            _showTooltipEvent.EventRaised += ShowTooltip;
        }

        private void OnDisable()
        {
            _showTooltipEvent.EventRaised -= ShowTooltip;
        }

        private void ShowTooltip(bool isShow)
        {
            _toolTipGameObject.SetActive(false);
            if (isShow)
                _toolTipGameObject.SetActive(true);
        }
    }
}