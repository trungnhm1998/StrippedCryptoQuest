using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest
{
    public enum ECharacterSkill
    {
        MainCharacter = 0,
        Hero = 1,
        Archer = 2,
        Priest = 3
    }
    
    [CreateAssetMenu(menuName = "Crypto Quest/Ability/Skill")]
    public class SkillSO : ScriptableObject
    {
        public ECharacterSkill CharacterSkills;
        public Sprite Icon;
        public LocalizedString Name;
        public LocalizedString Description;
        public float Mana;
    }
}
