using CryptoQuest.Map;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Events/Map Path Event Channel")]
    public class MapPathEventChannelSO : ScriptableObject
    {
        public UnityAction<MapPathSO> EventRaised;

        public void RaiseEvent(MapPathSO path)
        {
            OnRaiseEvent(path);
        }

        private void OnRaiseEvent(MapPathSO path)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke(path);
        }
    }
}