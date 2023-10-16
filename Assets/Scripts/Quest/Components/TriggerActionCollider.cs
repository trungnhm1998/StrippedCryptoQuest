using CryptoQuest.Quest.Actions;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class TriggerActionCollider : MonoBehaviour
    {
        [field: SerializeField] public BoxCollider2D BoxCollider2D { get; set; }

        private NextAction _nextAction;
        private ECollideActionType _collideActionType;
        private bool _isRepeatable;

        public void SetAction(NextAction nextAction)
        {
            _nextAction = nextAction;
            BoxCollider2D.enabled = true;
        }
        public void SetBoxSize(Vector2 componentSizeBox) => BoxCollider2D.size = componentSizeBox;
        public void SetRepeatType(bool isRepeatable) => _isRepeatable = isRepeatable;
        public void SetCollideActionType(ECollideActionType collideActionType) => _collideActionType = collideActionType;
        private void OnTriggerEnter2D(Collider2D other) => Execute(other, ECollideActionType.OnEnter);
        private void OnTriggerExit2D(Collider2D other) => Execute(other, ECollideActionType.OnExit);

        private void Execute(Collider2D other, ECollideActionType collideType)
        {
            if (!other.CompareTag("Player")) return;

            if (!BoxCollider2D.enabled) return;

            if (_collideActionType != collideType) return;

            BoxCollider2D.enabled = _isRepeatable;

            StartCoroutine(_nextAction.Execute());
        }
    }
}