using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Input;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CryptoQuest.Gameplay.Battle
{
    public class EncounterZone : MonoBehaviour
    {
        [SerializeField] private BattleFieldSO _battleField;
        [SerializeField] private TriggerBattleEncounterEventSO _triggerBattleEncounterEvent;

        [Header("Area Configuration")]
        [SerializeField, ReadOnly] private string _playerTag = "Player";

        [SerializeField] private float _customRatio = 1.7f;
        [SerializeField] private InputMediatorSO _inputMediatorSO;
        [SerializeField] private BoxCollider2D _collider;
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
            _inputMediatorSO.EnableMenuInput();
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

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(150, 0, 0, .3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(new Vector3(_collider.offset.x, _collider.offset.y, -2), _collider.size);
        }
    }
}