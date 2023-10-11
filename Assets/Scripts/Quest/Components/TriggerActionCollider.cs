using CryptoQuest.Quest.Actions;
using UnityEngine;
using ECollideActionType = CryptoQuest.Quest.Components.QuestColliderTrigger.ECollideActionType;

namespace CryptoQuest.Quest.Components
{
    public class TriggerActionCollider : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _collider2D;

        private NextAction _nextAction;
        private ECollideActionType _collideActionType;

        public void SetAction(NextAction nextAction, Vector2 dataSizeBox)
        {
            _nextAction = nextAction;

            _collider2D.size = dataSizeBox;
            _collider2D.enabled = true;
        }

        public void SetCollideActionType(ECollideActionType collideActionType) =>
            _collideActionType = collideActionType;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_collideActionType != ECollideActionType.OnEnter) return;
            Execute();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_collideActionType != ECollideActionType.OnExit) return;
            Execute();
        }

        private void Execute()
        {
            StartCoroutine(_nextAction.Execute());
            _collider2D.enabled = false;
        }
    }
}