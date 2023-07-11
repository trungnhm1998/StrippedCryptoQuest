using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class NpcBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueScriptableObject _dialogue;

        [Header("Raise on")]
        [SerializeField] private DialogEventChannelSO _dialogEventChannel;
        [SerializeField] private DialogueEventChannelSO _dialogueEventChannel;

        public void Interact()
        {
            // _dialogEventChannel.Show(_dialogue);
            _dialogueEventChannel.Show(_dialogue);
        }
    }
}