using CryptoQuest.Quest.Actions;
using UnityEngine;
using ECollideActionType = CryptoQuest.Quest.Components.QuestColliderTrigger.ECollideActionType;

namespace CryptoQuest.Quest.Components
{
    public class TriggerActionCollider : MonoBehaviour
    {
        [field: SerializeField] private BoxCollider2D _collider2D { get; set; }

        private NextAction _nextAction;
        private ECollideActionType _collideActionType;

        public void SetBoxSize(Vector2 componentSizeBox) => _collider2D.size = componentSizeBox;

        public void SetAction(NextAction nextAction)
        {
            _nextAction = nextAction;
            _collider2D.enabled = true;
        }

        public void SetCollideActionType(ECollideActionType collideActionType) =>
            _collideActionType = collideActionType;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_collideActionType == ECollideActionType.OnEnter)
                Execute();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_collideActionType == ECollideActionType.OnExit)
                Execute();
        }

        private void Execute()
        {
            StartCoroutine(_nextAction.Execute());
            _collider2D.enabled = false;
        }
    }
}