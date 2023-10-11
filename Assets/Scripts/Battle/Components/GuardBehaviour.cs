using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class GuardBehaviour : CharacterComponentBase
    {
        [SerializeField] private GuardAbility _guardAbility;

        private GuardAbilitySpec _spec;
        private TinyMessageSubscriptionToken _roundEndedEventToken;

        private void OnEnable() =>
            _roundEndedEventToken = BattleEventBus.SubscribeEvent<RoundEndedEvent>(RemoveGuardTag);

        private void OnDisable() => BattleEventBus.UnsubscribeEvent(_roundEndedEventToken);

        public override void Init() => _spec = Character.AbilitySystem.GiveAbility<GuardAbilitySpec>(_guardAbility);

        public void GuardUntilEndOfTurn()
        {
            _spec.GuardActivatedEvent += OnGuardActivated;
            _spec.TryActiveAbility();
        }

        private void RemoveGuardTag(RoundEndedEvent eventObject)
        {
            _spec.EndAbility();
            _spec.GuardActivatedEvent -= OnGuardActivated;
        }

        private void OnGuardActivated()
        {
            BattleEventBus.RaiseEvent(new GuardedEvent()
            {
                Character = Character
            });
        }
    }
}