using DG.Tweening;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Common
{
    public class UIShakeEffect : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _shakeEvent;
        [SerializeField] private VoidEventChannelSO _shakeCompleteEvent;

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

        private void OnEnable()
        {
            _shakeEvent.EventRaised += Shake;
        }

        private void OnDisable()
        {
            _shakeEvent.EventRaised -= Shake;
        }

        private void Start()
        {
            if (!_isShakeAtStart) return;
            Shake();
        }

        private void Shake()
        {
            _rect.DOShakePosition(_shakeDuration, _shakeStrength, _shakeVibrato, _shakeRandomness)
                .OnComplete(() => _shakeCompleteEvent.RaiseEvent());
        }
    }
}