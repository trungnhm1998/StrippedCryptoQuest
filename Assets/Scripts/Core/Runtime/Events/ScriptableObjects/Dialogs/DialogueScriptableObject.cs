using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Core.Runtime.Events.ScriptableObjects.Dialogs
{
    [CreateAssetMenu(menuName = "Dialogue System/Dialog")]
    public class DialogueScriptableObject : ScriptableObject
    {
        public List<LocalizedString> Lines;

        public LocalizedString GetLine(int currentIndex)
        {
            return Lines[currentIndex];
        }
    }
}