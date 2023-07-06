using System;
using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI.FadeController;
using UnityEngine;
using UnityEngine.Localization;
using DG.Tweening;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest
{
    public class MapNameUI : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _sceneLoaded;
        [SerializeField] private VoidEventChannelSO _hideMapName;
        [SerializeField] private LocalizedString _mapNameLocalizedKey;
        [SerializeField] private LocalizeStringEvent _mapNameLabel;
        [SerializeField] private GameObject _container;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _frameImage;
        [SerializeField] private float _showAnimationDuration = .5f;
        [SerializeField] private float _hideAnimationDuration = .5f;
        [SerializeField] private float _hideDistance = -1000f;
        private Sequence _sequence;

        private void OnEnable()
        {
            _sceneLoaded.EventRaised += OnSceneLoaded;
            _hideMapName.EventRaised += OnHideMapName;
        }

        private void OnDisable()
        {
            _sceneLoaded.EventRaised -= OnSceneLoaded;
            _hideMapName.EventRaised -= OnHideMapName;
        }

        private void Start()
        {
            HideMapName(0);
        }

        private void OnSceneLoaded()
        {
            ShowMapName(_showAnimationDuration);
        }

        private void OnHideMapName()
        {
            HideMapName(_hideAnimationDuration);
        }

        private void ShowMapName(float duration)
        {
            _mapNameLabel.StringReference = _mapNameLocalizedKey;
            var text = _mapNameLabel.gameObject.GetComponent<TextMeshProUGUI>();
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_backgroundImage.DOFade(1, duration))
                .Join(_frameImage.DOFade(1, duration))
                .Join(text.DOFade(1, duration))
                .Join(_container.transform.DOMoveX(0, duration));
        }

        private void HideMapName(float duration)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_backgroundImage.DOFade(0, duration))
                .Join(_frameImage.DOFade(0, duration))
                .Join(_mapNameLabel.gameObject.GetComponent<TextMeshProUGUI>().DOFade(0, duration))
                .Join(_container.transform.DOMoveX(_hideDistance, duration));
        }
    }
}