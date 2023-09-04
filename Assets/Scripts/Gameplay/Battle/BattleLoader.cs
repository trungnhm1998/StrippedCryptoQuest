using System;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Input;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleLoader : MonoBehaviour
    {
        public static event Action<int> LoadBattleWithId;
        public static void RequestLoadBattle(int id) => LoadBattleWithId?.Invoke(id);
        public static event Action<EnemyParty> LoadBattle;
        public static void RequestLoadBattle(EnemyParty party) => LoadBattle?.Invoke(party);
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private BattleInputSO _battleInput;
        [SerializeField] private SpiralConfigSO _spiralConfigSo;
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private SceneScriptableObject _battleSceneSO;

        [Header("Events to listen to")]
        [SerializeField] private VoidEventChannelSO _onBattleEndEventChannel;
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;

        [Header("Events to raise")]
        [SerializeField] private UnloadSceneEventChannelSO _unloadSceneEvent;
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEventChannelSo;

        [Header("Config"), SerializeField]
        private EnemyParty[] _enemyParties = Array.Empty<EnemyParty>();

        private void OnEnable()
        {
            _onBattleEndEventChannel.EventRaised += OnBattleEnd;
            _onSceneLoadedEventChannel.EventRaised += OnSceneLoaded;
            LoadBattle += LoadingBattle;
            LoadBattleWithId += LoadingBattle;
        }

        private void OnDisable()
        {
            _onBattleEndEventChannel.EventRaised -= OnBattleEnd;
            _onSceneLoadedEventChannel.EventRaised -= OnSceneLoaded;
            LoadBattle -= LoadingBattle;
            LoadBattleWithId -= LoadingBattle;

            _spiralConfigSo.DoneSpiralIn -= SpiralInDone;
            _spiralConfigSo.DoneFadeOut -= StartBattle;
        }

        private void LoadingBattle(int id)
        {
            var party = Array.Find(_enemyParties, enemyParty => enemyParty.Id == id);
            if (party == null)
            {
                Debug.LogError($"No enemy party with id \"{id}\" found");
                return;
            }

            LoadingBattle(party);
        }

        private void LoadingBattle(EnemyParty party)
        {
            _gameState.UpdateGameState(EGameState.Battle);
            _battleInput.EnableBattleInput();
            _battleBus.CurrentEnemyParty = party;
            ShowSpiralAndLoadBattleScene();
        }

        private void ShowSpiralAndLoadBattleScene()
        {
            _spiralConfigSo.Color = Color.black;
            _spiralConfigSo.DoneSpiralIn += SpiralInDone;
            _spiralConfigSo.DoneFadeOut += StartBattle;
            _spiralConfigSo.ShowSpiral();
        }

        private void SpiralInDone()
        {
            _loadSceneEventChannelSo.RequestLoad(_battleSceneSO);
        }

        private void OnSceneLoaded()
        {
            _spiralConfigSo.OnFadeOut();
        }

        private void StartBattle()
        {
            _spiralConfigSo.DoneSpiralIn -= SpiralInDone;
            _spiralConfigSo.DoneFadeOut -= StartBattle;
        }

        private void OnBattleEnd()
        {
            _unloadSceneEvent.RequestUnload(_battleSceneSO);
        }
    }
}