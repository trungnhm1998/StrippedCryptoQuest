using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Character
{
    [CreateAssetMenu(fileName = "HeroSkillSetSO", menuName = "Gameplay/Character/Hero Skill Set")]
    public class HeroSkillSetSO : ScriptableObject
    {
        [SerializeField] private HeroSkillsSet[] _skillMappings = Array.Empty<HeroSkillsSet>();
        public HeroSkillsSet[] SkillMappings => _skillMappings;
    }
}