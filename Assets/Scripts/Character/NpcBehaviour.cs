using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class NpcBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueScriptableObject _dialogue;
        private NPCFacingDirection _facingDirection;

        [Header("Raise on")]
        [SerializeField] private DialogueEventChannelSO _dialogEventChannel;

        private void Awake()
        {
            _facingDirection = gameObject.GetComponent<NPCFacingDirection>();
        }
        public void Interact()
        {
            _dialogEventChannel.Show(_dialogue);
            _facingDirection.NPCInteract();
        }
    }
}