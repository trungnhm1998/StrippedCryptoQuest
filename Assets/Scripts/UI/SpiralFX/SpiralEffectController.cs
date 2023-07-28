using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
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

        private void Awake()
        {
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;
        }

        private void OnEnable()
        {
            _spiralConfig.SpiralIn += SpiralIn_Raised;
            _spiralConfig.SpiralOut += SpiralOut_Raised;
        }

        private void OnDisable()
        {
            _spiralConfig.SpiralIn -= SpiralIn_Raised;
            _spiralConfig.SpiralOut -= SpiralOut_Raised;
        }

        private void SpiralIn_Raised()
        {
            OnSpiralIn();
        }

        private void SpiralOut_Raised()
        {
            OnSpiralOut();
        }

        private void OnSpiralIn()
        {
            ResetSpiralSize();
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
                .OnComplete(() => { OnSpiralSpin(); });
        }

        private void OnSpiralSpin()
        {
            for (int i = 0; i < _spiralMasks.Count; i++)
            {
                _spiralMasks[i].gameObject.SetActive(true);
                _spiralMasks[i].color = _spiralConfig.Color;
                _spiralMasks[i].DOFillAmount(1, _spiralDuration / 2);
            }
        }

        private void OnSpiralReveseSpin()
        {
            for (int i = 0; i < _spiralMasks.Count; i++)
            {
                _spiralMasks[i].gameObject.SetActive(true);
                _spiralMasks[i].DOFillAmount(0, _spiralDuration / 2);
            }
        }

        private void OnSpiralOut()
        {
            StartCoroutine(StartReverseSpiral());
        }

        private IEnumerator StartReverseSpiral()
        {
            OnSpiralReveseSpin();
            yield return new WaitForSeconds(_spiralDuration / 2);
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_baseHorizontal.rectTransform.DOSizeDelta(
                    new Vector2(0, _baseHorizontal.rectTransform.sizeDelta.y), _spiralDuration / 2))
                .Join(_baseVertical.rectTransform.DOSizeDelta(
                    new Vector2(_baseVertical.rectTransform.sizeDelta.x, 0), _spiralDuration / 2))
                .OnComplete(() => ResetSpiralSize());
        }


        private void ResetSpiralSize()
        {
            for (int i = 0; i < _spiralMasks.Count; i++)
            {
                _spiralMasks[i].gameObject.SetActive(false);
                _spiralMasks[i].fillAmount = 0;
            }
        }
    }
}