using CryptoQuest.Events;
using DG.Tweening;
using IndiGames.Core.Events.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.MapName
{
    public class UIMapName : MonoBehaviour
    {
        [Header("Listened events")]
        [SerializeField] private LocalizedStringEventChannelSO _onShowMapNameUI;

        [SerializeField] private VoidEventChannelSO _onHideMapNameUI;

        [Header("Set up")]
        [SerializeField] private GameObject _container;

        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _frameImage;
        [SerializeField] private LocalizeStringEvent _mapNameText;
        [SerializeField] private float _showDuration = 10f;
        [SerializeField] private float _showSpeed = .5f;
        [SerializeField] private float _hideSpeed = .5f;
        [SerializeField] private float _hideDistance = -500f;
        private Sequence _sequence;
        private TextMeshProUGUI _text;

        private void OnEnable()
        {
            _container.transform.DOMoveX(_hideDistance, 0);
            _text = _mapNameText.GetComponent<TextMeshProUGUI>();
            _onShowMapNameUI.EventRaised += OnShowMapName;
            _onHideMapNameUI.EventRaised += OnLoadNewScene;
        }

        private void OnDisable()
        {
            _onShowMapNameUI.EventRaised -= OnShowMapName;
            _onHideMapNameUI.EventRaised -= OnLoadNewScene;
        }

        private void OnLoadNewScene()
        {
            HideMapName(0);
        }

        private void Start()
        {
            _container.SetActive(false);
        }

        private void OnShowMapName(LocalizedString mapName)
        {
            SetUpMapName(mapName);
            ShowMapName(_showSpeed);
        }

        private void SetUpMapName(LocalizedString mapName)
        {
            _mapNameText.StringReference = mapName;
        }

        private void ShowMapName(float duration)
        {
            _container.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_backgroundImage.rectTransform);
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_backgroundImage.DOFade(1, duration))
                .Join(_text.DOFade(1, duration))
                .Join(_frameImage.DOFade(1, duration))
                .Join(_container.transform.DOMoveX(0, duration))
                .AppendInterval(_showDuration)
                .OnComplete(() => HideMapName(_hideSpeed));
        }

        private void HideMapName(float duration)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_backgroundImage.DOFade(0, duration))
                .Join(_frameImage.DOFade(0, duration))
                .Join(_text.DOFade(0, duration))
                .Join(_container.transform.DOMoveX(_hideDistance, duration));
        }
    }
}