using CryptoQuest.Battle.Events;
using CryptoQuest.Character.Tag;
using DG.Tweening;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class HeroDeadBehavior : CharacterComponentBase
    {
        [SerializeField] private float _delayNotify = 0.5f;
        private HeroBehaviour _heroBehaviour;
        public override void Init()
        {
            _heroBehaviour = GetComponent<HeroBehaviour>();
            _heroBehaviour.AbilitySystem.TagSystem.TagAdded += CheckIfDeadTagAdded;
        }

        private void OnDestroy()
        {
            if (IsValid()) _heroBehaviour.AbilitySystem.TagSystem.TagAdded -= CheckIfDeadTagAdded;
        }

        private void CheckIfDeadTagAdded(TagScriptableObject[] tagsAdded)
        {
            if (tagsAdded.Contains(TagsDef.Dead))
            {
                DOTween.Sequence().SetDelay(_delayNotify).OnComplete(OnDead).Play();
            }
        }

        private void OnDead()
        {
            BattleEventBus.RaiseEvent(new DeadEvent()
            {
                Character = _heroBehaviour
            });
        }    
    }
}
