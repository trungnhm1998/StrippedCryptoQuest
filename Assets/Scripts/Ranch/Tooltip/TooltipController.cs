using System.Collections;
using CryptoQuest.Beast;
using CryptoQuest.UI.Tooltips;
using CryptoQuest.UI.Tooltips.Events;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Ranch.Tooltip
{
    public class TooltipController : UITooltipBase
    {
        [SerializeField] private UIBeastTooltip _uiTooltip;
        [SerializeField] private float _delayTime = 0.1f;
        private Tween _delayedCall;
        private IBeast _beast;

        protected override bool CanShow()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return false;
            var provider = selectedGameObject.GetComponent<ITooltipBeastProvider>();
            if (provider == null) return false;
            if (provider.Beast == null || provider.Beast.IsValid() == false) return false;
            _beast = provider.Beast;
            return true;
        }
        
        protected override void Init()
        {
            _delayedCall?.Kill();
            _delayedCall = DOVirtual.DelayedCall(_delayTime,
                () => _uiTooltip.Init(_beast));
        }
    }
}