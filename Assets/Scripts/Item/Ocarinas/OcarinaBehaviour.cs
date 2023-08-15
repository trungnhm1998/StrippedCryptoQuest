using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Events;
using CryptoQuest.Map;
using CryptoQuest.System.Dialogue.Events;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Item.Ocarinas
{
    public class OcarinaBehaviour : MonoBehaviour
    {
        [SerializeField] private PathStorageSO _pathStorage;
        [SerializeField] private LoadSceneEventChannelSO _requestLoadMapEvent;
        [SerializeField] private SceneScriptableObject _worldMapScene;
        [SerializeField] private MapPathEventChannelSO _destinationSelectedEvent;
        [SerializeField] private VoidEventChannelSO _destinationConfirmEvent;
        [SerializeField] private SpiralConfigSO _spiralConfig;
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;

        [Header("Ocarina UI")]
        [SerializeField] private GameObject _ocarinaUI;

        private MapPathSO _selectedPath;
        private List<GoFrom> _cachedDestinations = new();

        private void OnEnable()
        {
            _destinationSelectedEvent.EventRaised += SelectDestination;
            _destinationConfirmEvent.EventRaised += ConfirmUseOcarina;
            _ocarinaUI.SetActive(false);
        }

        private void OnDisable()
        {
            _destinationSelectedEvent.EventRaised -= SelectDestination;
            _destinationConfirmEvent.EventRaised -= ConfirmUseOcarina;
        }

        private void SelectDestination(MapPathSO path)
        {
            _selectedPath = path;
        }

        private void ConfirmUseOcarina()
        {
            StartCoroutine(CoActivateOcarinaAnim());
        }

        public IEnumerator CoActivateOcarinaAnim()
        {
            HeroBehaviour heroBehaviour = FindObjectOfType<HeroBehaviour>();
            _ocarinaUI.SetActive(true);
            yield return heroBehaviour.ActivateOcarina();
            _ocarinaUI.SetActive(false);
            ActivateSpiral();
        }

        public void ActivateSpiral()
        {
            _spiralConfig.Color = Color.blue;
            _spiralConfig.DoneSpiralIn += TriggerOcarina;
            _spiralConfig.DoneSpiralOut += FinishTrasition;
            _onSceneLoadedEventChannel.EventRaised += _spiralConfig.OnSpiralOut;
            _spiralConfig.OnSpiralIn();
        }

        private void FinishTrasition()
        {
            _spiralConfig.DoneSpiralIn -= TriggerOcarina;
            _spiralConfig.DoneSpiralOut -= FinishTrasition;
            _onSceneLoadedEventChannel.EventRaised -= _spiralConfig.OnSpiralOut;
        }

        private void TriggerOcarina()
        {
            if (SceneManager.GetSceneByName("WorldMap").isLoaded)
                MoveHeroToPathEntrance(_selectedPath);
            else
            {
                _pathStorage.LastTakenPath = _selectedPath;
                _requestLoadMapEvent.RequestLoad(_worldMapScene);
            }
        }

        private void MoveHeroToPathEntrance(MapPathSO path)
        {
            if (_cachedDestinations.Count == 0)
                _cachedDestinations.AddRange(FindObjectsOfType<GoFrom>());
            foreach (GoFrom destination in _cachedDestinations)
            {
                if (path == destination.MapPath)
                {
                    HeroBehaviour hero = FindObjectOfType<HeroBehaviour>();
                    hero.transform.position = destination.transform.position;
                    _spiralConfig.OnSpiralOut();
                    break;
                }
            }
        }
    }
}