using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using Unity.Collections;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class EncounterZone : MonoBehaviour
    {
        [SerializeField] private TriggerBattleEncounterEventSO _triggerBattleEncounterEvent;
        [SerializeField] private BattleDataSO _battleData;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField, ReadOnly] private string _playerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;

            OnTriggerBattleEncounter();
        }

        private void OnTriggerBattleEncounter()
        {
            _triggerBattleEncounterEvent.Raise(_battleData);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(150, 0, 0, .3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(new Vector3(_collider.offset.x, _collider.offset.y, -2), _collider.size);
        }
    }
}