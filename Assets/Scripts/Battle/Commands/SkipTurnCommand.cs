using CryptoQuest.Battle.Events;
using CryptoQuest.Character.Tag;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class SkipTurnCommand : ICommand
    {
        private readonly Components.Character _character;
        public SkipTurnCommand(Components.Character character)
        {
            _character = character;
        }

        public IEnumerator Execute()
        {
            Debug.Log($"{_character.DisplayName} Skip turn");
            var abnormals = _character.AbilitySystem.EffectSystem.GrantedTags;
            foreach (var abnormal in abnormals)
            {
                if (abnormal.IsChildOf(TagsDef.Abnormal) == false) continue;
                if (abnormal is not TagSO abnormalTag) continue;
                BattleEventBus.RaiseEvent(new AbnormalEvent()
                {
                    Character = _character,
                    Reason = abnormalTag.SkipTurnMessage
                });
            }
            yield break;
        }
    }
}