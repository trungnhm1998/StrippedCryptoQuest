using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle;
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

        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private float _minEncounterSteps = 3f;
        [SerializeField] private float _maxEncounterSteps = 5f; // allow half a step?
        [SerializeField] private EncounterDatabase _database;
        [SerializeField] private GameStateSO _gameState;

        [Header("Debug")] [SerializeField] private List<EncounterInfo> _currentEncounterInfos = new();
        [SerializeField] private EncounterData _currentEncounterData;
        [SerializeField] private int _currentPriority = -1;
        [SerializeField] private float _stepLeftBeforeTriggerBattle;

        private void Awake()
        {
            _triggerBattleEncounterEvent.EventRaised += TriggerBattle;
            EncounterZone.LoadingEncounterArea += LoadEncounterZone;
            EncounterZone.EnterEncounterZone += RegisterStepHandler;
            EncounterZone.ExitEncounterZone += RemoveStepHandler;
            EncounterZone.RegisterEncounterInfo += RegisterEncounterInfo;
            EncounterZone.UnregisterEncounterInfo += UnregisterEncounterInfo;
        }

        private void OnDestroy()
        {
            _triggerBattleEncounterEvent.EventRaised -= TriggerBattle;
            EncounterZone.LoadingEncounterArea -= LoadEncounterZone;
            EncounterZone.EnterEncounterZone -= RegisterStepHandler;
            EncounterZone.ExitEncounterZone -= RemoveStepHandler;
            EncounterZone.RegisterEncounterInfo -= RegisterEncounterInfo;
            EncounterZone.UnregisterEncounterInfo -= UnregisterEncounterInfo;
        }

        private void RegisterStepHandler()
        {
            EncounterInfo highestPriorityEncounter = GetHighestPriorityEncounter();
            StartCoroutine(GetEncounter(highestPriorityEncounter.EncounterId, SetupStepsCounter));
        }

        private void RemoveStepHandler()
        {
            _currentEncounterData = null; // either null or new EncounterData()
            _gameplayBus.Hero.Step -= DecrementStepCountBeforeTriggerBattle;
            _stepLeftBeforeTriggerBattle = 0;
            HandleExitZone();
        }


        private EncounterInfo GetHighestPriorityEncounter()
        {
            int maxPriority = _currentEncounterInfos.Max(x => x.Priority);
            EncounterInfo encounterInfo = _currentEncounterInfos.FirstOrDefault(x => x.Priority == maxPriority);

            return encounterInfo;
        }

        private void SetupStepsCounter(EncounterData encounter)
        {
            _currentEncounterData = encounter;
            _maxEncounterSteps = _currentEncounterData.EncounterRate;
            if (_stepLeftBeforeTriggerBattle > 0) return;
            GenerateRandomStepTilNextTrigger();
            _gameplayBus.Hero.Step += DecrementStepCountBeforeTriggerBattle;
        }

        private void LoadEncounterZone(string encounterId)
            => StartCoroutine(_database.LoadDataById(encounterId));


        private void DecrementStepCountBeforeTriggerBattle()
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

        private void GenerateRandomStepTilNextTrigger()
            => _stepLeftBeforeTriggerBattle = Random.Range(_minEncounterSteps, _maxEncounterSteps);

        private void TriggerBattle(string encounterId) => StartCoroutine(GetEncounter(encounterId, TriggerBattle));

        private IEnumerator GetEncounter(string encounterId, Action<EncounterData> callback)
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

        #region Area Priority

        private void RegisterEncounterInfo(EncounterInfo encounterInfo)
        {
            _currentEncounterInfos.Add(encounterInfo);
            if (_currentPriority < encounterInfo.Priority)
                _currentPriority = encounterInfo.Priority;
        }

        private void UnregisterEncounterInfo(EncounterInfo encounterInfo)
        {
            if (_currentEncounterInfos.Contains(encounterInfo))
                _currentEncounterInfos.Remove(encounterInfo);
        }

        private void HandleExitZone()
        {
            if (_currentEncounterInfos.Count == 0)
            {
                _currentPriority = -1;
                return;
            }

            _currentPriority = _currentEncounterInfos.Max(x => x.Priority);
            EncounterInfo highestPriorityEncounter = GetHighestPriorityEncounter();
            var encounterData = _database.GetDataById(highestPriorityEncounter.EncounterId);
            SetupStepsCounter(encounterData);
        }

        #endregion
    }
}