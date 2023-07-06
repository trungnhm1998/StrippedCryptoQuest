using IndiGames.GameplayAbilitySystem.AbilitySystem;
using System;

namespace CryptoQuest.Gameplay.Battle
{
    [Serializable]
    public class SkillParameters : AbilityParameters
    {
        public int continuesTurn = 1;
        public float basePower = 10;
    }
}