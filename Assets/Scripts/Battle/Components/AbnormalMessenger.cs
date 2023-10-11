using System;
using CryptoQuest.Battle.Events;
using CryptoQuest.Character.Tag;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;

namespace CryptoQuest.Battle.Components
{
    public class AbnormalMessenger : CharacterComponentBase
    {
        public override void Init()
        {
            Character.AbilitySystem.EffectSystem.EffectAdded += LogEffectAdded;
            Character.AbilitySystem.EffectSystem.EffectRemoved += LogEffectRemoved;
        }

        private void OnDestroy()
        {
            if (Character.IsValid() == false) return;
            Character.AbilitySystem.EffectSystem.EffectAdded -= LogEffectAdded;
            Character.AbilitySystem.EffectSystem.EffectRemoved -= LogEffectRemoved;
        }

        private void LogEffectRemoved(ActiveEffectSpecification activeEffect)
        {
            RaiseEventForTags(activeEffect.GrantedTags, tag =>
            {
                BattleEventBus.RaiseEvent(new EffectRemovedEvent()
                {
                    Character = Character,
                    Reason = tag.RemoveMessage,
                });
            });
        }

        private void LogEffectAdded(ActiveEffectSpecification activeEffect)
        {
            RaiseEventForTags(activeEffect.GrantedTags, tag =>
            {
                BattleEventBus.RaiseEvent(new EffectAddedEvent()
                {
                    Character = Character,
                    Reason = tag.AddedMessage,
                });
            });
        }

        private static void RaiseEventForTags(TagScriptableObject[] tags, Action<TagSO> action)
        {
            foreach (var tag in tags)
            {
                if (tag is not TagSO tagSO) continue;
                action(tagSO);
            }
        }
    }
}