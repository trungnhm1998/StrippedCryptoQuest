using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Characters.Events
{
    [CreateAssetMenu(menuName = "Dialogue System/Dialog Event")]
    public class DialogEventScriptableObject : ScriptableObject
    {
        public event UnityAction<LocalizedString> ShowEvent = delegate { };

        public void OnShow(LocalizedString getLine)
        {
            if (getLine.IsEmpty)
            {
                Debug.LogWarning("A request for showing dialog has been made, but no one listening.");
                return;
            }

            if (ShowEvent == null)
            {
                Debug.LogWarning("A request for showing dialog has been made, but no one listening.");
                return;
            }

            ShowEvent.Invoke(getLine);
        }
    }
}