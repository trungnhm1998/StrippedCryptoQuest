using CryptoQuest.Gameplay.Ship;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public interface IShipController
    {
        void Sail(IShip ship);
        void Landing();
        IShip SailingShip { get; }
    }

    [RequireComponent(typeof(Animator), typeof(AutoMoveToPosition))]
    public class HeroShipBehaviour : MonoBehaviour, IShipController
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AutoMoveToPosition _autoMove;
        [SerializeField] private LandingShipBehaviour _landingBehaviour;

        private readonly int _idleClip = Animator.StringToHash("Idles");
        private readonly int _shipClip = Animator.StringToHash("Ship");
        private readonly int _inputX = Animator.StringToHash("InputX");
        private readonly int _inputY = Animator.StringToHash("InputY");

        private IShip _sailingShip = new NullShip();
        public IShip SailingShip => _sailingShip;
        
        private void OnValidate()
        {
            _animator = GetComponent<Animator>();
            _autoMove = GetComponent<AutoMoveToPosition>();
            _landingBehaviour = GetComponentInChildren<LandingShipBehaviour>();
        }

        public void Sail(IShip ship)
        {
            if (_sailingShip.IsValid() || _autoMove.IsAutoMoving) return;
            _autoMove.AutoMoveTo(ship.GameObject.transform.position, () => 
            {   
                _animator.Play(_shipClip);
                _sailingShip = ship;
                ship.SetSail();
                _landingBehaviour.gameObject.SetActive(true);
                _landingBehaviour.SetShipBehaviour(this);
            });
        }

        public void Landing()
        {
            _animator.Play(_idleClip);
            _sailingShip.SetAnchor();
            
            var dir = new Vector3(_animator.GetFloat(_inputX), _animator.GetFloat(_inputY), 0)
                .normalized;
            _sailingShip = new NullShip();
            _autoMove.AutoMoveTo(transform.position + dir);
            _landingBehaviour.gameObject.SetActive(false);
        }
    }

    public class NullShipController : IShipController
    {
        public void Sail(IShip ship) { }
        public void Landing() { }
        public IShip SailingShip => new NullShip();
    }
}
