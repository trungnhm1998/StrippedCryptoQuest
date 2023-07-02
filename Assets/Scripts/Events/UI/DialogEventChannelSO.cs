using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.UI
{
    [CreateAssetMenu(menuName = "Create DialogEventChannelSO", fileName = "DialogEventChannelSO", order = 0)]
    public class DialogEventChannelSO : ScriptableObject
    {
        public event UnityAction<DialogueScriptableObject> ShowEvent;
        public event UnityAction HideEvent;

        public void Show(DialogueScriptableObject dialogue)
        {
            OnShow(dialogue);
        }

        public void Hide()
        {
            OnHide();
        }

        private void OnHide()
        {
            if (HideEvent == null)
            {
                Debug.LogWarning("A request for hiding dialog has been made, but no one listening.");
                return;
            }

            HideEvent.Invoke();
        }

        private void OnShow(DialogueScriptableObject dialogue)
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