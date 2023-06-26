using UnityEngine;

namespace CryptoQuest.Characters
{
    public class Npc : MonoBehaviour, IInteractable
    {
        private IDialog _dialogController = NullDialog.Instance;

        private string _dialogData;
        public string DialogData => _dialogData;

        private void Awake()
        {
            _dialogController = GetComponent<IDialog>();
        }

        public void Interact()
        {
            _dialogData = _dialogController.GetDialogKey();
        }
    }

}