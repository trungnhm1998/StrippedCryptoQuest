using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class CastSkillLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _castSkillMessage;
        private TinyMessageSubscriptionToken _castSkillEvent;

        private void OnEnable()
        {
            _castSkillEvent = BattleEventBus.SubscribeEvent<CastingSkillEvent>(LogCastSkill);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_castSkillEvent);
        }

        private void LogCastSkill(CastingSkillEvent skillEvent)
        {
            var castMessage =
                new LocalizedString(_castSkillMessage.TableReference, _castSkillMessage.TableEntryReference)
                {
                    {
                        Constants.CHARACTER_NAME, new StringVariable()
                        {
                            Value = skillEvent.Character.DisplayName
                        }
                    }
                };
            var localizedSkillName = skillEvent.Skill.Parameters.SkillName;
            if (localizedSkillName.IsEmpty)
                castMessage.Add(Constants.SKILL_NAME, new StringVariable()
                {
                    Value = skillEvent.Skill.name
                });
            else
                castMessage.Add(Constants.SKILL_NAME, localizedSkillName);
            Logger.QueueLog(castMessage);
        }
    }
}