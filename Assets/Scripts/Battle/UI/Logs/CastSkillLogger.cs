using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class CastSkillLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _castSkillMessage;
        private TinyMessageSubscriptionToken _castSkillEvent;

        private void OnEnable()
        {
            _castSkillEvent = BattleEventBus.SubscribeEvent<CastSkillEvent>(LogCastSkill);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_castSkillEvent);
        }

        private void LogCastSkill(CastSkillEvent skillEvent)
        {
            var castMessage = _castSkillMessage;
            castMessage.Add(Constants.CHARACTER_NAME, skillEvent.Character.LocalizedName);
            castMessage.Add(Constants.SKILL_NAME, skillEvent.Skill.Parameters.SkillName);
            Logger.AppendLog(castMessage);
        }
    }
}
