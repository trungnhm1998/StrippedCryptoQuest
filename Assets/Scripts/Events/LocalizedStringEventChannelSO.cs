using UnityEngine;
using UnityEngine.Localization;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Events/Localized String Event Channel")]
    public class LocalizedStringEventChannelSO : GenericEventChannelSO<LocalizedString>
    {
#if UNITY_EDITOR
        [SerializeField] private LocalizedString _debugValue;
        public LocalizedString DebugValue => _debugValue;
#endif

        public override void RaiseEvent(LocalizedString value)
        {
            base.RaiseEvent(value);
#if UNITY_EDITOR
            _debugValue = value;
#endif
        }

        protected override void OnRaiseEvent(LocalizedString value)
        {
            base.OnRaiseEvent(value);
#if UNITY_EDITOR
            _debugValue = value;
#endif
        }
    }
}