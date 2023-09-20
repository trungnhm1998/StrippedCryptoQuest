using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Input;
using CryptoQuest.Map;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace CryptoQuest.Item.Ocarina
{
    public class OcarinaBehaviour : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _worldMapScene;
        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private PathStorageSO _pathStorage;
        [SerializeField] private InputMediatorSO _inputMediatorSO;

        [Header("Listen")]
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;

        [SerializeField] private OcarinaAbility _ocarinaAbility;
        [SerializeField] private RegisterTownToOcarinaEventChannelSO _registerTownEvent;

        [FormerlySerializedAs("_consumeItemEventChannel")]
        [Header("Raise")]
        [SerializeField] private ConsumableEventChannel _consumedItemEventChannel;

        [SerializeField] private SpiralConfigSO _spiralConfig;

        [SerializeField] private LoadSceneEventChannelSO _requestLoadMapEvent;

        [Header("Ocarina UI")]
        [SerializeField] private GameObject _ocarinaUI;

        private readonly List<GoFrom> _cachedDestinations = new();

        private void Awake()
        {
            _ocarinaUI.SetActive(false);
            _ocarinaAbility.TeleportToTown += StartTeleportSequence;
        }

        private void OnDestroy()
        {
            _ocarinaAbility.TeleportToTown -= StartTeleportSequence;
        }

        private void StartTeleportSequence(MapPathSO path)
        {
            StartCoroutine(CoActivateOcarinaAnim(path));
        }

        private IEnumerator CoActivateOcarinaAnim(MapPathSO location)
        {
            _inputMediatorSO.DisableAllInput();
            _consumedItemEventChannel.RaiseEvent(null); // raise just to fall back state
            _ocarinaUI.SetActive(true);
            yield return _gameplayBus.Hero.ActivateOcarina();
            _ocarinaUI.SetActive(false);
            ShowSpiral();
            yield return new WaitForSeconds(_spiralConfig.Duration);
            // screen should be all obscured by now
            TriggerOcarina(location);
        }

        private void ShowSpiral()
        {
            _spiralConfig.Color = Color.blue;
            _spiralConfig.ShowSpiral();
        }

        private void HideSpiralAfterSceneLoaded()
        {
            StartCoroutine(CoHideSpiralAndEnableMapInput());
            _onSceneLoadedEventChannel.EventRaised -= HideSpiralAfterSceneLoaded;
        }

        private IEnumerator CoHideSpiralAndEnableMapInput()
        {
            _spiralConfig.HideSpiral();
            yield return new WaitForSeconds(_spiralConfig.Duration);
            _inputMediatorSO.EnableMapGameplayInput();
        }

        private void TriggerOcarina(MapPathSO path)
        {
            if (SceneManager.GetSceneByName("WorldMap").isLoaded)
            {
                MoveHeroToLocationEntrance(path);
            }
            else
            {
                _pathStorage.LastTakenPath = path;
                _requestLoadMapEvent.RequestLoad(_worldMapScene);
                _onSceneLoadedEventChannel.EventRaised += HideSpiralAfterSceneLoaded;
            }
        }

        private void MoveHeroToLocationEntrance(MapPathSO path)
        {
            if (_cachedDestinations.Count == 0)
                _cachedDestinations.AddRange(FindObjectsOfType<GoFrom>());
            foreach (GoFrom destination in _cachedDestinations)
            {
                if (path != destination.MapPath) continue;
                _gameplayBus.Hero.transform.position = destination.transform.position;
                StartCoroutine(CoHideSpiralAndEnableMapInput());
                return;
            }
        }
    }
}