using CryptoQuest.Quest.Actions;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class TriggerActionCollider : MonoBehaviour
    {
        private NextAction _nextAction;
        private QuestColliderTrigger.ECollideActionType _collideActionType;
        public void SetAction(NextAction nextAction) => _nextAction = nextAction;

        public void SetCollideActionType(QuestColliderTrigger.ECollideActionType collideActionType) =>
            _collideActionType = collideActionType;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_collideActionType == QuestColliderTrigger.ECollideActionType.OnEnter)
                Execute();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_collideActionType == QuestColliderTrigger.ECollideActionType.OnExit)
                Execute();
        }

        private void Execute()
        {
            StartCoroutine(_nextAction.Execute());
        }
    }
}