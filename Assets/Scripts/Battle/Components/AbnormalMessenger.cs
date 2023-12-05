﻿using CryptoQuest.AbilitySystem;
using CryptoQuest.Battle.Events;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;

namespace CryptoQuest.Battle.Components
{
    public class AbnormalMessenger : CharacterComponentBase
    {
        protected override void OnInit()
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
                BattleEventBus.RaiseEvent(new EffectAddedEvent()
                {
                    Tag = tagScriptableObject,
                    Character = Character,
                });
            }
        }

        private void LogEffectRemoved(params TagScriptableObject[] tagScriptableObjects)
        {
            foreach (var tagScriptableObject in tagScriptableObjects)
            {
                BattleEventBus.RaiseEvent(new EffectRemovedEvent()
                {
                    Tag = tagScriptableObject,
                    Character = Character,
                });
            }
        }
    }
}