using CryptoQuest.UI.SpiralFX;
using DG.Tweening;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.System.TransitionSystem
{
    public class TransitionPresenter : MonoBehaviour
    {
        public UnityAction OnTransitionInComplete;
        public UnityAction OnTransitionOutComplete;
        [SerializeField] private VoidEventChannelSO _onTransitionProgressedEvent;
        [SerializeField] private SpiralConfigSO _spiralConfigSO;
        [SerializeField] private SpiralEffectController _spiralEffectController;
        [SerializeField] private Image _progressImage;

        private void OnEnable()
        {
            _spiralConfigSO.DoneSpiralIn += OnTransitionInCompleted;
            _spiralConfigSO.DoneSpiralOut += OnTransitionOutCompleted;
        }

        private void OnDisable()
        {
            _spiralConfigSO.DoneSpiralIn -= OnTransitionInCompleted;
            _spiralConfigSO.DoneSpiralOut -= OnTransitionOutCompleted;
        }

        public void Fadein()
        {
            _progressImage.color = new Color(_progressImage.color.r, _progressImage.color.g, _progressImage.color.b, 0);
            _progressImage.gameObject.SetActive(true);
            _progressImage.DOFade(1, 0.5f)
                .OnComplete(OnTransitionInCompleted);
        }

        public void FadeOut()
        {
            _progressImage.gameObject.SetActive(true);
            _progressImage.DOFade(0, 0.5f)
                .OnComplete(OnTransitionOutCompleted);
        }

        private void OnTransitionInCompleted()
        {
            OnTransitionInComplete?.Invoke();
        }

        private void OnTransitionOutCompleted()
        {
            OnTransitionOutComplete?.Invoke();
        }


        public void SpiralIn()
        {
            _progressImage.color = _spiralConfigSO.Color;
            _spiralEffectController.SpiralIn();
        }

        public void SpiralOut()
        {
            _spiralEffectController.SpiralOut();
        }

        public void TransitionProgressed()
        {
            _progressImage.gameObject.SetActive(true);
            _progressImage.color = new Color(_progressImage.color.r, _progressImage.color.g, _progressImage.color.b, 1);
            _onTransitionProgressedEvent.RaiseEvent();
        }

        public void ResetToDefault()
        {
            _spiralEffectController.RestoreDefault();
            _progressImage.gameObject.SetActive(false);
        }
    }
}