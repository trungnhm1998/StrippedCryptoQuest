using CryptoQuest.AbilitySystem;
using CryptoQuest.Battle.Events;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;

namespace CryptoQuest.Battle.Components
{
    public class AbnormalMessenger : CharacterComponentBase
    {
        private ITagAssetProvider _tagAssetProvider;

        private ITagAssetProvider TagAssetProvider =>
            _tagAssetProvider ??= ServiceProvider.GetService<ITagAssetProvider>();
        public override void Init()
        {
            Character.AbilitySystem.TagSystem.TagAdded += LogEffectAdded;
            Character.AbilitySystem.TagSystem.TagRemoved += LogEffectRemoved;
        }

        private void OnDestroy()
        {
            if (Character.IsValid() == false) return;
            Character.AbilitySystem.TagSystem.TagAdded -= LogEffectAdded;
            Character.AbilitySystem.TagSystem.TagRemoved -= LogEffectRemoved;
        }

        private void LogEffectAdded(params TagScriptableObject[] tagScriptableObjects)
        {
            foreach (var tagScriptableObject in tagScriptableObjects)
            {
                if (!TagAssetProvider.TryGetTagAsset(tagScriptableObject, out var tagAsset)) continue;
                BattleEventBus.RaiseEvent(new EffectAddedEvent()
                {
                    Character = Character,
                    Reason = tagAsset.AddedMessage,
                });
            }
        }

        private void LogEffectRemoved(params TagScriptableObject[] tagScriptableObjects)
        {
            foreach (var tagScriptableObject in tagScriptableObjects)
            {
                if (!TagAssetProvider.TryGetTagAsset(tagScriptableObject, out var tagAsset)) continue;
                if (Character.HasTag(tagScriptableObject)) continue;
                BattleEventBus.RaiseEvent(new EffectRemovedEvent()
                {
                    Character = Character,
                    Reason = tagAsset.RemoveMessage,
                });
            }
        }
    }
}