using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle;
using UnityEngine;

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
            _stepLeftBeforeTriggerBattle = Random.Range(_minEncounterSteps, _maxEncounterSteps);
        }

        private void TriggerBattle(string encounterId)
        {
            var encounterConfig = _database.GetEncounterData(encounterId);
            Debug.Log($"Triggering battle with encounter {encounterId} {encounterConfig.name}");
            var battle = encounterConfig.GetRandomBattlefield();
        }
    }
}