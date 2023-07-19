using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem;

namespace CryptoQuest.Gameplay.Battle.Core
{
    [Serializable]
    public class SkillParameters : AbilityParameters
    {
        public int ContinuesTurn = 1;
        public float BasePower = 10;
    }
}