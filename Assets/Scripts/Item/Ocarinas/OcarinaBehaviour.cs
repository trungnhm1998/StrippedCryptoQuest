using System;
using CryptoQuest.Events;
using CryptoQuest.Map;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
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
            _pathStorage.LastTakenPath = _selectedPath;
            _requestLoadMapEvent.RequestLoad(_worldMapScene);
        }
    }
}