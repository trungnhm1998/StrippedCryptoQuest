using System;
using System.Collections.Generic;
using CryptoQuest.Menus.TownTransfer;
using CryptoQuest.System.SaveSystem;
using CryptoQuest.System.SaveSystem.Loaders;
using IndiGames.Core.Events;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Ocarina
{
    public class TeleportLocationsController : MonoBehaviour
    {
        [SerializeField] private TeleportLocationsSaveSO _saveSo;
        [SerializeField] private OcarinaLocations _ocarinaLocations;
        [SerializeField] private TownTransferLocations _transferLocations;
        [SerializeField] private ConditionalOcarinaLocations _conditionalOcarinaLocations;
        [SerializeField] private ConditionalTransferLocations _conditionalTransferLocations;
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEvent;
        [SerializeField] private RegisterTownToOcarinaEventChannelSO _registerOcarinaTown;
        [SerializeField] private RegisterTownToTransferEventChannelSO _registerTransferTown;
        private List<SceneScriptableObject> _cachedDestinations = new();

        private void OnEnable()
        {
            _loadSceneEvent.LoadingRequested += OnSceneLoaded;
        }

        private void OnDisable()
        {
            _loadSceneEvent.LoadingRequested -= OnSceneLoaded;
        }

        private void Awake()
        {
            foreach (var conditionalOcarinaLocation in _conditionalOcarinaLocations.Locations)
            {
                if (_cachedDestinations.Contains(conditionalOcarinaLocation.RequiredLocations)) continue;
                _cachedDestinations.Add(conditionalOcarinaLocation.RequiredLocations);
            }

            foreach (var conditionalTransferLocation in _conditionalTransferLocations.Locations)
            {
                if (_cachedDestinations.Contains(conditionalTransferLocation.RequiredLocations)) continue;
                _cachedDestinations.Add(conditionalTransferLocation.RequiredLocations);
            }

            LoadConditionalDestinations();
            ActionDispatcher.Dispatch(new DestinationsLoaded());
        }

        private void OnSceneLoaded(SceneScriptableObject scene)
        {
            foreach (var conditionalOcarinaLocation in _conditionalOcarinaLocations.Locations)
            {
                if (scene != conditionalOcarinaLocation.RequiredLocations) continue;
                LoadConditionalDestinations();
                if (!_saveSo.VisitedLocations.Contains(conditionalOcarinaLocation.RequiredLocations.Guid))
                    _registerOcarinaTown.RaiseEvent(conditionalOcarinaLocation.Entrance);

                break;
            }

            foreach (var conditionalTransferLocation in _conditionalTransferLocations.Locations)
            {
                if (scene != conditionalTransferLocation.RequiredLocations) continue;
                LoadConditionalDestinations();
                if (!_saveSo.VisitedLocations.Contains(conditionalTransferLocation.RequiredLocations.Guid))
                    _registerTransferTown.RaiseEvent(conditionalTransferLocation.Entrance);

                break;
            }

            if (_cachedDestinations.Contains(scene))
                _saveSo.AddVisitedLocation(scene.Guid);
        }

        private void LoadConditionalDestinations()
        {
            _ocarinaLocations.ConditionalLocations.Clear();
            foreach (var conditionalOcarinaLocation in _conditionalOcarinaLocations.Locations)
            {
                if (!_saveSo.VisitedLocations.Contains(conditionalOcarinaLocation.RequiredLocations.Guid)) continue;
                _ocarinaLocations.ConditionalLocations.Add(conditionalOcarinaLocation.Entrance);
            }

            _transferLocations.ConditionalLocations.Clear();
            foreach (var conditionalTransferLocation in _conditionalTransferLocations.Locations)
            {
                if (!_saveSo.VisitedLocations.Contains(conditionalTransferLocation.RequiredLocations.Guid)) continue;
                _transferLocations.ConditionalLocations.Add(conditionalTransferLocation.Entrance);
            }
        }
    }
}