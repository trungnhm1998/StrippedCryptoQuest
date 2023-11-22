using CryptoQuest.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.InteractableObject.Door
{
    public class DoorBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject _interactableObject;
        private bool _isClosed = true;

        private bool _canInteract = true;

        public void Interact()
        {
            if (!_canInteract) return;

            _isClosed = !_isClosed;
            _interactableObject.SetActive(_isClosed);
        }

        public void SetInteract(bool canInteract)
        {
            _canInteract = canInteract;
        }
    }
}