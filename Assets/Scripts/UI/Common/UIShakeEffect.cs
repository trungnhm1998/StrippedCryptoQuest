using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Common
{
    public class UIShakeEffect : MonoBehaviour
    {
        public UnityAction ShakeComplete;

        [SerializeField] private RectTransform _rect;
        [SerializeField] private float _shakeDuration = .5f;
        [SerializeField] private float _shakeStrength = 5f;
        [SerializeField] private int _shakeVibrato = 10;
        [SerializeField] private float _shakeRandomness = 50f;
        [SerializeField] private bool _isShakeAtStart = false;

        private void OnValidate()
        {
            if (_rect) return;
            _rect = GetComponent<RectTransform>();
        }

        private void Start()
        {
            if (!_isShakeAtStart) return;
            Shake();
        }

        public void Shake()
        {
            _rect.DOShakePosition(_shakeDuration, _shakeStrength, _shakeVibrato, _shakeRandomness)
                .OnComplete(() => ShakeComplete?.Invoke());
        }
    }
}