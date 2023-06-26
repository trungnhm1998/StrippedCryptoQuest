using System;
using UnityEngine;

namespace CryptoQuest.Characters
{
    public class Npc : MonoBehaviour, IInteractable
    {
        private IDialog _dialogController = NullDialog.Instance;

        private void Awake()
        {
            _dialogController = GetComponent<IDialog>();
        }

        public void Interact()
        {
            _dialogController.ShowDialog();
        }
    }
}