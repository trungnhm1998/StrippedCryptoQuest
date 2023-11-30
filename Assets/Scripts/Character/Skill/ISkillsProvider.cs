using System;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;

namespace CryptoQuest.Character.Skill
{
    public interface ISkillsProvider
    {
        void GetSkills(HeroBehaviour hero, Action<List<CastSkillAbility>> skillsLoadedCallback);
    }
}