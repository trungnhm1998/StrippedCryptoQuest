using CryptoQuest.UI.Tooltips.Events;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Tooltips
{
    public class TooltipTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private float _delayShowTooltip = 0.5f;
        [SerializeField] private ShowTooltipEvent _tooltipEnabledEventChannel;
        private Tween _delayedCall;

        public void OnSelect(BaseEventData _)
        {
            _delayedCall?.Kill();
            _delayedCall = DOVirtual.DelayedCall(_delayShowTooltip,
                () => _tooltipEnabledEventChannel.RaiseEvent(true));
        }

        public void OnDeselect(BaseEventData _)
        {
            _delayedCall?.Kill();
            _tooltipEnabledEventChannel.RaiseEvent(false);
        }
    }
}