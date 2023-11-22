using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using UnityEngine;
using CoreEffectContext = IndiGames.GameplayAbilitySystem.EffectSystem.GameplayEffectContext;

namespace CryptoQuest.AbilitySystem
{
    [Serializable]
    public class GameplayEffectContext : CoreEffectContext
    {
        public GameplayEffectContext(SkillInfo skillInfo)
        {
            _skillInfo = skillInfo;
        }

        [SerializeField] private SkillInfo _skillInfo;
        public int Turns => _skillInfo.SkillParameters.ContinuesTurn;
        public CastSkillAbility Skill { get; set; }
        public SkillInfo SkillInfo => _skillInfo;
        public SkillParameters Parameters => _skillInfo.SkillParameters;

        public static GameplayEffectContext ExtractEffectContext(GameplayEffectContextHandle handle)
        {
            var baseContext = handle.Get();
            if (baseContext is GameplayEffectContext context)
                return context;

            return null;
        }
    }
}