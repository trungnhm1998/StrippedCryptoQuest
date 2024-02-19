using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character.MonoBehaviours;
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
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Ocarina
{
    public class OcarinaBehaviour : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _worldMapScene;
        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private PathStorageSO _pathStorage;
        [SerializeField] private InputMediatorSO _inputMediatorSO;
        [SerializeField] private OcarinaLocations _ocarinaData;
        [SerializeField] private SpiralConfigSO _spiralConfig;
        [SerializeField] private FadeConfigSO _fadeConfig;
        [SerializeField] private Color _transitionColor = Color.black;

        [Header("Listen")] [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;

        [SerializeField] private RegisterTownToOcarinaEventChannelSO _registerTownEvent;

        [Header("Raise")] [SerializeField] private LoadSceneEventChannelSO _requestLoadMapEvent;

        [SerializeField] private UnityEvent _onOcarinaTriggered;

        [Header("Ocarina UI")] [SerializeField]
        private GameObject _ocarinaUI;

        private readonly List<GoFrom> _cachedDestinations = new();

        private void Awake()
        {
            _ocarinaUI.SetActive(false);
        }

        public void StartTeleportSequence(MapPathSO path)
        {
            StartCoroutine(CoActivateOcarinaAnim(path));
        }

        private IEnumerator CoActivateOcarinaAnim(MapPathSO location)
        {
            _inputMediatorSO.DisableAllInput();
            _ocarinaUI.SetActive(true);
            var heroController = _gameplayBus.Hero.GetComponent<HeroController>();
            string animationClipName = "Hero_Ocarina";
            yield return heroController.CoPlayAnimation(animationClipName);
            _ocarinaUI.SetActive(false);

            // screen should be all  by now
            TriggerOcarina(location);
        }


        private void TriggerOcarina(MapPathSO path)
        {
            ActionDispatcher.Dispatch(new TriggerTransitionAction(path));
            _onOcarinaTriggered?.Invoke();
        }
    }
}