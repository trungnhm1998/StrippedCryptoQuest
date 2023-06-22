using UnityEngine;

namespace CryptoQuest.Characters
{
    [RequireComponent(typeof(DialogController))]
    public class Npc : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogController _dialogController;

        [SerializeField] private InteractController _interactController;

        private string _dialogData;
        public string DialogData => _dialogData;

        public void Interact()
        {
            _dialogData = _dialogController.GetDialogKey();
        }
    }
}