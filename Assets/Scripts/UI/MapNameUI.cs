using System;
using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI.FadeController;
using UnityEngine;
using UnityEngine.Localization;
using DG.Tweening;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest
{
    public class MapNameUI : MonoBehaviour
    {
        [Header("Listened events")]
        [SerializeField] private StringEventChannelSO _onShowMapNameUI;

        [SerializeField] private LoadSceneEventChannelSO _loadMapEvent;

        [SerializeField] private GameObject _container;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _frameImage;
        [SerializeField] private TextMeshProUGUI _mapNameText;
        [SerializeField] private float _showDuration = 10f;
        [SerializeField] private float _showSpeed = .5f;
        [SerializeField] private float _hideSpeed = .5f;
        [SerializeField] private float _hideDistance = -500f;
        private Sequence _sequence;

        private void OnEnable()
        {
            _container.transform.DOMoveX(_hideDistance, 0);
            _onShowMapNameUI.EventRaised += OnShowMapName;
            _loadMapEvent.LoadingRequested += OnLoadNewScene;
        }

        private void OnDisable()
        {
            _onShowMapNameUI.EventRaised -= OnShowMapName;
            _loadMapEvent.LoadingRequested -= OnLoadNewScene;
        }

        private void OnLoadNewScene(SceneScriptableObject scene)
        {
            HideMapName(0);
        }

        private void Start()
        {
            _container.SetActive(false);
        }

        private void OnShowMapName(string mapName)
        {
            SetUpMapName(mapName);
            ShowMapName(_showSpeed);
        }

        private void SetUpMapName(string mapName)
        {
            _mapNameText.text = mapName;
        }

        private void ShowMapName(float duration)
        {
            _container.SetActive(true);
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_backgroundImage.DOFade(1, duration))
                .Join(_mapNameText.DOFade(1, duration))
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
                .Join(_mapNameText.DOFade(0, duration))
                .Join(_container.transform.DOMoveX(_hideDistance, duration));
        }
    }
}