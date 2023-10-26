using System.Collections;
using CryptoQuest.Battle.Components;
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
            if (!Character.TryGetComponent(out CommandExecutor commandExecutor)) yield break;
            commandExecutor.PostExecuteCommand += ApplyEffectPostAction;
        }

        private void ApplyEffectPostAction()
        {
            var contextHandle = new GameplayEffectContextHandle(SkillContext);
            var effectSpec = _def.EffectToApply.CreateEffectSpec(Owner, contextHandle);
            BattleEventBus.RaiseEvent(new PlayVfxEvent(_def.vfxId));
            Owner.ApplyEffectSpecToSelf(effectSpec);
        }

        protected override void OnAbilityEnded()
        {
            if (!Character.TryGetComponent(out CommandExecutor commandExecutor)) return;
            commandExecutor.PostExecuteCommand -= ApplyEffectPostAction;
        }
    }
}