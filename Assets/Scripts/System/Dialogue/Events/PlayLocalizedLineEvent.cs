using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.System.Dialogue.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Dialogue/Events/PlayLocalizedLine")]
    public class PlayLocalizedLineEvent : ScriptableObject
    {
        public event UnityAction<LocalizedString> PlayDialogueRequested;

        public void RaiseEvent(LocalizedString localizedLine) =>
            PlayDialogueRequested?.Invoke(localizedLine);
    }
}