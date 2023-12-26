using System;
using CryptoQuest.Character.Behaviours;
using CryptoQuest.Utils;
using CryptoQuest.Gameplay.Ship;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public interface IShipController
    {
        void Sail(IShip ship);
        void Landing();
    }

    [RequireComponent(typeof(Animator), typeof(AutoMoveToPosition))]
    public class HeroShipBehaviour : MonoBehaviour, IShipController
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AutoMoveToPosition _autoMove;
        [SerializeField] private LayerMask _landLayer;

        private readonly int _idleClip = Animator.StringToHash("Idles");
        private readonly int _shipClip = Animator.StringToHash("Ship");
        private readonly int _inputX = Animator.StringToHash("InputX");
        private readonly int _inputY = Animator.StringToHash("InputY");

        private IShip _sailingShip = new NullShip();

        
        private void OnValidate()
        {
            _animator = GetComponent<Animator>();
            _autoMove = GetComponent<AutoMoveToPosition>();
        }

        public void Sail(IShip ship)
        {
            if (_sailingShip.IsValid() || _autoMove.IsAutoMoving) return;
            _autoMove.AutoMoveTo(ship.GameObject.transform.position, () => 
            {   
                _animator.Play(_shipClip);
                _sailingShip = ship;
                ship.SetSail();
            });
        }

        public void OnTriggerChangeDetected(bool entered, GameObject go)
        {
            if (!_sailingShip.IsValid()) return;
            if (_landLayer.Contains(go.layer))
            {
                Landing();
            }
        }

        public void Landing()
        {
            _animator.Play(_idleClip);
            _sailingShip.SetAnchor();
            
            var dir = new Vector3(_animator.GetFloat(_inputX), _animator.GetFloat(_inputY), 0)
                .normalized;
            _sailingShip = new NullShip();
            _autoMove.AutoMoveTo(transform.position + dir);
        }
    }
}
