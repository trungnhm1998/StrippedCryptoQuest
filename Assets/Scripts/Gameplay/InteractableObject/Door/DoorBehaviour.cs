using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character;
using UnityEngine;

namespace CryptoQuest.Scripts.Gameplay.InteractableObject
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
