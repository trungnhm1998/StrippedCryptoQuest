using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.UI.SpiralFX;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleLoader : MonoBehaviour
    {
        [SerializeField] private BattleInputSO _battleInput;
        [SerializeField] private SpiralConfigSO _spiralConfigSo;
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private SceneScriptableObject _battleSceneSO;

        [Header("Events to listen to")]
        [SerializeField] private TriggerBattleEncounterEventSO _triggerBattleEncounterEventSo;

        [SerializeField] private VoidEventChannelSO _onBattleEndEventChannel;
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;

        [Header("Events to raise")]
        [SerializeField] private UnloadSceneEventChannelSO _unloadSceneEvent;

        [SerializeField] private LoadSceneEventChannelSO _loadSceneEventChannelSo;

        [SerializeField] private VoidEventChannelSO _startBattleEventChannel;

        private void OnEnable()
        {
            _triggerBattleEncounterEventSo.EncounterBattle += OnEncounterBattle;
            _onBattleEndEventChannel.EventRaised += OnBattleEnd;
            _onSceneLoadedEventChannel.EventRaised += OnSceneLoaded;
        }

        private void OnDisable()
        {
            _triggerBattleEncounterEventSo.EncounterBattle -= OnEncounterBattle;
            _onBattleEndEventChannel.EventRaised -= OnBattleEnd;
            _onSceneLoadedEventChannel.EventRaised -= OnSceneLoaded;
            _spiralConfigSo.DoneSpiralIn -= SpiralInDone;
            _spiralConfigSo.DoneFadeOut -= StartBattle;
        }

        private void OnEncounterBattle(BattleInfo battleInfo)
        {
            _battleInput.EnableBattleInput();
            _battleBus.CurrentBattleInfo = battleInfo;
            _spiralConfigSo.Color = Color.black;
            _spiralConfigSo.DoneSpiralIn += SpiralInDone;
            _spiralConfigSo.DoneFadeOut += StartBattle;
            _spiralConfigSo.OnSpiralIn();
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
            _startBattleEventChannel.RaiseEvent();
            _spiralConfigSo.DoneSpiralIn -= SpiralInDone;
            _spiralConfigSo.DoneFadeOut -= StartBattle;
        }

        private void OnBattleEnd()
        {
            _unloadSceneEvent.RequestUnload(_battleSceneSO);
        }
    }
}