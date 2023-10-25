using System.Collections;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    /// <summary>
    /// Apply an instant effect to owner when the action is done
    /// </summary>
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Passive/Post Action With Effect Passive",
        fileName = "PostActionPassive", order = 0)]
    public class PostActionPassiveAbility : PassiveAbility
    {
        [field: SerializeField] public int vfxId { get; private set; } = -1;
        [SerializeField] private GameplayEffectContext _context;
        public GameplayEffectContext Context => _context;
        [SerializeField] private GameplayEffectDefinition _effect; // Should be infinite type
        public GameplayEffectDefinition EffectToApply => _effect;

        protected override GameplayAbilitySpec CreateAbility()
            => new PostActionPassiveAbilitySpec(this);
    }

    public class PostActionPassiveAbilitySpec : PassiveAbilitySpec
    {
        private readonly PostActionPassiveAbility _def;

        public PostActionPassiveAbilitySpec(PostActionPassiveAbility def) => _def = def;

        protected override IEnumerator OnAbilityActive()
        {
            if (_def.Context.Parameters.targetAttribute.Attribute == null) yield break;
            Character.TurnEnded += ApplyEffectPostAction;
        }

        private void ApplyEffectPostAction()
        {
            var contextHandle = new GameplayEffectContextHandle(_def.Context);
            var effectSpec = _def.EffectToApply.CreateEffectSpec(Owner, contextHandle);
            BattleEventBus.RaiseEvent(new PlayVfxEvent(_def.vfxId));
            Owner.ApplyEffectSpecToSelf(effectSpec);
        }

        protected override void OnAbilityEnded() => Character.TurnEnded -= ApplyEffectPostAction;
    }
}