using System;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Gameplay.Reward.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class BattleLoader : MonoBehaviour
    {
        public static event Action<int> LoadBattleWithId;
        public static void RequestLoadBattle(int id) => LoadBattleWithId?.Invoke(id);
        public static event Action<Battlefield> LoadBattle;
        public static void RequestLoadBattle(Battlefield party) => LoadBattle?.Invoke(party);
        
        [SerializeField] private BattleInput _battleInput;
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private SpiralConfigSO _spiralConfigSo;
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private SceneScriptableObject _battleSceneSO;

        [Header("Events to listen to")]
        [Obsolete]
        [SerializeField] private VoidEventChannelSO _onBattleEndEventChannel;

        [SerializeField] private BattleEventSO _endBattleEvent;

        [Header("Events to raise")]
        [SerializeField] private UnloadSceneEventChannelSO _unloadSceneEvent;

        [SerializeField] private LoadSceneEventChannelSO _loadSceneEventChannelSo;
        [Obsolete]
        [SerializeField] private BattleResultEventSO _battleCompletedEvent;
        [SerializeField] private RewardSO _rewardEventChannel;

        [Header("Config"), SerializeField]
        private Battlefield[] _enemyParties = Array.Empty<Battlefield>();

        private void Awake()
        {
            _onBattleEndEventChannel.EventRaised += OnBattleEnd;
            LoadBattle += LoadingBattle;
            LoadBattleWithId += LoadingBattle;

            _endBattleEvent.EventRaised += UnloadBattle;
            AdditiveGameSceneLoader.SceneUnloaded += BattleUnloaded;
        }

        private void OnDestroy()
        {
            _onBattleEndEventChannel.EventRaised -= OnBattleEnd;
            LoadBattle -= LoadingBattle;
            LoadBattleWithId -= LoadingBattle;

            _spiralConfigSo.DoneSpiralIn -= SpiralInDone;
            _spiralConfigSo.DoneFadeOut -= StartBattle;

            _endBattleEvent.EventRaised -= UnloadBattle;
            AdditiveGameSceneLoader.SceneUnloaded -= BattleUnloaded;
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
            _battleInput.DisableAllInput(); // enable battle input when battle is loaded
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

        [Obsolete]
        private void OnBattleEnd()
        {
            NotifyBattleResult();
            _unloadSceneEvent.RequestUnload(_battleSceneSO);
        }

        [Obsolete]
        private void NotifyBattleResult()
        {
            BattleResultInfo result = new()
            {
                IsWin = true, // this depends on the battle result, but for now we assume it's always win
                Battlefield = _battleBus.CurrentBattlefield,
                BattleContext = _context
            };
            _battleCompletedEvent.RaiseEvent(result);
        }


        private CompletedContext _context;

        private void UnloadBattle(CompletedContext context)
        {
            _context = context;
            OnBattleEnd();
        }

        [Obsolete]
        private void BattleUnloaded(SceneScriptableObject scene)
        {
            if (scene != _battleSceneSO) return;
            _gameState.UpdateGameState(_gameState.PreviousGameState);
            _battleInput.EnableMapGameplayInput();
            _rewardEventChannel.RewardRaiseEvent(_context.Loots);
        }
    }
}