using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Characters
{
    [RequireComponent(typeof(DialogController), typeof(BoxCollider2D))]
    public class Npc : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogController _dialogController;
        [SerializeField] private InputMediatorSO _interactController;

        private string _dialogData;
        public string DialogData => _dialogData;

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
            Debug.Log("Interacting with NPC");
            _dialogData = _dialogController.GetDialogKey();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _interactController.EnableMapGameplayInput();
                // TODO: Show UI
            }
        }
    }
}