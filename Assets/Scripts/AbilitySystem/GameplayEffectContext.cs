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
        [SerializeField] private int _turns = 3;
        public int Turns => _turns;
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