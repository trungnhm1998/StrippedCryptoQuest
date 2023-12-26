using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Manager;
using CryptoQuest.Input;
using CryptoQuest.Map;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Menus.TownTransfer
{
    public class TownTransferBehaviour : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _worldMapScene;
        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private PathStorageSO _pathStorage;
        [SerializeField] private InputMediatorSO _inputMediatorSO;
        [SerializeField] private TownTransferLocations _transferLocations;
        [SerializeField] private FadeConfigSO _fadeConfig;
        [SerializeField] private Color _transitionColor = Color.black;

        [Header("Listen")]
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;


        [Header("Raise")]
        [SerializeField] private SpiralConfigSO _spiralConfig;

        [SerializeField] private LoadSceneEventChannelSO _requestLoadMapEvent;

        private readonly List<GoFrom> _cachedDestinations = new();


        private void RegisterTown(TownTransferPath location)
        {
            _transferLocations.Locations.Add(location);
        }

        public void StartTeleportSequence(MapPathSO path)
        {
            _inputMediatorSO.DisableAllInput();
            TriggerOcarina(path);
        }


        private void TriggerOcarina(MapPathSO path)
        {
            ActionDispatcher.Dispatch(new TriggerTransitionAction(path));
        }
    }
}