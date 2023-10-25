using CryptoQuest.Input;
using CryptoQuest.UI.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI
{
    public class ScrollVerticalButtonSelector : VerticalButtonSelector
    {
        [SerializeField] private AutoScroll _autoScroll;

        protected override void NavigateSelectCommand(Vector2 dir)
        {
            if (!Interactable || dir.y == 0) return;
            base.NavigateSelectCommand(dir);
            _autoScroll.Scroll();
            ScrollArrowsBehaviour.NavigateEvent?.Invoke();
        }
        
        protected override void OnSelectFirstButton()
        {
            base.OnSelectFirstButton();
            _autoScroll.ScrollToTop();
            ScrollArrowsBehaviour.NavigateEvent?.Invoke();
        }
    }
}