using CryptoQuest.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.InteractableObject.Door
{
    public class DoorBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject _interactableObject;
        private bool _isOpenning = false;

        public void Interact()
        {
            _isOpenning = !_isOpenning;
            _interactableObject.SetActive(_isOpenning);
        }
    }
}
