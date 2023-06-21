using System.Collections.Generic;
using UnityEngine;

namespace Core.Runtime.Events.ScriptableObjects.Dialogs
{
    [CreateAssetMenu(menuName = "Events/Dialog", order = 0)]
    public class DialogsScriptableObject : ScriptableObject
    {
        public List<string> messages;
    }
    
}