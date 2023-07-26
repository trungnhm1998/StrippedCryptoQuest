using CryptoQuest.Gameplay.Quest.Dialogue;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.UI
{
    public class DialogueEventChannelSO : ScriptableObject
    {
        public event UnityAction<IDialogueDef> ShowEvent;

        public void Show(IDialogueDef dialogue) => OnShow(dialogue);

        private void OnShow(IDialogueDef dialogue)
        {
            if (dialogue == null)
            {
                Debug.LogWarning("A request for showing dialog has been made, but not dialogue were provided.");
                return;
            }

            if (ShowEvent == null)
            {
                Debug.LogWarning("A request for showing dialog has been made, but no one listening.");
                return;
            }

            ShowEvent.Invoke(dialogue);
        }
    }
}