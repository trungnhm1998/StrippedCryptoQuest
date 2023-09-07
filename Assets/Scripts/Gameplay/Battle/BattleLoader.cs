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
        public static event Action<Battlefield> LoadBattle;
        public static void RequestLoadBattle(Battlefield party) => LoadBattle?.Invoke(party);
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private BattleInputSO _battleInput;
        [SerializeField] private SpiralConfigSO _spiralConfigSo;
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private SceneScriptableObject _battleSceneSO;

        [Header("Events to listen to")]
        [SerializeField] private VoidEventChannelSO _onBattleEndEventChannel;

        [Header("Events to raise")]
        [SerializeField] private UnloadSceneEventChannelSO _unloadSceneEvent;
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEventChannelSo;

        [Header("Config"), SerializeField]
        private Battlefield[] _enemyParties = Array.Empty<Battlefield>();

        private void OnEnable()
        {
            _onBattleEndEventChannel.EventRaised += OnBattleEnd;
            LoadBattle += LoadingBattle;
            LoadBattleWithId += LoadingBattle;
        }

        private void OnDisable()
        {
            _onBattleEndEventChannel.EventRaised -= OnBattleEnd;
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
                Debug.LogWarning($"No enemy party with id \"{id}\" found");
                return;
            }

            if (party.EnemyIds.Length == 0)
            {
                Debug.LogWarning($"No enemies in party with id \"{id}\" found");
                return;
            }

            LoadingBattle(party);
        }

        private void LoadingBattle(Battlefield party)
        {
            _gameState.UpdateGameState(EGameState.Battle);
            _battleInput.EnableBattleInput();
            _battleBus.CurrentBattlefield = party;
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