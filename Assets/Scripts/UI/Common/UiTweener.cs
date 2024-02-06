using DG.Tweening;
using UnityEngine;

namespace CryptoQuest.UI.Common
{
    public class UiTweener : MonoBehaviour
    {
        [SerializeField] private RectTransform _parent;
        [SerializeField] private RectTransform _content;
        [SerializeField] private float _timeDuration;

        private Sequence _sequence;

        public void Tween()
        {
            if (_content.rect.width <= _parent.rect.width) return;
            var reachValue = 0 - (_content.rect.width - _parent.rect.width);
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_content.transform.DOMoveX(reachValue, _timeDuration).SetEase(Ease.Linear))
                .AppendInterval(2f)
                .OnComplete(Reset)
                .SetLoops(-1);
        }

        public void Reset()
        {
            _sequence.Kill();
            _content.anchoredPosition = new Vector2(0, _content.anchoredPosition.y);
        }
    }
}