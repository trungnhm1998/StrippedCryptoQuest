using CryptoQuest.Core;
using CryptoQuest.UI.Tooltips;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    public class UITooltipTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private float _delayShowTooltip = 0.5f;
        [SerializeField] private UIEquipment _equipmentUI;
        [SerializeField] private RectTransform _toolTipPosition;
        private Tween _delayedCall;

        public void OnSelect(BaseEventData eventData)
        {
            if (_equipmentUI.Equipment.IsValid() == false) return;
            _delayedCall?.Kill();
            var context = new EquipmentTooltipContext()
            {
                Equipment = _equipmentUI.Equipment,
                Position = _toolTipPosition.position
            };
            _delayedCall = DOVirtual.DelayedCall(_delayShowTooltip,
                () => ActionDispatcher.Dispatch(new ShowEquipmentTooltip(context)));
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _delayedCall?.Kill();
            ActionDispatcher.Dispatch(new HideEquipmentTooltip());
        }
    }
}