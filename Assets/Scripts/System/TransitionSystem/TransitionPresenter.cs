using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest
{
    public class TransitionPresenter : MonoBehaviour
    {
        public UnityAction OnTransitionInComplete;
        public UnityAction OnTransitionOutComplete;
        public static UnityAction OnTransitionProgressed;
        public FadeConfigSO FadeConfigSO;
        public SpiralConfigSO SpiralConfigSO;
        public SpiralEffectController SpiralEffectController;
        public Image ProgressImage;

        private void OnEnable()
        {
            SpiralConfigSO.DoneSpiralIn += OnTransitionInCompleted;
            SpiralConfigSO.DoneSpiralOut += OnTransitionOutCompleted;
            FadeConfigSO.FadeInComplete += OnTransitionInCompleted;
            FadeConfigSO.FadeOutComplete += OnTransitionOutCompleted;
        }

        private void OnDisable()
        {
            SpiralConfigSO.DoneSpiralIn -= OnTransitionInCompleted;
            SpiralConfigSO.DoneSpiralOut -= OnTransitionOutCompleted;
            FadeConfigSO.FadeInComplete -= OnTransitionInCompleted;
            FadeConfigSO.FadeOutComplete -= OnTransitionOutCompleted;
        }

        public void Fadein()
        {
            ProgressImage.color = Color.black;
            FadeConfigSO.OnFadeIn();
        }

        private void OnTransitionInCompleted()
        {
            OnTransitionInComplete?.Invoke();
        }

        private void OnTransitionOutCompleted()
        {
            OnTransitionOutComplete?.Invoke();
        }

        public void FadeOut()
        {
            FadeConfigSO.OnFadeOut();
        }

        public void SpiralIn()
        {
            ProgressImage.color = SpiralConfigSO.Color;
            SpiralEffectController.SpiralIn();
        }

        public void SpiralOut()
        {
            SpiralEffectController.SpiralOut();
        }

        public void TransitionProgressed()
        {
            ProgressImage.gameObject.SetActive(true);
            OnTransitionProgressed?.Invoke();
        }

        public void ResetToDefault()
        {
            SpiralEffectController.RestoreDefault();
            ProgressImage.gameObject.SetActive(false);
        }
    }
}