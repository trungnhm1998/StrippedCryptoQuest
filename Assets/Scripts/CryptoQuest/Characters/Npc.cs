using System;
using UnityEngine;

namespace CryptoQuest.Characters
{
    [RequireComponent(typeof(DialogController))]
    public class Npc : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogController _dialogController;

        private string _dialogData;
        public string DialogData => _dialogData;

        public void Interact()
        {
            Debug.Log("Interacting with NPC");
            _dialogData = _dialogController.GetDialogKey();
        }
    }
}