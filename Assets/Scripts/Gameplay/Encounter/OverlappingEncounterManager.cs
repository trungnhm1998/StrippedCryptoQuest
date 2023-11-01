using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Battle;

namespace CryptoQuest.Gameplay.Encounter
{
    public class OverlappingEncounterManager : EncounterManager
    {
        private List<EncounterInfo> _currentEncounterInfos = new();
        private int _currentPriority = -1;

        protected override void Awake()
        {
            OverlappingEncounterZone.LoadingEncounterArea += LoadEncounterZone;
            OverlappingEncounterZone.EnterEncounterZone += RegisterStepHandler;
            OverlappingEncounterZone.ExitEncounterZone += RemoveStepHandler;
            OverlappingEncounterZone.RegisterEncounterInfo += RegisterEncounterInfo;
            OverlappingEncounterZone.UnregisterEncounterInfo += UnregisterEncounterInfo;
        }

        protected override void OnDestroy()
        {
            OverlappingEncounterZone.LoadingEncounterArea -= LoadEncounterZone;
            OverlappingEncounterZone.EnterEncounterZone -= RegisterStepHandler;
            OverlappingEncounterZone.ExitEncounterZone -= RemoveStepHandler;
            OverlappingEncounterZone.RegisterEncounterInfo -= RegisterEncounterInfo;
            OverlappingEncounterZone.UnregisterEncounterInfo -= UnregisterEncounterInfo;
        }

        private void RegisterStepHandler()
        {
            EncounterInfo highestPriorityEncounter = GetHighestPriorityEncounter();
            StartCoroutine(GetEncounter(highestPriorityEncounter.EncounterId, SetupStepsCounter));
        }

        protected override void SetupStepsCounter(EncounterData encounter)
        {
            _currentEncounterData = encounter;
            _maxEncounterSteps = _currentEncounterData.EncounterRate;
            if (_stepLeftBeforeTriggerBattle > 0) return;
            GenerateRandomStepTilNextTrigger();
            _gameplayBus.Hero.Step += DecrementStepCountBeforeTriggerBattle;
        }

        private void RemoveStepHandler()
        {
            _currentEncounterData = null;
            HandleExitZone();
        }

        private void RegisterEncounterInfo(EncounterInfo encounterInfo)
        {
            _currentEncounterInfos.Add(encounterInfo);
            if (_currentPriority < encounterInfo.Priority)
                _currentPriority = encounterInfo.Priority;
        }

        private void UnregisterEncounterInfo(EncounterInfo encounterInfo)
        {
            _currentEncounterInfos.Remove(encounterInfo);
        }

        private void HandleExitZone()
        {
            if (_currentEncounterInfos.Count == 0)
            {
                _currentPriority = -1;
                _gameplayBus.Hero.Step -= DecrementStepCountBeforeTriggerBattle;
                _stepLeftBeforeTriggerBattle = 0;
                return;
            }

            _currentPriority = _currentEncounterInfos.Max(x => x.Priority);
            EncounterInfo highestPriorityEncounter = GetHighestPriorityEncounter();
            StartCoroutine(GetEncounter(highestPriorityEncounter.EncounterId, SetupStepsCounter));
        }

        private EncounterInfo GetHighestPriorityEncounter()
        {
            int maxPriority = _currentEncounterInfos.Max(x => x.Priority);
            EncounterInfo encounterInfo = _currentEncounterInfos.FirstOrDefault(x => x.Priority == maxPriority);

            return encounterInfo;
        }
    }
}