using System;
using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Characters
{
    [RequireComponent(typeof(DialogController))]
    public class Npc : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogController _dialogController;
        [SerializeField] private InputMediatorSO _interactController;


        private string _dialogData;
        public string DialogData => _dialogData;

        private void Start()
        {
            _interactController.EnableMapGameplayInput();
        }

        private void OnEnable()
        {
            _interactController.InteractEvent += Interact;
        }

        private void OnDisable()
        {
            _interactController.InteractEvent -= Interact;
        }


        public void Interact()
        {
            _dialogData = _dialogController.GetDialogKey();
        }
    }
}