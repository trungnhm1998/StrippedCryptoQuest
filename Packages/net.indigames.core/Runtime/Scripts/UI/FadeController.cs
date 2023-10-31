using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace IndiGames.Core.UI
{
    public class FadeController : MonoBehaviour
    {
        [FormerlySerializedAs("FadeConfig")] [SerializeField]
        private FadeConfigSO _config;

        [SerializeField] private Image _fadeImg;
        [SerializeField] private Color _fadeInColor;
        [SerializeField] private Color _fadeOutColor;

        private void OnEnable()
        {
            _config.FadeIn += InLogic;
            _config.FadeOut += OutLogic;
        }

        private void OnDisable()
        {
            _config.FadeIn -= InLogic;
            _config.FadeOut -= OutLogic;
        }

        private void InLogic()
        {
            _fadeImg.enabled = true;
            _fadeImg.DOBlendableColor(_fadeInColor, _config.Duration)
                .OnComplete(() => _config.FadeInComplete?.Invoke());
        }

        private void OutLogic()
        {
            StartCoroutine(CoFadeOut());
        }


        private IEnumerator CoFadeOut()
        {
            yield return new WaitForSeconds(_config.WaitDuration);
            _fadeImg.DOBlendableColor(_fadeOutColor, _config.Duration)
                .OnComplete(() => _config.FadeOutComplete?.Invoke());
            yield return new WaitForSeconds(_config.Duration);
            _fadeImg.enabled = false;
        }
    }
}