using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.System.CutScene.Events
{
    [CreateAssetMenu(menuName = "Core/Events/Void Event Channel")]
    public class CutsceneEventChannelSO : ScriptableObject
    {
        public UnityAction EventRaised;

        public void RaiseEvent()
        {
            OnRaiseEvent();
        }

        private void OnRaiseEvent()
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke();
        }
    }
}