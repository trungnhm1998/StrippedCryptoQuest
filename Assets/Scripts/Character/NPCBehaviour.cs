using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Character
{
    public class NPCBehaviour : MonoBehaviour, IInteractable
    {
        // TODO: move to generic InteractableBehaviour, should also have the collider in that prefab
        [SerializeField] private UnityEvent _interacted;

        private IDialogueController _dialogueController;
        private NPCFacingDirection _npcFacingDirection; // TODO: violate DIP

        /// <summary>
        /// This will cause a stutter on scene load. Luckily, we have a fade in effect.
        /// </summary>
        private void Awake()
        {
            // var allBehaviours = GetComponents<ICharacterBehaviour>(); // TODO: this might be a better way to get all the behaviours
            // due to sear amount of NPCs in the game, use TryGetComponent instead of GetComponent then add NullDialogueController
            _dialogueController = GetComponent<IDialogueController>();
            _npcFacingDirection = GetComponent<NPCFacingDirection>();
        }

        public void Interact()
        {
            _interacted.Invoke();
            _dialogueController.ShowDialogue();
            _npcFacingDirection.FacePlayer();
        }
    }
}