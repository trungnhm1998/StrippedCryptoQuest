using IndiGames.Core.UI.FadeController;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events
{
    [CreateAssetMenu(menuName = "IndiGames Core/Events/ConfigSO Event Channel")]
    public class ConfigSOEventChannelSO : ScriptableObject
    {
        public UnityAction<FadeConfigSO> EventRaised;

        public void RaiseEvent(FadeConfigSO configSO)
        {
            OnRaiseEvent(configSO);
        }

        private void OnRaiseEvent(FadeConfigSO configSO)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke(configSO);
        }
    }
}