using CryptoQuest.Events.UI;
using IndiGames.Core.Events.ScriptableObjects.Dialogs;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class NpcBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueScriptableObject _dialogue;

        [Header("Raise on")]
        [SerializeField] private DialogEventChannelSO _dialogEventChannel;

        public void Interact()
        {
            _dialogEventChannel.Show(_dialogue);
        }
    }
}