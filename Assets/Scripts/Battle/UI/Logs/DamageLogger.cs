using System;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.AbilitySystem.Executions;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class DamageLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _damageMessage;
        [SerializeField] private LocalizedString _noDamageMessage;
        [SerializeField] private DealingDamageEvent _receivedDamageEvent;
        private TinyMessageSubscriptionToken _damageOverTimeEvent;

        private void Awake()
        {
            _receivedDamageEvent.DamageDealt += DamageDealt_LogDamage;
            _damageOverTimeEvent = BattleEventBus.SubscribeEvent<DamageOverTimeEvent>(LogDoT);
        }

        private void OnDestroy()
        {
            _receivedDamageEvent.DamageDealt -= DamageDealt_LogDamage;
            BattleEventBus.UnsubscribeEvent(_damageOverTimeEvent);
        }
        
        private void LogDoT(DamageOverTimeEvent ctx)
        {
            if (ctx.AffectingAttribute != AttributeSets.Health) return;
            LogDamage(ctx.Character, ctx.Magnitude);
        }

        private void DamageDealt_LogDamage(Components.Character dealer, Components.Character receiver, float damage) => LogDamage(receiver, damage);

        private void LogDamage(Components.Character receiver, float damage)
        {
            LocalizedString msg;

            if (Mathf.Approximately(damage, 0f))
                msg =
                    new LocalizedString(_noDamageMessage.TableReference, _noDamageMessage.TableEntryReference);
            else
            {
                msg = new LocalizedString(_damageMessage.TableReference, _damageMessage.TableEntryReference)
                {
                    {
                        Constants.DAMAGE_NUMBER, new FloatVariable()
                        {
                            Value = Math.Abs(damage)
                        }
                    }
                };
            }

            msg.Add(Constants.CHARACTER_NAME, new StringVariable()
            {
                Value = receiver.DisplayName
            });

            Logger.QueueLog(msg);
        }
    }
}