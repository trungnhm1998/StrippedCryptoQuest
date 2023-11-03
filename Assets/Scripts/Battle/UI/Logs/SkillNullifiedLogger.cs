using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class SkillNullifiedLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _nullifySkillLogMessage;
        private TinyMessageSubscriptionToken _nullifySkillEvent;

        private void Awake()
        {
            _nullifySkillEvent = BattleEventBus.SubscribeEvent<CastInvalidEvent>(LogSkillNullified);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_nullifySkillEvent);
        }

        private void LogSkillNullified(CastInvalidEvent ctx)
        {
            var msg = new LocalizedString(_nullifySkillLogMessage.TableReference,
                _nullifySkillLogMessage.TableEntryReference)
            {
                {
                    Constants.ABILITY_NAME, ctx.Skill.Def.SkillName
                },
                {
                    Constants.CHARACTER_NAME, new StringVariable()
                    {
                        Value = ctx.Character.DisplayName
                    }
                }
            };

            Logger.QueueLog(msg);
        }
    }
}