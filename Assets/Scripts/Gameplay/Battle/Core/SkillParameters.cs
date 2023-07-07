using IndiGames.GameplayAbilitySystem.AbilitySystem;
using System;

namespace CryptoQuest.Gameplay.Battle
{
    [Serializable]
    public class SkillParameters : AbilityParameters
    {
        public int ContinuesTurn = 1;
        public float BasePower = 10;
    }
}