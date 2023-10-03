using CryptoQuest.Battle.Events;
using CryptoQuest.Character.Ability;
using CryptoQuest.Character.Tag;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class GuardBehaviour : CharacterComponentBase
    {
        [SerializeField] private GuardAbility _guardAbility;

        private GuardAbilitySpec _spec;
        private TinyMessageSubscriptionToken _roundEndedEventToken;

        private void OnEnable()
        {
            _roundEndedEventToken = BattleEventBus.SubscribeEvent<RoundEndedEvent>(RemoveGuardTag);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_roundEndedEventToken);
        }

        public override void Init()
        {
            _spec = Character.AbilitySystem.GiveAbility<GuardAbilitySpec>(_guardAbility);
        }
        
        public void GuardUntilEndOfTurn()
        {
            _spec.TryActiveAbility();
        }

        private void RemoveGuardTag(RoundEndedEvent eventObject)
        {
            var tagSystem = Character.AbilitySystem.TagSystem;
            tagSystem.RemoveTags(new TagScriptableObject[] {TagsDef.Guard});
        }
    }
}