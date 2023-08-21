using System.Collections.Generic;
using CryptoQuest.Data;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CryptoQuest
{
    public class EncounterController : MonoBehaviour
    {
        [SerializeField] private BattleFieldsDatabase _battleFieldsDatabase;

        [Header("Listen to Events")]
        [SerializeField] private StringEventChannelSO _encounterRequestEvent;

        [SerializeField] private StringEventChannelSO _battleEncounterConfigureEvent;

        [Header("Raise Events")]
        [SerializeField] private FloatEventChannelSO _encounterRateConfigResponseEvent;

        [SerializeField] private TriggerBattleEncounterEventSO _triggerBattleEncounterEvent;
        private Dictionary<string, BattleFieldSO> _battleFieldsDict = new();

        private void Awake()
        {
            foreach (var battleField in _battleFieldsDatabase.BattleFields)
            {
                if (string.IsNullOrEmpty(battleField.BattleFieldId)) continue;
                _battleFieldsDict.Add(battleField.BattleFieldId, battleField);
            }
        }

        private void OnEnable()
        {
            _encounterRequestEvent.EventRaised += HandleEncounterRequest;
            _battleEncounterConfigureEvent.EventRaised += HandleBattleEncounterConfigureRequest;
        }

        private void OnDisable()
        {
            _encounterRequestEvent.EventRaised -= HandleEncounterRequest;
            _battleEncounterConfigureEvent.EventRaised -= HandleBattleEncounterConfigureRequest;
        }

        private void HandleEncounterRequest(string battleId)
        {
            if (!_battleFieldsDict.ContainsKey(battleId))
            {
                Debug.Log("Battle Id not found in database");
                return;
            }

            BattleFieldSO battleField = _battleFieldsDict[battleId];
            BattleDataSO currentBattData = battleField.GetBattleToInit();
            BattleInfo currentBattleInfo =
                new(currentBattData, battleField.IsBattleEscapable, battleField.BattleBackground);
            _triggerBattleEncounterEvent.Raise(currentBattleInfo);
        }

        private void HandleBattleEncounterConfigureRequest(string battleId)
        {
            if (!_battleFieldsDict.ContainsKey(battleId))
            {
                Debug.Log("Battle Id not found in database");
                return;
            }

            BattleFieldSO battleField = _battleFieldsDict[battleId];
            float encounterRateBuff = GetBuffsRatio();
            float _countdown = Random.Range(3, battleField.EncounterRate) * encounterRateBuff;
            _encounterRateConfigResponseEvent.RaiseEvent(_countdown);
        }

        private float GetBuffsRatio()
        {
            float encounterRateBuff
                = BattleCalculator.CalculateEncounterRateBuff(0, 0);
            return encounterRateBuff;
        }
    }
}