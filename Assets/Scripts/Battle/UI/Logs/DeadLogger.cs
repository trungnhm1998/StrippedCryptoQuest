using CryptoQuest.Character.Attributes;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class DeadLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _deadLogMessage;
        [SerializeField] private ApplyTagIfConditionIsMet _deadTagApplied;
        private TinyMessageSubscriptionToken _deadEvent;

        private void OnEnable()
        {
            _deadTagApplied.TagAdded += LogDead;
        }

        private void OnDisable()
        {
            _deadTagApplied.TagAdded -= LogDead;
        }

        private void LogDead(ApplyTagIfConditionIsMet.Context ctx)
        {
            var castMessage = _deadLogMessage;
            castMessage.Add(Constants.CHARACTER_NAME, ctx.Character.LocalizedName);
            Logger.AppendLog(castMessage);
        }
    }
}
