using System;
using System.Collections.Generic;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Events;
using CryptoQuest.Map;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace CryptoQuest
{
    public class OcarinaBehaviour : MonoBehaviour
    {
        [SerializeField] private PathStorageSO _pathStorage;
        [SerializeField] private LoadSceneEventChannelSO _requestLoadMapEvent;
        [SerializeField] private SceneScriptableObject _worldMapScene;
        [SerializeField] private MapPathEventChannelSO _destinationSelectedEvent;
        [SerializeField] private VoidEventChannelSO _destinationConfirmEvent;

        private MapPathSO _selectedPath;
        private List<GoFrom> _cachedDestinations = new();

        private void OnEnable()
        {
            _destinationSelectedEvent.EventRaised += SelectDestination;
            _destinationConfirmEvent.EventRaised += ConfirmUseOcarina;
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
                _cachedDestinations = new List<GoFrom>(FindObjectsOfType<GoFrom>());
            foreach (GoFrom destination in _cachedDestinations)
            {
                if (path == destination.MapPath)
                {
                    HeroBehaviour hero = GameObject.FindObjectOfType<HeroBehaviour>();
                    hero.transform.position = destination.transform.position;
                    break;
                }
            }
        }
    }
}