using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Gameplay.Encounter
{
    public class EncounterManager : MonoBehaviour
    {
        /// <summary>
        /// For quest, event or simply force by cheat
        /// </summary>
        [SerializeField] private StringEventChannelSO _triggerBattleEncounterEvent;

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

        private void RemoveStepHandler(HeroBehaviour hero, string encounterId)
        {
            hero.Step -= OnUpdateStepBeforeTriggerBattle;
            _currentEncounterData = null; // either null or new EncounterData()
        }

        private void RegisterStepHandler(HeroBehaviour hero, string encounterId)
        {
            if (_database.TryGetEncounterData(encounterId, out _currentEncounterData))
            {
                _maxEncounterSteps = _currentEncounterData.EncounterRate;
                GenerateRandomStepTilNextTrigger();
                hero.Step += OnUpdateStepBeforeTriggerBattle;
                return;
            }

            var handle = _database.PreloadEncounter(encounterId);
            if (handle.IsValid() == false) return;
            handle.Completed += asyncHandle =>
            {
                SetupEncounterConfigAfterLoaded(asyncHandle);
                hero.Step += OnUpdateStepBeforeTriggerBattle;
            };
        }

        private void SetupEncounterConfigAfterLoaded(AsyncOperationHandle<EncounterData> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _currentEncounterData = handle.Result;
                _maxEncounterSteps = _currentEncounterData.EncounterRate;
                GenerateRandomStepTilNextTrigger();
            }
            else
            {
                Debug.LogWarning($"Encounter data loaded failed with error {handle.OperationException}");
            }
        }

        private void TriggerBattle(EncounterData encounter)
        {
            if (_gameState.CurrentGameState == EGameState.Battle)
            {
                Debug.Log("EncounterManager::TriggerBattle - already in battle");
                return;
            }

            var battle = encounter.GetRandomBattlefield();
            Debug.Log($"Trigger battle with encounter {encounter.name} - battle {battle.name}");
            BattleLoader.RequestLoadBattle(battle);
        }

        private void LoadEncounterZone(string encounterId)
        {
            _database.PreloadEncounter(encounterId);
        }

        private float _stepLeftBeforeTriggerBattle;

        private void OnUpdateStepBeforeTriggerBattle()
        {
            _stepLeftBeforeTriggerBattle--;
            if (!(_stepLeftBeforeTriggerBattle <= 0)) return;
            GenerateRandomStepTilNextTrigger();
            if (_database.TryGetEncounterData(_currentEncounterData.ID, out var encounter))
                TriggerBattle(encounter);
            else
                TriggerBattleAsync(_currentEncounterData.ID);
        }

        private void GenerateRandomStepTilNextTrigger()
        {
            _stepLeftBeforeTriggerBattle = Random.Range(_minEncounterSteps, _maxEncounterSteps);
        }

        private void TriggerBattleAsync(string encounterId)
        {
            var handle = _database.PreloadEncounter(encounterId);
            if (handle.IsValid() == false) return;
            handle.Completed += InternalTriggerBattleAsync;
        }

        private void InternalTriggerBattleAsync(AsyncOperationHandle<EncounterData> op)
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
                TriggerBattle(op.Result);
            else
                Debug.LogWarning($"Encounter data loaded failed with error {op.OperationException}");
        }

        private void TriggerBattle(string encounterId)
        {
            if (_database.TryGetEncounterData(encounterId, out var encounter))
                TriggerBattle(encounter);
            else
                TriggerBattleAsync(encounterId);
        }
    }
}