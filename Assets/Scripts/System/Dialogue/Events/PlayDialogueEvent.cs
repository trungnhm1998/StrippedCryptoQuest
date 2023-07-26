using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.System.Dialogue.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Dialogue/Events/PlayDialogue")]
    public class PlayDialogueEvent : ScriptableObject
    {
        public event UnityAction<string> PlayDialogueRequested;

        public void RaiseEvent(string yarnNodeName) =>
            PlayDialogueRequested?.Invoke(yarnNodeName);
    }
}