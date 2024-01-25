using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class CastSkillFailedLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _failedMessage;
        private TinyMessageSubscriptionToken _failedEvent;

        private void Awake()
        {
            _failedEvent = BattleEventBus.SubscribeEvent<CastSkillFailedEvent>(LogCastFailed);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_failedEvent);
        }
        
        private void LogCastFailed(CastSkillFailedEvent ctx)
        {
            var castMessage = new LocalizedString(_failedMessage.TableReference, _failedMessage.TableEntryReference);
            var localizedSkillName = ctx.Skill.SkillName;
            castMessage.Add(Constants.ABILITY_NAME, localizedSkillName.IsEmpty ? 
                new StringVariable()
                {
                    Value = ctx.Skill.name
                }
                : localizedSkillName);
            Logger.QueueLog(castMessage);
        }
    }
}