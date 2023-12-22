using CryptoQuest.Character;
using CryptoQuest.Character.Behaviours;
using CryptoQuest.Character.MonoBehaviours;
using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Ship
{
    public interface IShip
    {
        GameObject GameObject { get; }
        void SetAnchor();
        void SetSail();
        bool IsValid();
    }
    
    public class ShipBehaviour : CharacterBehaviour, IInteractableOnTouch, IShip
    {
        public event Action<ShipBehaviour> SailedShip;

        [SerializeField] private ShipBus _shipBus;

        private GameObject _controller;
        public GameObject GameObject => gameObject;

        public void Interact(GameObject controller)
        {
            if (!controller.TryGetComponent<IShipController>(out var shipController)) return;

            shipController.Sail(this);
            _controller = controller;
        }

        public void SetSail()
        {
            gameObject.SetActive(false);
            SailedShip?.Invoke(this);
        }

        public bool IsValid() => gameObject != null;

        public void SetAnchor()
        {
            gameObject.SetActive(true);
            transform.position = _controller.transform.position;
            if (!_controller.TryGetComponent<FacingBehaviour>(out var facingBehaviour))
                return;
            SetFacingDirection(facingBehaviour.FacingDirection);
            _shipBus.LastPosition = new SerializableVector2(transform.position);
            _shipBus.HasSailed = true;
        }
    }

    public class NullShip : IShip
    {
        public GameObject GameObject => null;
        public void SetAnchor() { }
        public void SetSail() { }
        public bool IsValid() => false;
    }
}
