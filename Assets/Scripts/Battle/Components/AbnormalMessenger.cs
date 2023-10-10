using System;
using CryptoQuest.Battle.Events;
using CryptoQuest.Character.Tag;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;

namespace CryptoQuest.Battle.Components
{
    public class AbnormalMessenger : CharacterComponentBase
    {
        private TagSystemBehaviour _tagSystem;

        public override void Init()
        {
            _tagSystem = GetComponent<TagSystemBehaviour>();
            _tagSystem.TagAdded += OnTagAdded;
            _tagSystem.TagRemoved += OnTagRemoved;
        }

        private void OnDestroy()
        {
            if (Character.IsValid() == false) return;
            _tagSystem.TagAdded -= OnTagAdded;
            _tagSystem.TagRemoved -= OnTagRemoved;
        }

        private void OnTagRemoved(TagScriptableObject[] tagScriptableObjects)
        {
            RaiseEventForTags(tagScriptableObjects, tag =>
            {
                BattleEventBus.RaiseEvent(new EffectRemovedEvent()
                {
                    Character = Character,
                    Reason = tag.RemoveMessage,
                });
            });
        }

        private void OnTagAdded(TagScriptableObject[] tagScriptableObjects)
        {
            RaiseEventForTags(tagScriptableObjects, tag =>
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