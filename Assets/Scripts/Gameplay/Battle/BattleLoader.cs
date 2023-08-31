using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
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

        private void OnEnable()
        {
            _onBattleEndEventChannel.EventRaised += OnBattleEnd;
            _onSceneLoadedEventChannel.EventRaised += OnSceneLoaded;
        }

        private void OnDisable()
        {
            _onBattleEndEventChannel.EventRaised -= OnBattleEnd;
            _onSceneLoadedEventChannel.EventRaised -= OnSceneLoaded;
            _spiralConfigSo.DoneSpiralIn -= SpiralInDone;
            _spiralConfigSo.DoneFadeOut -= StartBattle;
        }

        // private void OnEncounterBattle(BattleInfo battleInfo)
        // {
        //     _battleInput.EnableBattleInput();
        //     _battleBus.CurrentBattleInfo = battleInfo;
        //     _spiralConfigSo.Color = Color.black;
        //     _spiralConfigSo.DoneSpiralIn += SpiralInDone;
        //     _spiralConfigSo.DoneFadeOut += StartBattle;
        //     _spiralConfigSo.OnSpiralIn();
        // }

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
            _gameState.UpdateGameState(EGameState.Battle);
            _spiralConfigSo.DoneSpiralIn -= SpiralInDone;
            _spiralConfigSo.DoneFadeOut -= StartBattle;
        }

        private void OnBattleEnd()
        {
            _unloadSceneEvent.RequestUnload(_battleSceneSO);
        }
    }
}