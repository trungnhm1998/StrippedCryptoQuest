using System.Collections.Generic;
using CryptoQuest.Character.Ability;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class HeroSkills : CharacterComponentBase
    {
        [SerializeField] private List<CastableAbility> _skills;
        public IReadOnlyList<CastableAbility> Skills => _skills;

        /// <summary>
        /// Based on character level and class, get all skills that character can use
        /// </summary>
        public override void Init() { }
    }
}