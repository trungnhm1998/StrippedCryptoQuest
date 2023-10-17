using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Character
{
    public class NPCBehaviour : MonoBehaviour, IInteractable
    {
        public static event UnityAction<NPCBehaviour> Interacted;

        // TODO: move to generic InteractableBehaviour, should also have the collider in that prefab
        [SerializeField] private UnityEvent _interacted;

        [SerializeField] private ReactionBehaviour _reactionBehaviour;


        /// <summary>
        /// This will cause a stutter on scene load. Luckily, we have a fade in effect.
        /// </summary>
        private void Awake()
        {
            // var allBehaviours = GetComponents<ICharacterBehaviour>(); // TODO: this might be a better way to get all the behaviours
            // due to sear amount of NPCs in the game, use TryGetComponent instead of GetComponent then add NullDialogueController
        }

        private void OnValidate()
        {
            if (_interacted.GetPersistentEventCount() <= 2) return;
            for (int index = 2; index < _interacted.GetPersistentEventCount(); index++)
            {
                _interacted.SetPersistentListenerState(index, UnityEventCallState.Off);
            }
        }

        public void Interact()
        {
            Interacted?.Invoke(this);
            _interacted.Invoke();
        }

        public void ShowReaction(Reaction reaction)
        {
            _reactionBehaviour.ShowReaction(reaction);
        }
    }
}