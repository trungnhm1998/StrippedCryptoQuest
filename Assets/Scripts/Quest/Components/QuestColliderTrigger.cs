using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class QuestColliderTrigger : MonoBehaviour
    {
        [SerializeField] private QuestSO _questToComplete;
        [SerializeField] private QuestEventChannelSO _questGiverChannelSo;

        [SerializeField] private ECollideActionType _actionType;
        [SerializeField] private BoxCollider2D _collider2D;

        public void SetQuestData(QuestSO questData) => _questToComplete = questData;

        public void SetColliderBoxSize(Vector2 size) => _collider2D.size = size;

        private void OnTriggerEnter2D(Collider2D other) => GiveQuest(other, ECollideActionType.OnEnter);

        private void OnTriggerExit2D(Collider2D other) => GiveQuest(other, ECollideActionType.OnExit);

        private void GiveQuest(Collider2D other, ECollideActionType collideType)
        {
            if (!other.CompareTag("Player")) return;
            if (_actionType != collideType) return;

            _collider2D.enabled = false;

            _questGiverChannelSo.RaiseEvent(_questToComplete);
        }

        private enum ECollideActionType
        {
            OnEnter = 0,
            OnExit = 1
        }
    }
}