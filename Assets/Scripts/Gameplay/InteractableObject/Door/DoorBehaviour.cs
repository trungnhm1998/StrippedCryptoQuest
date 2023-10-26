using CryptoQuest.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.InteractableObject.Door
{
    public class DoorBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject _interactableObject;
        private bool _isOpenning = false;

        private bool _canInteract = true;

        public void Interact()
        {
            if (!_canInteract) return;
            
            _isOpenning = !_isOpenning;
            _interactableObject.SetActive(_isOpenning);
        }
        
        public void SetInteract(bool canInteract)
        {
            _canInteract = canInteract;
        }
    }
}
