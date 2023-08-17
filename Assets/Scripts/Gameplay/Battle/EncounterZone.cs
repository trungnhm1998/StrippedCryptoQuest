using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class EncounterZone : MonoBehaviour
    {
        [SerializeField] private BattleFieldSO _battleField;
        [SerializeField] private TriggerBattleEncounterEventSO _triggerBattleEncounterEvent;

        [Header("Area Configuration")]
        [SerializeField, ReadOnly] private string _playerTag = "Player";

        [SerializeField] private string _encounterId;
        [SerializeField] private float _customRatio = 1.7f;
        private Vector2 _playerPosition;
        private float _countdown;


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            InitCountDown();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            if ((Vector2)other.transform.position == _playerPosition) return;

            _playerPosition = other.transform.position;
            _countdown -= Time.deltaTime / _customRatio;
            if (_countdown < 0f)
            {
                OnCountdownEnd();
                InitCountDown();
            }
        }

        private void OnCountdownEnd()
        {
            BattleDataSO currentBattData = _battleField.GetBattleToInit();
            BattleInfo currentBattleInfo =
                new(currentBattData, _battleField.IsBattleEscapable, _battleField.BattleBackground);
            OnTriggerBattleEncounter(currentBattleInfo);
        }


        private void InitCountDown()
        {
            float encounterRateBuff = GetBuffsRatio();
            _countdown = Random.Range(3, _battleField.EncounterRate) * encounterRateBuff;
        }

        private float GetBuffsRatio()
        {
            float encounterRateBuff
                = BattleCalculator.CalculateEncounterRateBuff(0, 0);
            return encounterRateBuff;
        }

        private void OnTriggerBattleEncounter(BattleInfo battleInfo)
        {
            _triggerBattleEncounterEvent.Raise(battleInfo);
        }

        /// <summary>
        /// This will be called when SuperTiled2Unity has finished importing the component.
        /// </summary>
        /// <param name="encounterId"></param>
        public void EncounterId(string encounterId)
        {
            _encounterId = encounterId;
        }
    }
}