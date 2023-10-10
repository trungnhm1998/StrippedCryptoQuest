using CryptoQuest.Battle.Events;
using CryptoQuest.Character.Tag;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class AbnormalBehavior : CharacterComponentBase
    {
        private Character _abnormalCharacter;

        public override void Init()
        {
            _abnormalCharacter = GetComponent<Character>();
            _abnormalCharacter.AbilitySystem.TagSystem.TagAdded += AddAbnormal;
            _abnormalCharacter.AbilitySystem.TagSystem.TagRemoved += RemoveAbnormal;
        }

        private void OnDestroy()
        {
            if (Character.IsValid() == false) return;
            _abnormalCharacter.AbilitySystem.TagSystem.TagAdded -= AddAbnormal;
            _abnormalCharacter.AbilitySystem.TagSystem.TagRemoved -= RemoveAbnormal;
        }

        private void AddAbnormal(TagScriptableObject[] tagsAdded)
        {
            foreach (var abnormal in tagsAdded)
            {
                if (abnormal.IsChildOf(TagsDef.Abnormal) == false) continue;
                if (abnormal is not TagSO abnormalTag) continue;
                BattleEventBus.RaiseEvent(new EffectAffectingEvent()
                {
                    Character = _abnormalCharacter,
                    Reason = abnormalTag.AddedMessage
                });
            }
        }

        private void RemoveAbnormal(TagScriptableObject[] tagsAdded)
        {
            if (tagsAdded.Contains(TagsDef.Dead))
            {
                return;
            }

            foreach (var abnormal in tagsAdded)
            {
                if (abnormal.IsChildOf(TagsDef.Abnormal) == false) continue;
                if (abnormal is not TagSO abnormalTag) continue;
                BattleEventBus.RaiseEvent(new EffectAffectingEvent()
                {
                    Character = _abnormalCharacter,
                    Reason = abnormalTag.RemoveMessage
                });
            }
        }
    }
}