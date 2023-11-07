using System;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities.MultipleContextAbility
{
    [Serializable]
    public class ContextEffectsMapping
    {
        [field: SerializeReference, SubclassSelector]
        public IContextSelector ContextSelector { get; private set; } = new DefaultContext();

        [field: SerializeField]
        public EEffectTargetType TargetType { get; private set; }
        
        [field: SerializeField]
        public GameplayEffectDefinition[] Effects { get; private set; }
    }

    public interface IContextSelector
    {
        GameplayEffectContext GetContext(CastSkillAbility castAbility);
    }

    [Serializable]
    public class DefaultContext : IContextSelector
    {
        public GameplayEffectContext GetContext(CastSkillAbility castAbility)
            => castAbility.Context;
    }

    [Serializable]
    public class CustomContext : IContextSelector
    {
        [SerializeField] private GameplayEffectContext _customContext;
        
        public GameplayEffectContext GetContext(CastSkillAbility castAbility)
            => _customContext;
    }

    public enum EEffectTargetType
    {
        OtherTarget = 0,
        SelfTarget = 1
    }
}