using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI.SpiralFX
{
    public class SpiralEffectController : MonoBehaviour
    {
        [SerializeField] protected SpiralConfigSO _spiralConfig;

        [Header("Spiral")]
        [SerializeField] private Image _baseHorizontal;

        [SerializeField] private Image _baseVertical;
        [SerializeField] private List<Image> _spiralMasks;
        private float _spiralDuration => _spiralConfig.Duration;
        private float _screenWidth;
        private float _screenHeight;
        private Sequence _sequence;
        private Vector2 _horizontalStartSize;
        private Vector2 _verticalStartSize;

        private void Awake()
        {
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;
            _horizontalStartSize = _baseHorizontal.rectTransform.sizeDelta;
            _verticalStartSize = _baseVertical.rectTransform.sizeDelta;
        }

        private void OnEnable()
        {
            _spiralConfig.SpiralIn += SpiralIn_Raised;
            _spiralConfig.SpiralOut += SpiralOut_Raised;
            _spiralConfig.FadeOut += FadeOutRaised;
        }

        private void OnDisable()
        {
            _spiralConfig.SpiralIn -= SpiralIn_Raised;
            _spiralConfig.SpiralOut -= SpiralOut_Raised;
            _spiralConfig.FadeOut -= FadeOutRaised;
        }

        private void FadeOutRaised()
        {
            FadeOut();
        }

        private void SpiralIn_Raised()
        {
            SpiralIn();
        }

        private void SpiralOut_Raised()
        {
            SpiralOut();
        }

        private void FadeOut()
        {
            for (int i = 0; i < _spiralMasks.Count; i++)
            {
                _spiralMasks[i].gameObject.SetActive(true);
                _spiralMasks[i].color = _spiralConfig.Color;
                _spiralMasks[i].DOFade(0, _spiralDuration);
            }

            _baseHorizontal.gameObject.SetActive(false);
            _baseVertical.gameObject.SetActive(false);
            StartCoroutine(CoInvokeFadeoutStatus(_spiralDuration));
        }

        private IEnumerator CoInvokeFadeoutStatus(float duration)
        {
            yield return new WaitForSeconds(duration);
            _spiralConfig.OnFinishFadeOut();
        }

        private void SpiralIn()
        {
            ResetSpiralSize();
            _baseHorizontal.gameObject.SetActive(true);
            _baseVertical.gameObject.SetActive(true);
            _baseHorizontal.color = _spiralConfig.Color;
            _baseVertical.color = _spiralConfig.Color;

            _baseHorizontal.gameObject.SetActive(true);
            _baseVertical.gameObject.SetActive(true);
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_baseHorizontal.rectTransform.DOSizeDelta(
                    new Vector2(_screenWidth, _baseHorizontal.rectTransform.sizeDelta.y), _spiralDuration / 2))
                .Join(_baseVertical.rectTransform.DOSizeDelta(
                    new Vector2(_baseVertical.rectTransform.sizeDelta.x, _screenHeight), _spiralDuration / 2))
                .OnComplete(() => { SpiralSpin(); });
        }

        private void SpiralSpin()
        {
            for (int i = 0; i < _spiralMasks.Count; i++)
            {
                _spiralMasks[i].gameObject.SetActive(true);
                _spiralMasks[i].color = _spiralConfig.Color;
                _spiralMasks[i].DOFillAmount(1, _spiralDuration / 2);
            }

            StartCoroutine(CoInvokeSpiralInStatus(_spiralDuration / 2));
        }

        private IEnumerator CoInvokeSpiralInStatus(float duration)
        {
            yield return new WaitForSeconds(duration);
            _spiralConfig.OnFinishSpiralIn();
        }

        private void SpiralReveseSpin()
        {
            for (int i = 0; i < _spiralMasks.Count; i++)
            {
                _spiralMasks[i].gameObject.SetActive(true);
                _spiralMasks[i].DOFillAmount(0, _spiralDuration / 2);
            }
        }

        private void SpiralOut()
        {
            StartCoroutine(StartReverseSpiral());
        }

        private IEnumerator StartReverseSpiral()
        {
            SpiralReveseSpin();
            yield return new WaitForSeconds(_spiralDuration / 2);
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_baseHorizontal.rectTransform.DOSizeDelta(
                    new Vector2(0, _baseHorizontal.rectTransform.sizeDelta.y), _spiralDuration / 2))
                .Join(_baseVertical.rectTransform.DOSizeDelta(
                    new Vector2(_baseVertical.rectTransform.sizeDelta.x, 0), _spiralDuration / 2))
                .OnComplete(() =>
                {
                    _spiralConfig.OnFinishSpiralOut();
                });
        }


        private void ResetSpiralSize()
        {
            for (int i = 0; i < _spiralMasks.Count; i++)
            {
                _spiralMasks[i].gameObject.SetActive(false);
                _spiralMasks[i].color = _spiralConfig.Color;
                _spiralMasks[i].fillAmount = 0;
            }

            _baseHorizontal.rectTransform.sizeDelta = _horizontalStartSize;
            _baseVertical.rectTransform.sizeDelta = _verticalStartSize;
        }
    }
}