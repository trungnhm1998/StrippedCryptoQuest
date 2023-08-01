using CryptoQuest.Character.DialogueProviders;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CryptoQuest.Character
{
    public class NPCBehaviour : MonoBehaviour, IInteractable
    {
        public static event UnityAction<NPCBehaviour> Interacted;

        // TODO: move to generic InteractableBehaviour, should also have the collider in that prefab
        [SerializeField] private UnityEvent _interacted;

        [SerializeField] private ReactionBehaviour _reactionBehaviour;

        private DialogueProviderBehaviour _dialogueProviderBehaviour;
        private NPCFacingDirection _npcFacingDirection; // TODO: violate DIP

        /// <summary>
        /// This will cause a stutter on scene load. Luckily, we have a fade in effect.
        /// </summary>
        private void Awake()
        {
            // var allBehaviours = GetComponents<ICharacterBehaviour>(); // TODO: this might be a better way to get all the behaviours
            // due to sear amount of NPCs in the game, use TryGetComponent instead of GetComponent then add NullDialogueController
            _dialogueProviderBehaviour = GetComponent<DialogueProviderBehaviour>();
            _npcFacingDirection = GetComponent<NPCFacingDirection>();
        }

        public void Interact()
        {
            Interacted?.Invoke(this);
            _interacted.Invoke();
            if (_dialogueProviderBehaviour) _dialogueProviderBehaviour.ShowDialogue();
            if (_npcFacingDirection) _npcFacingDirection.FacePlayer();
        }

        public void ShowReaction(Reaction reaction)
        {
            _reactionBehaviour.ShowReaction(reaction);
        }
    }
}