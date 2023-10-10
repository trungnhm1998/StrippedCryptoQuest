using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Ability;

namespace CryptoQuest.Character
{
    public interface ISkillsProvider
    {
        void GetSkills(HeroBehaviour hero, Action<List<CastSkillAbility>> skillsLoadedCallback);
    }
}