using CryptoQuest.System.Dialogue.YarnManager;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.System.Dialogue.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Dialogue/Events/YarnProject Config Event")]
    public class YarnProjectConfigEvent : ScriptableObject
    {
        public event UnityAction<YarnProjectConfigSO> ConfigRequested;

        public void RaiseEvent(YarnProjectConfigSO yarnProjectConfig) =>
            ConfigRequested?.Invoke(yarnProjectConfig);
    }
}