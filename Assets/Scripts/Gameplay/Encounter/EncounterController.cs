using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation;
using UnityEngine;

namespace CryptoQuest.Gameplay.Encounter
{
    public class EncounterController : MonoBehaviour
    {
        /// <summary>
        /// For quest, event or simply force by cheat
        /// </summary>
        [SerializeField] private StringEventChannelSO _triggerBattleEncounterEvent;

        [SerializeField] private EncounterDatabase _database;
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
            _triggerBattleEncounterEvent.EventRaised += TriggerBattle;
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
            hero.Step += OnUpdateStepBeforeTriggerBattle;
            _currentEncounterData = _database.GetEncounterData(encounterId);
            _maxEncounterSteps = _currentEncounterData.EncounterRate;
            GenerateRandomStepTilNextTrigger();
        }

        private void LoadEncounterZone(string encounterId)
        {
            _database.PreloadEncounter(encounterId);
        }

        private float _stepLeftBeforeTriggerBattle;
        private float _maxEncounterSteps = 5f; // allow half a step?

        private void OnUpdateStepBeforeTriggerBattle()
        {
            _stepLeftBeforeTriggerBattle--;
            if (_stepLeftBeforeTriggerBattle <= 0)
            {
                GenerateRandomStepTilNextTrigger();
                TriggerBattle(_currentEncounterData.ID);
            }
        }

        private void GenerateRandomStepTilNextTrigger()
        {
            _stepLeftBeforeTriggerBattle = Random.Range(3, _maxEncounterSteps);
        }

        private void TriggerBattle(string encounterId)
        {
            var battleId = _database.GetEncounterData(encounterId);
            Debug.Log($"Triggering battle with encounter {encounterId} {battleId.name}");
            
        }

        private void LoadBattleWithData(string battleId)
        {
            // if (!_database.TryGetBattlefield(battleId, out var _battleFieldsDict))
            // {
            //     Debug.Log("Battle Id not found in database");
            //     return;
            // }

            // BattlefieldSO battlefield = _battleFieldsDict[battleId];
            // BattleDataSO currentBattData = battlefield.GetBattleToInit();
            // BattleInfo currentBattleInfo =
            //     new(currentBattData, battlefield.IsEscapable, battlefield.Background);
            // _triggerBattleEncounterEvent.Raise(currentBattleInfo);
        }

        private void HandleBattleEncounterConfigureRequest(string battleId)
        {
            // if (!_database.TryGetBattlefield(battleId, out var _battleFieldsDict))
            // {
            //     Debug.Log("Battle Id not found in database");
            //     return;
            // }
            //
            // BattlefieldSO battlefield = _battleFieldsDict[battleId];
            // float encounterRateBuff = GetBuffsRatio();
            // float _countdown = Random.Range(3, battlefield.EncounterRate) * encounterRateBuff;
            // _encounterRateConfigResponseEvent.RaiseEvent(_countdown);
        }

        private float GetBuffsRatio()
        {
            float encounterRateBuff
                = BattleCalculator.CalculateEncounterRateBuff(0, 0);
            return encounterRateBuff;
        }
    }
}