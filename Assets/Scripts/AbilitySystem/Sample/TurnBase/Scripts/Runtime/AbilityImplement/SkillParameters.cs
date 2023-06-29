using Indigames.AbilitySystem;
using System;

namespace Indigames.AbilitySystem.Sample
{
    [Serializable]
    public class SkillParameters : AbilityParameters
    {
        public int continuesTurn = 1;
        public float basePower = 10;
    }
}