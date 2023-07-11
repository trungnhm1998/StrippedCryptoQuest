using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class NpcBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueScriptableObject _dialogue;

        [Header("Raise on")]
        [SerializeField] private DialogueEventChannelSO _dialogEventChannel;

        public void Interact()
        {
            _dialogEventChannel.Show(_dialogue);
        }
    }
}