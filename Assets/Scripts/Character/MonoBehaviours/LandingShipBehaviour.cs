using System;
using CryptoQuest.Character.Behaviours;
using CryptoQuest.Utils;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LandingShipBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private LayerMask _landLayer;
        [SerializeField] private float _castDistance = 0.1f;
        [SerializeField] private float _boxSize = 0.2f;
        private IShipController _shipController = new NullShipController();
        private readonly int _inputX = Animator.StringToHash("InputX");
        private readonly int _inputY = Animator.StringToHash("InputY");

        public void SetShipBehaviour(IShipController controller)
        {
            _shipController = controller;
        }

        private bool IsSafeToLand()
        {
            var dir = new Vector3(_animator.GetFloat(_inputX), _animator.GetFloat(_inputY), 0)
                .normalized;
            // use overlap 2d to avoid collision with other ships in the same layers
            var colliders = Physics2D.OverlapBoxAll(transform.position + dir * _castDistance, Vector2.one * _boxSize, 
                0);
            foreach (var collider in colliders)
            {
                if (_landLayer.Contains(collider.gameObject.layer) || !collider.isTrigger)
                    return false;
            }
            return true;
        }

        public void Landing()
        {
            if (!IsSafeToLand()) return;

            _shipController.Landing();
            _shipController = new NullShipController();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!_shipController.SailingShip.IsValid()) return;
            if (_landLayer.Contains(other.gameObject.layer))
            {
                Landing();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // draw raycast box for landing
            var dir = new Vector3(_animator.GetFloat(_inputX), _animator.GetFloat(_inputY), 0)
                .normalized;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + dir * _castDistance, Vector2.one * _boxSize);
        }
#endif
    }
}
