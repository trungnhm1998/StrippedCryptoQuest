using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Input;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CryptoQuest.Gameplay.Battle
{
    public class EncounterZone : MonoBehaviour
    {
        [SerializeField] private TriggerBattleEncounterEventSO _triggerBattleEncounterEvent;
        [SerializeField] private List<BattleEncounterSetup> _battleEncounterSetups;
        [SerializeField] private float _encounterRate = 5f;
        [SerializeField] private SceneScriptableObject _battleSceneSO;
        [SerializeField] private bool _isBattleEscapable = true;

        [Header("Area Configuration")]
        [SerializeField, ReadOnly] private string _playerTag = "Player";

        [SerializeField] private float _customRatio = 1.7f;
        [SerializeField] private InputMediatorSO _inputMediatorSO;
        [SerializeField] private BoxCollider2D _collider;
        private Vector2 _playerPosition;
        private float _countdown;
        private BattleEncounterSetup _currentBattleSetup;


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
            SetUpBattleToInit();
            BattleInfo currentBattleInfo =
                new(_battleSceneSO, _currentBattleSetup.BattleData, _isBattleEscapable);
            OnTriggerBattleEncounter(currentBattleInfo);
        }

        private void SetUpBattleToInit()
        {
            float randomValue = Random.value;
            for (int i = 0; i < _battleEncounterSetups.Count; i++)
            {
                randomValue -= _battleEncounterSetups[i].Probability;
                if (randomValue <= 0 || i == _battleEncounterSetups.Count - 1)
                {
                    _currentBattleSetup = _battleEncounterSetups[i];
                    break;
                }
            }
        }

        private void InitCountDown()
        {
            float encounterRateBuff = BattleCalculator.CalculateEncounterRateBuff(0, 0);
            _countdown = Random.Range(3, _encounterRate) * encounterRateBuff;
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