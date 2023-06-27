using System.Collections.Generic;
using UnityEngine;

namespace Core.Runtime.Events.ScriptableObjects.Dialogs
{
    [CreateAssetMenu(menuName = "Dialogue System/Dialog")]
    public class DialogsScriptableObject : ScriptableObject
    {
        public List<string> Messages;
    }
}