using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class ReflectDamageLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _reflectDamageMessage;
        private TinyMessageSubscriptionToken _reflectDamageEvent;

        private void Awake()
        {
            _reflectDamageEvent = BattleEventBus.SubscribeEvent<ReflectDamageEvent>(LogReflectDamage);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_reflectDamageEvent);
        }

        private void LogReflectDamage(ReflectDamageEvent ctx)
        {
            var msg = new LocalizedString(_reflectDamageMessage.TableReference,
                _reflectDamageMessage.TableEntryReference);
            msg.Add(Constants.CHARACTER_NAME, new StringVariable()
            {
                Value = ctx.Character.DisplayName
            });

            Logger.QueueLog(msg);
        }
    }
}