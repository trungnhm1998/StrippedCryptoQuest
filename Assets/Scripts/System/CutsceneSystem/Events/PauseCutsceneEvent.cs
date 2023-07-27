using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.System.CutsceneSystem.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Cutscenes/Events/PauseCutscene")]
    public class PauseCutsceneEvent : ScriptableObject
    {
        public event UnityAction<bool> PauseCutsceneRequested;
        
        public void RaiseEvent(bool pause) =>
            PauseCutsceneRequested?.Invoke(pause);
    }
}