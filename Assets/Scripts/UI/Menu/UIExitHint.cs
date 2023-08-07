using CryptoQuest.Input;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class UIExitHint : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private GameObject _hint;
        [SerializeField] private Image _fillImage;
        private TweenerCore<float, float, FloatOptions> _tween;
        private float _holdDuration = -1f;

        private void OnEnable()
        {
            _hint.SetActive(false);
            _inputMediator.ClosingMenuEvent += ClosingMenuEvent;
        }

        private void OnDisable()
        {
            _inputMediator.ClosingMenuEvent -= ClosingMenuEvent;
        }


        private void ClosingMenuEvent(InputAction.CallbackContext context)
        {
            if (_holdDuration <= 0f)
            {
                _holdDuration = ((HoldInteraction)context.interaction).duration;
            }

            if (context.started)
            {
                _hint.SetActive(true);
                _fillImage.fillAmount = 0;
                _tween?.Kill();
                _tween = DOTween.To(() => _fillImage.fillAmount, value => _fillImage.fillAmount = value, 1f,
                    _holdDuration);
            }

            if (!context.performed && !context.canceled) return;

            _tween?.Kill();
            _fillImage.fillAmount = 0;
            _hint.SetActive(false);
        }
    }
}