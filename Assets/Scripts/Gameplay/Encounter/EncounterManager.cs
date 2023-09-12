using System;
using System.Collections;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CryptoQuest.Gameplay.Encounter
{
    public class EncounterManager : MonoBehaviour
    {
        /// <summary>
        /// For quest, event or simply force by cheat
        /// </summary>
        [SerializeField] private StringEventChannelSO _triggerBattleEncounterEvent;

        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private float _minEncounterSteps = 3f;
        [SerializeField] private float _maxEncounterSteps = 5f; // allow half a step?
        [SerializeField] private EncounterDatabase _database;
        [SerializeField] private GameStateSO _gameState;
        private EncounterData _currentEncounterData;

        private void Awake()
        {
            _triggerBattleEncounterEvent.EventRaised += TriggerBattle;
            EncounterZone.LoadingEncounterArea += LoadEncounterZone;
            EncounterZone.EnterEncounterZone += RegisterStepHandler;
            EncounterZone.ExitEncounterZone += RemoveStepHandler;
        }

        private void OnDestroy()
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

        void SetupStepsCounter(EncounterData encounter)
        {
            _currentEncounterData = encounter;
            _maxEncounterSteps = _currentEncounterData.EncounterRate;
            GenerateRandomStepTilNextTrigger();
            _gameplayBus.Hero.Step += DecrementStepCountBeforeTriggerBattle;
        }

        private void LoadEncounterZone(string encounterId)
            => StartCoroutine(_database.LoadDataById(encounterId));

        private float _stepLeftBeforeTriggerBattle;

        private void DecrementStepCountBeforeTriggerBattle()
        {
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

        private void GenerateRandomStepTilNextTrigger()
            => _stepLeftBeforeTriggerBattle = Random.Range(_minEncounterSteps, _maxEncounterSteps);

        private void TriggerBattle(string encounterId) => StartCoroutine(GetEncounter(encounterId, TriggerBattle));

        private IEnumerator GetEncounter(string encounterId, Action<EncounterData> callback)
        {
            yield return _database.LoadDataById(encounterId);
            var encounter = _database.GetDataById(encounterId);
            if (encounter == null)
                yield break;
            callback?.Invoke(encounter);
        }
    }
}