using CryptoQuest.Battle.Character;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
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
        [SerializeField] private AttributeChangeEvent _heroHealthChangeEvent;
        private TinyMessageSubscriptionToken _token;

        private void Awake()
        {
            _token = BattleEventBus.SubscribeEvent<LoadedEvent>(RegisterEvent);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_token);
            _heroHealthChangeEvent.Changed -= LogDamage;
        }

        private void RegisterEvent(LoadedEvent ctx)
        {
            _heroHealthChangeEvent.Changed += LogDamage;
        }

        private void LogDamage(AttributeSystemBehaviour owner, AttributeValue oldVal,
            AttributeValue newVal)
        {
            LocalizedString localizedMessage;
            var damage = Mathf.RoundToInt(oldVal.CurrentValue - newVal.CurrentValue);
            if (damage <= 0)
            {
                localizedMessage = _noDamageMessage;
            }
            else
            {
                localizedMessage = _damageMessage;
                localizedMessage.Add(Constants.DAMAGE_NUMBER, new FloatVariable()
                {
                    Value = damage
                });
            }

            localizedMessage.Add(Constants.CHARACTER_NAME, owner.GetComponent<Components.Character>().LocalizedName);

            Logger.AppendLog(localizedMessage);
        }
    }
}