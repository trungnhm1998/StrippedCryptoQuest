using System;
using System.Collections;
using CryptoQuest.Battle;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.SafeZone;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

namespace CryptoQuest.Gameplay.Encounter
{
    public class EncounterManager : MonoBehaviour
    {
        /// <summary>
        /// For quest, event or simply force by cheat
        /// </summary>
        [SerializeField] private StringEventChannelSO _triggerBattleEncounterEvent;

        [SerializeField] protected GameplayBus _gameplayBus;
        [SerializeField] protected float _minEncounterSteps = 3f;
        [SerializeField] protected float _maxEncounterSteps = 5f; // allow half a step?
        [SerializeField] private EncounterDatabase _database;
        [SerializeField] private GameStateSO _gameState;
        protected EncounterData _currentEncounterData;
        protected float _stepLeftBeforeTriggerBattle;

        private float _encounterBuff = 1;
        public float EncounterBuff 
        { 
            get => _encounterBuff;
            set
            {
                _encounterBuff = value;
                GenerateRandomStepTilNextTrigger();
            } 
        }

        protected virtual void Awake()
        {
            _triggerBattleEncounterEvent.EventRaised += TriggerBattle;
            EncounterZone.LoadingEncounterArea += LoadEncounterZone;
            EncounterZone.EnterEncounterZone += RegisterStepHandler;
            EncounterZone.ExitEncounterZone += RemoveStepHandler;
        }

        protected virtual void OnDestroy()
        {
            _triggerBattleEncounterEvent.EventRaised -= TriggerBattle;
            EncounterZone.LoadingEncounterArea -= LoadEncounterZone;
            EncounterZone.EnterEncounterZone -= RegisterStepHandler;
            EncounterZone.ExitEncounterZone -= RemoveStepHandler;
        }

        private void RemoveStepHandler(string encounterId)
        {
            if (_currentEncounterData == null) return;
            _gameplayBus.Hero.Step -= DecrementStepCountBeforeTriggerBattle;
            _currentEncounterData = null; // either null or new EncounterData()
        }

        private void RegisterStepHandler(string encounterId)
            => StartCoroutine(GetEncounter(encounterId, SetupStepsCounter));

        protected virtual void SetupStepsCounter(EncounterData encounter)
        {
            _currentEncounterData = encounter;
            _maxEncounterSteps = _currentEncounterData.EncounterRate;
            GenerateRandomStepTilNextTrigger();
            _gameplayBus.Hero.Step += DecrementStepCountBeforeTriggerBattle;
        }

        protected void LoadEncounterZone(string encounterId)
            => StartCoroutine(_database.LoadDataById(encounterId));


        protected void DecrementStepCountBeforeTriggerBattle()
        {
            if (SafeZoneController.IsSafeZoneActive) return;
            _stepLeftBeforeTriggerBattle--;
            if (_stepLeftBeforeTriggerBattle > 0) return;
            TriggerBattle(_currentEncounterData);
        }

        private void TriggerBattle(EncounterData encounter)
        {
            if (_gameState.CurrentGameState == EGameState.Battle)
            {
                Debug.Log("EncounterManager::TriggerBattle - already in battle");
                return;
            }

            if (encounter == null)
            {
                Debug.LogWarning("EncounterManager::TriggerBattle - encounter is null");
                return;
            }

            GenerateRandomStepTilNextTrigger();
            var battle = encounter.GetRandomBattlefield();
            Debug.Log($"Trigger battle with encounter {encounter.name} - battle {battle.name}");
            BattleLoader.RequestLoadBattle(battle);
        }

        protected void GenerateRandomStepTilNextTrigger()
        {
            _stepLeftBeforeTriggerBattle = Random.Range(_minEncounterSteps, _maxEncounterSteps) * _encounterBuff;
        }

        private void TriggerBattle(string encounterId) => StartCoroutine(GetEncounter(encounterId, TriggerBattle));

        protected IEnumerator GetEncounter(string encounterId, Action<EncounterData> callback)
        {
            yield return _database.LoadDataById(encounterId);
            var handle = _database.GetHandle(encounterId);
            yield return handle;
            if (!handle.IsValid() || handle.Status != AsyncOperationStatus.Succeeded) yield break;
            var encounter = _database.GetDataById(encounterId);
            if (encounter == null)
                yield break;
            callback?.Invoke(encounter);
        }
    }
}